using System.Linq;
using System.Text;
using Spire.Doc;
//using Spire.Pdf;
using System;
using System.Windows;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Spire.Xls;
using System.IO;

namespace B_Counter.Util
{
    public class DocReader
    {
        /// <summary>.docx, .doc, .xlsx, .xls ⇒ PlainText</summary>
        public static string GetDocumentPlainText(string pFilePath)
        {
            StringBuilder plainText = new StringBuilder();
            string ext = System.IO.Path.GetExtension(pFilePath).ToUpper();

            if (ext.Equals(".DOCX") || ext.Equals(".DOC"))
            {
                //Create word document
                Document document = new Document();

                //load a document
                try
                {
                    document.LoadFromFile(pFilePath);
                    plainText.Append(document.GetText());
                    document.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(string.Format("{0}を開く際にエラーが発生しました。", pFilePath));
#if DEBUG
                    MessageBox.Show(err.ToString());
#endif
                    document.Close();
                    return null;
                }
            }

            else if (ext.Equals(".XLSX") || ext.Equals(".XLS"))
            {
                //Create Excel workbook

                using (Workbook workbook = new Workbook())
                {
                    //load a workbook
                    try
                    {
                        workbook.LoadFromFile(pFilePath);

                        foreach (Worksheet sheet in workbook.Worksheets)
                        {
                            sheet.SaveToFile(sheet.Name + ".txt", "\t", Encoding.UTF8);
                            plainText.AppendLine(GetDocumentPlainText(sheet.Name + ".txt"));
                        }

                        workbook.Dispose();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(string.Format("{0}を開く際にエラーが発生しました。", pFilePath));
#if DEBUG
                        MessageBox.Show(err.ToString());
#endif
                        workbook.Dispose();
                        return null;
                    }

                }
            }
            else if (ext.Equals(".PDF"))
            {
                try
                {
                    var reader = new PdfReader(pFilePath);
                    var numberOfPages = reader.NumberOfPages;

                    for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {
                        plainText.Append(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex));
                    }
                }
                catch (Exception err)
                {

                    MessageBox.Show(string.Format("{0}を開く際にエラーが発生しました。", pFilePath));
#if DEBUG
                    MessageBox.Show(err.ToString());
#endif
                    return null;
                }
            }
            else if (ext.Equals(".TXT") || ext.Equals(".CSV"))
            {
                try
                {
                    plainText.Append(System.IO.File.ReadAllText(pFilePath));
                }
                catch (Exception err)
                {

                    MessageBox.Show(string.Format("{0}を開く際にエラーが発生しました。", pFilePath));
#if DEBUG
                    MessageBox.Show(err.ToString());
#endif
                    return null;
                }
            }
            else
            {
                //分類不可
                return null;
            }

            return plainText.ToString().Trim();
        }
    }
}
