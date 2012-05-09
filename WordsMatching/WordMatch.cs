using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;


namespace WordsMatching
{
    class WordMatch
    {
        private Dictionary<int, List<string>> _lengthDict;
        private Dictionary<char, List<string>> _headerDict;
        
        public Dictionary<int, List<string>> LengthDict
        {
            get{
                if (_lengthDict == null)
                {
                    _lengthDict = new Dictionary<int, List<string>>();
                }
                return _lengthDict;
            }
            set 
            {
                _lengthDict = value;
            }
        }

        public Dictionary<char, List<string>> HeaderDict
        {
            get
            {
                if (_headerDict == null)
                {
                    _headerDict = new Dictionary<char, List<string>>();
                }
                return _headerDict;
            }
            set 
            {
                _headerDict = value;
            }
        }

        public int TotalCount
        {
            get;
            set;
        }

        public void Init()
        {
            LengthDict.Clear();
            HeaderDict.Clear();
            
            ///
            /// version 1.0, read all .txt files from dict folder.
            ///

            //string path = System.Environment.CurrentDirectory+@"\dict\";
            ////MessageBox.Show(path);

            //var files = Directory.GetFiles(path, "*.txt");

            //foreach (var file in files)
            //{
            //    StreamReader reader = new StreamReader(file);           
            //    string word;
            //    while ((word = reader.ReadLine()) != null)
            //    {
            //        word = word.Trim();
            //        if(word.Length <=0)
            //            continue;
                   
            //        if (!LengthDict.ContainsKey(word.Length))
            //        {
            //            LengthDict[word.Length] = new List<string>();
            //        }
            //        LengthDict[word.Length].Add(word);

            //        if (!HeaderDict.ContainsKey(char.ToUpper(word[0])))
            //        {
            //            HeaderDict[char.ToUpper(word[0])] = new List<string>();
            //        }
            //        HeaderDict[char.ToUpper(word[0])].Add(word);
            //    }
            //}
            
            ///
            /// version 1.1 add dict file as resource.
            ///

            
            string[] words = Properties.Resources.US.Split(new string[] { Environment.NewLine,"\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string w in words)
            {
                string word = w.Trim();
                if (word.Length <= 0)
                    continue;

                if (!LengthDict.ContainsKey(word.Length))
                {
                    LengthDict[word.Length] = new List<string>();
                }
                LengthDict[word.Length].Add(word);

                if (!HeaderDict.ContainsKey(char.ToUpper(word[0])))
                {
                    HeaderDict[char.ToUpper(word[0])] = new List<string>();
                }
                HeaderDict[char.ToUpper(word[0])].Add(word);

                TotalCount++;
            }

        }

        /// <summary>
        /// out put a list from dictionary which match the patten
        /// i need think about how stuff work for this search condition.
        /// </summary>
        /// <param name="patten"></param>
        /// <param name="length"></param>
        /// <param name="stuff"></param>
        /// <returns></returns>
        public List<string> Match(string patten, int length/*, string stuff*/)
        {

            string regStr =ParsePatten(patten);

            Regex regex = new Regex(regStr);
            List<string> mList = new List<string>();

            // if no '*' in patten, use fixed length.
            if (!patten.Contains('*') && !patten.Contains('[') && length <=0 )
            {
                length = patten.Length;
            }

            if (length >0) // go with length dictionary
            {
                List<string> llist = LengthDict[length];
                foreach (string word in llist)
                {
                    if (regex.IsMatch(word))
                        mList.Add(word);
                }
            }
            else if (patten.Length > 0 && char.IsLetter(patten[0])) // go with header dictionary
            {
                List<string> hlist = HeaderDict[char.ToUpper(patten[0])];
                foreach (string word in hlist)
                {
                    if (regex.IsMatch(word))
                        mList.Add(word);
                }
            }
            else // search all dictionary
            {
                foreach (List<string> nlist in HeaderDict.Values)
                {
                    foreach (string word in nlist)
                    {
                        if (regex.IsMatch(word))
                            mList.Add(word);
                    }
                }
            }

            return mList;
        }

        // convert '?' and '*' to regular expression.
        private string ParsePatten(string patten)
        {
            string regularExString;
            if (patten != null && patten.Length > 0 )//&& char.IsLetter(patten[0]))
                patten= "^"+patten;
            if (patten != null && patten.Length > 0)//&& char.IsLetter(patten[patten.Length-1])
                patten += "$";

            patten = patten.Replace("?", "[a-zA-Z]");
            regularExString = patten.Replace("*", "[a-zA-Z]*");
            
            return regularExString;
        }

        private bool MatchStuff(string word, string stuff)
        {
            
            return true;
        }
    }

    //public static class StuffCompare{
    //    public static bool compareStuff(this string s, string stuff)
    //    {
    //        char[] arr = s.ToCharArray();
    //        char[] arr_stuff = stuff.ToCharArray();

    //        Array.Sort(arr);
            
    //        return true;
    //    }
    //}
}
