using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WtfDll
{
    /// <summary>
    /// Defines the <see cref="FileModel" />.
    /// </summary>
    public class FileModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the status.
        /// </summary>
        private FileStatus status;

        /// <summary>
        /// Defines the PropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the File.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
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

        /// <summary>
        /// The RaisePropertyChanged.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        private void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
