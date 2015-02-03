using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JvlSCard;

namespace JvlMifareTest
{
    class ReaderDetector
    {
        private bool _IsReaderConnected = false;
        private string _ReaderName = "OMNIKEY CARDMAN 5x21 CL 0";

        private SCardRoutines scardr;

        private ArrayList _ReaderList;

        public ReaderDetector()
        {
            scardr = new SCardRoutines();
        }


        public void ListReaders()
        {


            _ReaderList = scardr.ListReaders();
            if (_ReaderList.Count > 0)
            {
                _IsReaderConnected = true;
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < _ReaderList.Count; i++)
                    Console.WriteLine(" ({0}) {1} ", i, _ReaderList[i]);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                _IsReaderConnected = false;
            }


        }

        public void SelectReader(int number)
        {
            try
            {
                _ReaderName = _ReaderList[number].ToString();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Selected Reader: {0}", _ReaderName);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Reader Number!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public bool IsReaderConnected
        {
            get { return _IsReaderConnected; }
        }

        public string ReaderName
        {
            get { return _ReaderName; }
        }
    }
}
