using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using WtfApp.Utils;

namespace WtfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel { get { return (MainViewModel)this.DataContext; } }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();

            lstFileView.Drop += ListView_Drop;
            btnRemoveSelected.Click += BtnRemoveSelected_Click;
            btnClearSelection.Click += BtnClearSelection_Click;
            btnImiyaNieOpredeleno.Click += BtnImiyaNieOpredeleno_Click;
        }

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

        private static async Task ProcessFile(string fileName = "undefined")
        {
            await Task.Delay(3 * 1000); // non-ui blocking
        }

        private void BtnClearSelection_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
            this.statusBar.Content = "Selection has been cleared";
        }

        private void ClearSelection()
        {
            this.lstFileView.SelectedItems.Clear();
        }

        private void BtnRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            // get files from selection and remove them from the model collection allFiles
            var selectedFilesCopy = this.ViewModel.SelectedFiles.ToArray();
            this.ViewModel.AllFiles.RemoveMany(selectedFilesCopy);
        }

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
