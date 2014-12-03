using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ProjectPediaWebAPI.PortfolioCore
{
    class SqlTextLoader 
    {
        public SqlTextLoader(string path, string ext)
        {
            LoadDirectory(path, ext);
        }

        private string[] ItemList;
        private int CurrentIndex;
        private int ListLength;

        public bool HasMoreData()
        {
            return (ListLength>CurrentIndex) ? true : false;
        }

        public string GetCurrentFileName()
        {
            return ItemList[CurrentIndex];
        }

        public string[] GetNextSetOfQueries()
        {
            string[] CommandsInFile = new string[0];

            string FilePath = ItemList[CurrentIndex];

            try
            {
                using (StreamReader sr = new StreamReader(FilePath))
                {
                    String contents = sr.ReadToEnd();
                    CommandsInFile = Regex.Split(contents, ";\r\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Parsing data file:");
                Console.WriteLine(e.Message);
            }

            CurrentIndex++;

            return CommandsInFile;
        }

        private string GetIDFromFileName(string FilePath)
        {
            string[] pieces = FilePath.Split('\\');

            string fileName = pieces[pieces.Length-1];

            string[] filePlusExt = fileName.Split('.');

            return filePlusExt[0];
        }

        private void LoadDirectory(string path, string ext)
        {
            CurrentIndex = 0;
            try
            {
                System.Console.WriteLine("Trying to load directory " + path + ext);
                ItemList = Directory.GetFiles(path, ext);
                ListLength = ItemList.Length;
            }
            catch (Exception exc)
            {
                System.Console.WriteLine("Could not Load Directory " + path + ": \n  --> " + exc.Message);
                ListLength = 0;
            }
        }

        

    }

    

}