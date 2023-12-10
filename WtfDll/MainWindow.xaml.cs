using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WtfDll.Utils;

namespace WtfDll
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Gets the ViewModel.
        /// </summary>
        private MainViewModel ViewModel
        {
            get { return (MainViewModel)this.DataContext; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();

            lstFileView.Drop += ListView_Drop;
            btnRemoveSelected.Click += BtnRemoveSelected_Click;
            btnClearSelection.Click += BtnClearSelection_Click;
            btnImiyaNieOpredeleno.Click += BtnImiyaNieOpredeleno_Click;
        }

        /// <summary>
        /// The BtnImiyaNieOpredeleno_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private async void BtnImiyaNieOpredeleno_Click(object sender, RoutedEventArgs e)
        {
            foreach (var fileItem in this.ViewModel.SelectedFiles)
            {
                this.statusBar.Content = $"Processing file {fileItem.File}...";
                await ProcessFile();
                fileItem.Status = FileStatus.Processed;
            }
            this.statusBar.Content = $"{lstFileView.SelectedItems.Count} files processed";
            ClearSelection();
        }

        /// <summary>
        /// The ProcessFile.
        /// </summary>
        /// <param name="fileName">The fileName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private static async Task ProcessFile(string fileName = "undefined")
        {
            await Task.Delay(2 * 1000); // non-ui blocking
        }

        /// <summary>
        /// The BtnClearSelection_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void BtnClearSelection_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            this.statusBar.Content = "Selection has been cleared";
        }

        /// <summary>
        /// The ClearSelection.
        /// </summary>
        private void ClearSelection()
        {
            this.lstFileView.SelectedItems.Clear();
        }

        /// <summary>
        /// The BtnRemoveSelected_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void BtnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            // get files from selection and remove them from the model collection allFiles
            var selectedFilesCopy = this.ViewModel.SelectedFiles.ToArray();
            this.ViewModel.AllFiles.RemoveMany(selectedFilesCopy);
        }

        /// <summary>
        /// The ListView_Drop.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="DragEventArgs"/>.</param>
        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop, true);

                foreach (var file in droppedFiles)
                {
                    var fileModel = new FileModel { File = file, Status = FileStatus.Unknown };
                    this.ViewModel.AllFiles.Add(fileModel);
                }
            }
        }
    }
}
