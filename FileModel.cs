using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WtfApp
{
    public class FileModel : INotifyPropertyChanged
    {
        private FileStatus status;
        public event PropertyChangedEventHandler PropertyChanged;

        public string File { get; set; }
        public FileStatus Status
        {
            get
            { 
                return status;
            
            }
            set
            {
                status = value;
                RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
