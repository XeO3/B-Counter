using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace B_Counter.Model
{
    public class FileDetail : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string arg)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(arg));
            }
        }


        /// <summary>파일 이름(확장자 포함)</summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value; OnPropertyChanged("FileName");
            }
        }

        /// <summary>파일 용량</summary>
        public long FileSize
        {
            get
            {
                return _fileSize;
            }

            set
            {
                _fileSize = value;
            }
        }

        /// <summary>파일 확장자</summary>
        public string Extention
        {
            private get
            {
                return _Extention;
            }

            set
            {
                _Extention = value;
            }
        }

        /// <summary>파일 위치</summary>
        public string FileDirectory
        {
            get
            {
                return _filePath;
            }

            set
            {
                _filePath = value;
            }
        }

        /// <summary>내부 텍스트</summary>
        public TextInfo Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
            }
        }

        /// <summary>選択フラー具</summary>
        public bool IsSelcted
        {
            get
            {
                return _IsSelcted;
            }

            set
            {
                _IsSelcted = value;
            }
        }

        private string _fileName;
        private long _fileSize;
        private string _Extention;
        private string _filePath;
        private TextInfo _text;
        private bool _IsSelcted;


        public FileDetail()
        {

        }
        public FileDetail(string pFilePath)
        {
            openFile(pFilePath);
        }



        private void openFile(string pFilePath)
        {
            FileInfo fileinfo = new FileInfo(pFilePath);
            this.FileSize = fileinfo.Length;
            this.FileDirectory = fileinfo.DirectoryName;
            this.FileName = fileinfo.Name;
            this.Extention = fileinfo.Extension;

            this.Text = new TextInfo(pFilePath);
            
                

            try
            {

            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }

       
    }
}
