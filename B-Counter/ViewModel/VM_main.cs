using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_Counter.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace B_Counter.ViewModel
{
    class VM_main : INotifyPropertyChanged
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

        public ObservableCollection<FileDetail> FileInfoList
        {
            get
            {
                return _fileInfoList ?? (_fileInfoList = new ObservableCollection<FileDetail>());
            }

            set
            {
                _fileInfoList = value;
            }
        }
        public int TotalCount
        {
            get
            {
                return FileInfoList.Count;
            }
        }

        public int TotalSize
        {
            get
            {
                return (int)FileInfoList.Select(u => (u.FileSize < 0) ? 0 : u.FileSize).Sum();
            }
        }

        public int TotalLengh
        {
            get
            {
                return (int)FileInfoList.Select(u => (u.Text.Length < 0) ? 0 : u.Text.Length).Sum();
            }
        }

        public int TotalWords
        {
            get
            {
                return FileInfoList.Sum(u => u.Text.Words);
            }
        }

        public int TotalCharacter
        {
            get
            {
                return FileInfoList.Sum(u => u.Text.Characters);
            }
        }

        public int TotalFullBytes
        {
            get
            {
                return FileInfoList.Sum(u => u.Text.FullBytes);
            }
        }

        public void AddFileInfo(string[] pPathList)
        {

            foreach (string file in pPathList)
            {
                if (!File.Exists(file))
                {
                    if (Directory.Exists(file))
                    {
                        DirectoryInfo dir = new DirectoryInfo(file);
                        string[] names = dir.GetFiles().Select(u => u.FullName).ToArray();
                        string[] dirs = dir.GetDirectories().Select(u => u.FullName).ToArray();

                        AddFileInfo(names);
                        AddFileInfo(dirs);
                    }

                    continue;

                }



                //중복 제거
                foreach (FileDetail item in FileInfoList)
                {
                    if ((item.FileDirectory + @"\" + item.FileName).ToUpper().Equals(file.ToUpper()))
                    {
                        FileInfoList.Remove(item);
                        break;
                    }
                }


                //파일 추가
                FileDetail newFile = new FileDetail(file);
                if (newFile.Text.Length < 0)
                {
                    continue;
                }

                this.FileInfoList.Add(new FileDetail(file));

                OnPropertyChanged("TotalCount");
                OnPropertyChanged("TotalSize");
                OnPropertyChanged("TotalLengh");
                OnPropertyChanged("TotalWords");
                OnPropertyChanged("TotalCharacter");
                OnPropertyChanged("TotalFullBytes");

            }

        }

        private ObservableCollection<FileDetail> _fileInfoList;

    }
}
