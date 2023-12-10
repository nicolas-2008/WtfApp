using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WtfDll;

namespace WtfApp
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            var window = new MainWindow();
            window.ShowDialog();
        }
    }
}
