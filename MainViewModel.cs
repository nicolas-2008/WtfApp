using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WtfApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool hasSelection;

        public ObservableCollection<FileModel> AllFiles { get; } = new ObservableCollection<FileModel>();
        public ObservableCollection<FileModel> SelectedFiles { get; } = new ObservableCollection<FileModel>();
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainViewModel()
        {
            SelectedFiles.CollectionChanged += SelectedFiles_CollectionChanged;
        }

        private void SelectedFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HasSelection = SelectedFiles.Count != 0;
        }
    }
}
