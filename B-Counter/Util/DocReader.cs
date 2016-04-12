using System.Linq;
using System.Text;
using Spire.Xls;
using Spire.Doc;
//using Spire.Pdf;
using System.IO;
using System;
using System.Windows;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

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
                }
                catch(Exception err)
                {
                    MessageBox.Show(err.ToString());
                    return string.Empty;
                }
                plainText.Append(document.GetText());
                document.Close();
            }
            else if (ext.Equals(".XLSX") || ext.Equals(".XLS"))
            {
                //Create Excel workbook
                Workbook workbook = new Workbook();

                //load a workbook
                try
                {
                    workbook.LoadFromFile(pFilePath);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                    return string.Empty;
                }

                for (int i = 0; i < workbook.Worksheets.Count; i++)
                {
                    string tmpfilename = "tempSheet" + i.ToString() + ".txt";
                    Worksheet sheet = workbook.Worksheets[i];

                    if (!sheet.IsEmpty)
                    {
                        plainText.Append("--[" + sheet.Name + "]--\r\n");
                        StringBuilder sb = new StringBuilder();
                        //全セルのテキストを記録
                        for (int row = 0; row < sheet.Rows.Count(); row++)
                        {
                            for (int col = 0; col < sheet.Columns.Count(); col++)
                            {
                                plainText.Append(sheet.GetText(row + 1, col + 1) + ", ");
                            }
                            plainText.AppendLine();
                        }
                        plainText.AppendLine();
                    }
                }
                workbook.Dispose();
            }
            else if (ext.Equals(".PDF"))
            {
                ////Create word document
                //PdfDocument doc = new PdfDocument();

                ////load a document
                //try
                //{
                //    doc.LoadFromFile(pFilePath);
                //}
                //catch (Exception err)
                //{
                //    MessageBox.Show(err.ToString());
                //    return string.Empty;
                //}

                //foreach (PdfPageBase page in doc.Pages)
                //{
                //    plainText.Append(page.ExtractText());
                //}
                //doc.Close();


                try
                {
                    var reader = new PdfReader(pFilePath);
                    var numberOfPages = reader.NumberOfPages;

                    for (var currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {
                        plainText.Append(PdfTextExtractor.GetTextFromPage(reader, currentPageIndex));
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            else if (ext.Equals(".TXT") || ext.Equals(".CSV"))
            {
                plainText.Append(System.IO.File.ReadAllText(pFilePath));
            }
            else
            {
                //分類不可
                return null;
            }

            return plainText.ToString();
        }
    }
}
