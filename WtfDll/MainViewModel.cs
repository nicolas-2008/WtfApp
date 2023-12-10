using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WtfDll
{
    /// <summary>
    /// Defines the <see cref="MainViewModel" />.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the hasSelection.
        /// </summary>
        private bool hasSelection;

        /// <summary>
        /// Gets the AllFiles.
        /// </summary>
        public ObservableCollection<FileModel> AllFiles { get; } = new ObservableCollection<FileModel>();

        /// <summary>
        /// Gets the SelectedFiles.
        /// </summary>
        public ObservableCollection<FileModel> SelectedFiles { get; } = new ObservableCollection<FileModel>();

        /// <summary>
        /// Gets or sets a value indicating whether HasSelection.
        /// </summary>
        public bool HasSelection
        {
            get
            {
                return hasSelection;
            }

            set
            {
                hasSelection = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Defines the PropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The RaisePropertyChanged.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        private void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            SelectedFiles.CollectionChanged += SelectedFiles_CollectionChanged;
        }

        /// <summary>
        /// The SelectedFiles_CollectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/>.</param>
        private void SelectedFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HasSelection = SelectedFiles.Count != 0;
        }
    }
}
