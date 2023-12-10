using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WtfDll.Utils
{
    /// <summary>
    /// Defines the <see cref="ListViewExtensions" />.
    /// </summary>
    public class ListViewExtensions
    {
        /// <summary>
        /// The GetSelectedItemsBinder.
        /// </summary>
        /// <param name="obj">The obj<see cref="DependencyObject"/>.</param>
        /// <returns>The <see cref="SelectedItemsBinder"/>.</returns>
        private static SelectedItemsBinder GetSelectedItemsBinder(DependencyObject obj)
        {
            return (SelectedItemsBinder)obj.GetValue(SelectedItemsBinderProperty);
        }

        /// <summary>
        /// The SetSelectedItemsBinder.
        /// </summary>
        /// <param name="obj">The obj<see cref="DependencyObject"/>.</param>
        /// <param name="items">The items<see cref="SelectedItemsBinder"/>.</param>
        private static void SetSelectedItemsBinder(DependencyObject obj, SelectedItemsBinder items)
        {
            obj.SetValue(SelectedItemsBinderProperty, items);
        }

        /// <summary>
        /// Defines the SelectedItemsBinderProperty.
        /// </summary>
        private static readonly DependencyProperty SelectedItemsBinderProperty = DependencyProperty.RegisterAttached("SelectedItemsBinder", typeof(SelectedItemsBinder), typeof(ListViewExtensions));

        /// <summary>
        /// Defines the SelectedItemsProperty.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(ListViewExtensions),
            new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

        /// <summary>
        /// The OnSelectedItemsChanged.
        /// </summary>
        /// <param name="o">The o<see cref="DependencyObject"/>.</param>
        /// <param name="value">The value<see cref="DependencyPropertyChangedEventArgs"/>.</param>
        private static void OnSelectedItemsChanged(DependencyObject o, DependencyPropertyChangedEventArgs value)
        {
            var oldBinder = GetSelectedItemsBinder(o);
            if (oldBinder != null)
                oldBinder.UnBind();

            SetSelectedItemsBinder(o, new SelectedItemsBinder((ListView)o, (IList)value.NewValue));
            GetSelectedItemsBinder(o).Bind();
        }

        /// <summary>
        /// The SetSelectedItems.
        /// </summary>
        /// <param name="elementName">The elementName<see cref="Selector"/>.</param>
        /// <param name="value">The value<see cref="IEnumerable"/>.</param>
        public static void SetSelectedItems(Selector elementName, IEnumerable value)
        {
            elementName.SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// The GetSelectedItems.
        /// </summary>
        /// <param name="elementName">The elementName<see cref="Selector"/>.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        public static IEnumerable GetSelectedItems(Selector elementName)
        {
            return (IEnumerable)elementName.GetValue(SelectedItemsProperty);
        }
    }

    /// <summary>
    /// Defines the <see cref="SelectedItemsBinder" />.
    /// </summary>
    public class SelectedItemsBinder
    {
        /// <summary>
        /// Defines the _listView.
        /// </summary>
        private ListView _listView;

        /// <summary>
        /// Defines the _collection.
        /// </summary>
        private IList _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedItemsBinder"/> class.
        /// </summary>
        /// <param name="listView">The listView<see cref="ListView"/>.</param>
        /// <param name="collection">The collection<see cref="IList"/>.</param>
        public SelectedItemsBinder(ListView listView, IList collection)
        {
            _listView = listView;
            _collection = collection;

            _listView.SelectedItems.Clear();

            foreach (var item in _collection)
            {
                _listView.SelectedItems.Add(item);
            }
        }

        /// <summary>
        /// The Bind.
        /// </summary>
        public void Bind()
        {
            _listView.SelectionChanged += ListView_SelectionChanged;

            if (_collection is INotifyCollectionChanged)
            {
                var observable = (INotifyCollectionChanged)_collection;
                observable.CollectionChanged += Collection_CollectionChanged;
            }
        }

        /// <summary>
        /// The UnBind.
        /// </summary>
        public void UnBind()
        {
            if (_listView != null)
                _listView.SelectionChanged -= ListView_SelectionChanged;

            if (_collection != null && _collection is INotifyCollectionChanged)
            {
                var observable = (INotifyCollectionChanged)_collection;
                observable.CollectionChanged -= Collection_CollectionChanged;
            }
        }

        /// <summary>
        /// The Collection_CollectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="NotifyCollectionChangedEventArgs"/>.</param>
        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems ?? new object[0])
            {
                if (!_listView.SelectedItems.Contains(item))
                    _listView.SelectedItems.Add(item);
            }
            foreach (var item in e.OldItems ?? new object[0])
            {
                _listView.SelectedItems.Remove(item);
            }
        }

        /// <summary>
        /// The ListView_SelectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="SelectionChangedEventArgs"/>.</param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems ?? new object[0])
            {
                if (!_collection.Contains(item))
                    _collection.Add(item);
            }

            foreach (var item in e.RemovedItems ?? new object[0])
            {
                _collection.Remove(item);
            }
        }
    }
}
