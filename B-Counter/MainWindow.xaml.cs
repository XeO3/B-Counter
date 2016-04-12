using B_Counter.ViewModel;
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

namespace B_Counter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VM_main main;

        public MainWindow()
        {
            InitializeComponent();
            main = new VM_main();
            this.DataContext = main;
        }

        private void button_import_Click(object sender, RoutedEventArgs e)
        {
            if (1 == 1)
            {

            }
        }



        private void Window_Drop(object sender, DragEventArgs e)
        {
            this.Opacity = 1;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                main.AddFileInfo(files);

            }

        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            this.Opacity = 0.5;
        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            this.Opacity = 1;
        }

        private void dataGrid_result_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
