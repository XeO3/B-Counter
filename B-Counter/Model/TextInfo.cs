using B_Counter.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace B_Counter.Model
{
    public class TextInfo
    {
        /// <summary>텍스트
        /// </summary>
        public string InnerText
        {
            get
            {
                return _innerText;
            }

            set
            {
                _innerText = value;
                if (null == value)
                {
                    //カウント不可
                    this._length = this._words = this._characters = this._fullBytes =  - 1;
                }
                else
                {
                    this._length = value.Length;
                    this._words = CountWords(value);
                    this._characters = CountCharactors(value);
                    this._fullBytes = CountZenkaku(value);
                }
            }
        }

        /// <summary>문자길이(공백포함)
        /// </summary>
        public int Length
        {
            get
            {
                return _length;
            }
        }

        /// <summary>워드수
        /// </summary>
        public int Words
        {
            get
            {
                return _words;
            }
        }

        /// <summary>문자수(공백 미포함)
        /// </summary>
        public int Characters
        {
            get
            {
                return _characters;
            }
        }

        /// <summary>全角文字+半角カタカナ
        /// </summary>
        public int FullBytes
        {
            get
            {
                return _fullBytes;
            }

            set
            {
                _fullBytes = value;
            }
        }



        #region private
        private string _innerText;
        private int _length;
        private int _words;
        private int _characters;
        private int _fullBytes;
        #endregion

        public TextInfo()
        {

        }

        public TextInfo(string pFilePath)
        {
            newFileOpen(pFilePath);
        }

        #region funtion
        public void newFileOpen(string pFilePath)
        {
            this.InnerText = DocReader.GetDocumentPlainText(pFilePath);
        }

        public int CountWords(string pText)
        {
            String text = pText.Trim();
            int wordCount = 0, index = 0;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == false)
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == true)
                    index++;
            }
            return wordCount;
        }

        public int CountCharactors(string pText)
        {
            string text = pText.Trim();
            int charCount = 0, index = 0;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == false)
                {
                    index++;
                    charCount++;
                }

                // skip whitespace until next word
                while (index < text.Length && Char.IsWhiteSpace(text[index]) == true)
                    index++;
            }


            return charCount;
        }

        public int CountZenkaku(string pText)
        {
            pText = Regex.Replace(pText, @"[^\x01-\x7E]", "");
            return CountCharactors(pText);
        }
        #endregion


        #region override
        public override string ToString()
        {
            return InnerText;
        }
        #endregion

    }
}
