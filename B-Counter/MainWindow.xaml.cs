using B_Counter.Model;
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
            e.Row.Header = (e.Row.GetIndex() + 1).ToString() + " ";
        }

        private void dataGrid_result_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataGridCellInfo cell in e.AddedCells)
            {
                if (cell.Item.GetType().Equals(typeof(FileDetail)))
                {
                    sb.AppendLine(((FileDetail)cell.Item).Text.InnerText);
                }
                break;
            }

            TextBox_Text.Text = sb.ToString();
            if (string.IsNullOrWhiteSpace(TextBox_Text.Text))
            {
                GroupBox_Selected.Visibility = Visibility.Collapsed;
            }
            else
            {
                GroupBox_Selected.Visibility = Visibility.Visible;
            }

        }

        private void button_CopyToClipBoard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TextBox_Text.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConvertFlowDocument(ref rich_TotalCount, main.TotalCount.ToString("#,0"));
            ConvertFlowDocument(ref rich_TotalSize, main.TotalSize.ToString("#,0"));
            ConvertFlowDocument(ref rich_TotalLengh, main.TotalLengh.ToString("#,0"));
            ConvertFlowDocument(ref rich_TotalCharacter, main.TotalCharacter.ToString("#,0"));
            ConvertFlowDocument(ref rich_TotalWords, main.TotalWords.ToString("#,0"));
            ConvertFlowDocument(ref rich_TotalFullBytes, main.TotalFullBytes.ToString("#,0"));
        }

        private void ConvertFlowDocument(ref RichTextBox rtb, string text)
        {
            FlowDocument myFlowDoc = new FlowDocument();
            Paragraph pa = new Paragraph();
            pa.FontSize = 15;
            pa.TextAlignment = TextAlignment.Right;

            foreach (char ch in text)
            {

                Run run = new Run(ch.ToString());

                if (ch.Equals('9'))
                {
                    run.Foreground = Brushes.Red;
                    run.FontWeight = FontWeights.Bold;
                    run.FontSize = 16;
                }
                pa.Inlines.Add(run);
            }

            myFlowDoc.Blocks.Add(pa);
            rtb.Document = myFlowDoc;
        }


        /// <summary> ITEM削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_result_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete))
            {

                //main.DeleteFileInfo(dataGrid_result.CurrentCell.Item);
                foreach (DataGridCellInfo cell in dataGrid_result.SelectedCells)
                {
                 //   var currentRowIndex = dataGrid_result.Items.IndexOf(cell.Item);



                    main.DeleteFileInfo(cell.Item);
                }
            }
        }


        /// <summary>datagrid의 체크박스 선택시 선택한 리스트 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChecked(object sender, RoutedEventArgs e)
        {

         
            var selectedlist = from list in main.FileInfoList
                               where list.IsSelcted
                               select new
                               {
                                   list.FileSize,
                                   list.Text.Characters,
                                   list.Text.FullBytes,
                                   list.Text.Length,
                                   list.Text.Words,
                               };

            int total = (int)selectedlist.Select(u => (u.FileSize < 0) ? 0 : u.FileSize).Sum();

            ConvertFlowDocument(ref rich_SelectedCount, selectedlist.Count().ToString("#,0"));
            ConvertFlowDocument(ref rich_SelectedSize, selectedlist.Select(u => (u.FileSize < 0) ? 0 : u.FileSize).Sum().ToString("#,0"));
            ConvertFlowDocument(ref rich_SelectedLengh, selectedlist.Select(u => (u.Length < 0) ? 0 : u.Length).Sum().ToString("#,0"));
            ConvertFlowDocument(ref rich_SelectedCharacter, selectedlist.Select(u => (u.Characters < 0) ? 0 : u.Characters).Sum().ToString("#,0"));
            ConvertFlowDocument(ref rich_SelectedWords, selectedlist.Select(u => (u.Words < 0) ? 0 : u.Words).Sum().ToString("#,0"));
            ConvertFlowDocument(ref rich_SelectedFullBytes, selectedlist.Select(u => (u.FullBytes < 0) ? 0 : u.FullBytes).Sum().ToString("#,0"));

        }
    }
}
