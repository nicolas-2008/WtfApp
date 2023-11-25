using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WtfApp.Utils
{
    public class ListViewExtensions
    {
        private static SelectedItemsBinder GetSelectedItemsBinder(DependencyObject obj)
        {
            return (SelectedItemsBinder)obj.GetValue(SelectedItemsBinderProperty);
        }

        private static void SetSelectedItemsBinder(DependencyObject obj, SelectedItemsBinder items)
        {
            obj.SetValue(SelectedItemsBinderProperty, items);
        }

        private static readonly DependencyProperty SelectedItemsBinderProperty = DependencyProperty.RegisterAttached("SelectedItemsBinder", typeof(SelectedItemsBinder), typeof(ListViewExtensions));


        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(ListViewExtensions),
            new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));


        private static void OnSelectedItemsChanged(DependencyObject o, DependencyPropertyChangedEventArgs value)
        {
            var oldBinder = GetSelectedItemsBinder(o);
            if (oldBinder != null)
                oldBinder.UnBind();

            SetSelectedItemsBinder(o, new SelectedItemsBinder((ListView)o, (IList)value.NewValue));
            GetSelectedItemsBinder(o).Bind();
        }

        public static void SetSelectedItems(Selector elementName, IEnumerable value)
        {
            elementName.SetValue(SelectedItemsProperty, value);
        }

        public static IEnumerable GetSelectedItems(Selector elementName)
        {
            return (IEnumerable)elementName.GetValue(SelectedItemsProperty);
        }
    }

    public class SelectedItemsBinder
    {
        private ListView _listView;
        private IList _collection;


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

        public void Bind()
        {
            _listView.SelectionChanged += ListView_SelectionChanged;

            if (_collection is INotifyCollectionChanged)
            {
                var observable = (INotifyCollectionChanged)_collection;
                observable.CollectionChanged += Collection_CollectionChanged;
            }
        }

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
