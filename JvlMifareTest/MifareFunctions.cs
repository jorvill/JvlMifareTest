/**
 * 
 * Author: Jorge Villicana
 * 
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JvlMifare;

namespace JvlMifareTest
{
    class MifareFunctions
    {

        private const int KEYLOCNUM = 0x00;
        private const int KEY_LENGTH = 6;

        private ReaderDetector _readerDetector;
        private MifareCommands mifarecom;



        public MifareFunctions(ReaderDetector readerDetector)
        {
            _readerDetector = readerDetector;
            mifarecom = new MifareCommands();
        }


        #region SelectReader
        public void SelectReader()
        {
            try
            {
                _readerDetector.ListReaders();
                Console.Write("Select reader: ");
                string r = Console.ReadLine();
                int num = Convert.ToInt32(r);
                _readerDetector.SelectReader(num);
                mifarecom.SetReader(_readerDetector.ReaderName);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Reader Number!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        #endregion

        #region ConnectCard
        public void ConnectCard()
        {
            int status = mifarecom.ConnectCard();
            Console.WriteLine("Connect Card ...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));
            if (status == MifareStatusCodes.SUCCESS)
                GetCardType();
 
        }
        #endregion

        #region DisconnectCard
        public void DisconnectCard()
        {
            int status = mifarecom.DisconnectCard();
            Console.WriteLine("Disconnect Card ...{0}", status.ToString("X4"));
        }
        #endregion

        #region CheckIfCardIsPresent
       public  void CheckIfCardIsPresent()
        {
            bool b = mifarecom.CheckIfCardIsPresent();
            Console.WriteLine("CheckIfCardIsPresent...{0}", b.ToString());
        }
        #endregion

        #region GetCSN
        public  void GetCSN()
        {
            byte[] csn = new byte[16];
            int len = 0;
            int status = mifarecom.GetCSN(ref csn, ref len);
            Console.WriteLine("GetCSN...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));
            if (status == MifareStatusCodes.SUCCESS)
            {
                Console.WriteLine("CSN = {0}", ConvertArrays.ConvertByteArrayToStringHex(csn, 8));

            }
        }
        #endregion

        #region WriteBlock
        public void WriteBlock()
        {
            int status = 0;

            byte[] data = new byte[32];
            try
            {
                Console.Write("Sector = ");
                int sector = Convert.ToInt16(Console.ReadLine());
                Console.Write("Bloque  = ");
                int bloque = Convert.ToInt16(Console.ReadLine());
                int blocknumber = 4 * sector + bloque;
                Console.Write("Data (hex) [16 byte max]= ");
                string strData = Console.ReadLine();

                int num = ConvertArrays.ConvertStringHexToByteArray(strData, ref data, 16);
                status = mifarecom.WriteData(GetBlockNumber(sector, bloque), data);

                Console.WriteLine("WriteData...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));
            }
            catch (Exception ex)
            {

                Console.WriteLine("Parametro Invalido");

            }

        }
        #endregion

        #region ReadBlock
        public  void ReadBlock()
        {
            int status = 0;

            byte[] data = new byte[16];
            try
            {
                Console.Write("Sector = ");
                int sector = Convert.ToInt16(Console.ReadLine());
                Console.Write("Bloque  = ");
                int bloque = Convert.ToInt16(Console.ReadLine());
                int blocknumber = 4 * sector + bloque;

                status = mifarecom.ReadData(GetBlockNumber(sector, bloque), ref data);

                
                Console.WriteLine("ReadData...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));
                

                if (status == MifareStatusCodes.SUCCESS)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(ConvertArrays.ConvertByteArrayToStringHex(data, 16));
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Invalid Parameter");

            }

        }
        #endregion

        #region GetCardType
        public void GetCardType()
        {
            JvlMifareTools miftools = new JvlMifareTools();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Card Type: {0}", miftools.GetCardTypeInfo(mifarecom.GetCardType()));
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        #endregion

        #region GetAccesBitsInfo
        public void GetAccessBitsInfo()
        {
            int status = 0;

            byte[] data = new byte[16];
            try
            {
                Console.Write("Sector [0 al 15]= ");
                int sector = Convert.ToInt16(Console.ReadLine());

                int bloque = 3;
                int blocknumber = 4 * sector + bloque;

                status = mifarecom.ReadData(blocknumber, ref data);
                Console.WriteLine("ReadData...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));

                if (status == MifareStatusCodes.SUCCESS)
                {
                    Console.WriteLine(ConvertArrays.ConvertByteArrayToStringHex(data, 16));
                    Console.WriteLine();
                    JvlMifareTools miftool = new JvlMifareTools();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(miftool.GetAccessBitsInfo(data));
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("Parametro Invalido");

            }


        }
        #endregion

        #region Authenticate

        public  void Authenticate()
        {
            int status = 0;

            byte[] data = new byte[16];
            try
            {
                Console.Write("Sector = ");
                int sector = Convert.ToInt16(Console.ReadLine());
                Console.Write("Tipo de Llave [A=0 , B=1]= ");
                short kt = Convert.ToInt16(Console.ReadLine());
                Console.Write("Keyloc [00 a 1F] (hex) = ");
                int keylocnum = Convert.ToInt16(Console.ReadLine(), 16);

                int keytype = 0;

                if (kt == 1)
                {
                    keytype = MifareCommands.KEYB;
                }
                else
                {
                    keytype = MifareCommands.KEYA;
                }

                status = mifarecom.Authenticate(keytype, keylocnum, GetBlockNumber(sector, 0));
                Console.WriteLine("Authenticate...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));



            }
            catch (Exception ex)
            {

                Console.WriteLine("Parametro Invalido");

            }
        }

        #endregion

        #region Increment

        public  void Increment()
        {
            int status = 0;

            byte[] data = new byte[16];
            try
            {
                Console.Write("Sector [0 al 15]= ");
                int sector = Convert.ToInt16(Console.ReadLine());
                Console.Write("Bloque [0 al 3] = ");
                int bloque = Convert.ToInt16(Console.ReadLine());
                int blocknumber = 4 * sector + bloque;
                Console.Write("Cantidad  = ");
                uint cantidad = Convert.ToUInt16(Console.ReadLine());

                status = mifarecom.Increment(GetBlockNumber(sector, bloque), cantidad);
                Console.WriteLine("Increment...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Parametro Invalido");
            }
        }

        #endregion

        #region Decrement
        public void Decrement()
        {
            int status = 0;

            byte[] data = new byte[16];
            try
            {
                Console.Write("Sector [0 al 15]= ");
                int sector = Convert.ToInt16(Console.ReadLine());
                Console.Write("Bloque [0 al 3] = ");
                int bloque = Convert.ToInt16(Console.ReadLine());
                int blocknumber = 4 * sector + bloque;
                Console.Write("Cantidad  = ");
                int cantidad = Convert.ToUInt16(Console.ReadLine());

                status = mifarecom.Decrement(GetBlockNumber(sector, bloque), cantidad);
                Console.WriteLine("Decrement...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Parametro Invalido");
            }
        }

        #endregion

        #region LoadKey

        public  void LoadKey()
        {
            byte[] key = new byte[6];
            try
            {
                Console.Write("Key [6 bytes] (hex) = ");
                int status = ConvertArrays.ConvertStringHexToByteArray(Console.ReadLine(), ref key, 6);
                Console.Write("Keyloc [00 a 1F] (hex) = ");
                int keylocnum = Convert.ToInt16(Console.ReadLine(), 16);

                status = mifarecom.LoadKey(key, keylocnum, KEY_LENGTH);
                Console.WriteLine("LoadKey...{0}", MifareStatusCodes.GetMifareStatusCodeInfo(status));
            }
            catch (Exception e)
            {
                Console.WriteLine("Parametro Invalido");
            }
        }

        #endregion

        #region VerifyCard
        public  void VerifyCard()
        {
            int status = 0;
            byte[] data = new byte[16];
            int sector = 0;
            //int blocknumber = 0;
            int block = 0;

            Console.ForegroundColor = ConsoleColor.Green;

            try
            {
                //First Load Transport Key (FF FF FF FF FF FF) in Loc 0x00  Length 6 bytes
                status = mifarecom.LoadKey(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 0, 6);
                // Second Load MAD key A in Loc 0x01
                status = mifarecom.LoadKey(new byte[] { 0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5 }, 1, 6);

                //Try authenticate sector 0 with transport key
                
                for (sector = 0; sector < 16; sector++)
                {
                    //Authenticate Sector
                    Console.WriteLine("SECTOR {0}", sector);

                    status = mifarecom.Authenticate(MifareCommands.KEYA, 0, GetBlockNumber(sector, 0));
                    //if it is sector 0 and failed with transport key then try with MAD key
                    if (sector == 0 && status != MifareStatusCodes.SUCCESS)
                    {
                        //Thread.Sleep(50); // Just for the OK 5x27 CK
                        status = mifarecom.Authenticate(MifareCommands.KEYA, 1, GetBlockNumber(sector, 0));

                    }
                    if (status == MifareStatusCodes.SUCCESS)
                    {
                        //Read block 0 to 3
                        for (block = 0; block < 4; block++)
                        {
                            //block = 4 * sectornumber + blocknumber;
                            status = mifarecom.ReadData(GetBlockNumber(sector, block), ref data);
                            if (status == MifareStatusCodes.SUCCESS)
                            {
                                Console.WriteLine("Block [{0}] - {1}", block,
                                    ConvertArrays.ConvertByteArrayToStringHex(data, 16));
                            }
                            else
                            {
                                Console.WriteLine("Block [{0}] - {1}", block, MifareStatusCodes.GetMifareStatusCodeInfo(status));
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Authentication Failed", ConsoleColor.Red);
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.WriteLine();
                    //Thread.Sleep(30); // Just for the OK 5x27 CK
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine("Invalid Parameter");
            }

            Console.ForegroundColor = ConsoleColor.Gray;


        }
        #endregion

        #region GetBlockNumber
        private int GetBlockNumber(int sector, int block)
        {

            int blocknumber = 0;
            if (sector < 33)
                blocknumber = 4 * sector + block;
            else
                blocknumber = 128 + 16 * (sector - 32) + block;

            return blocknumber;
        }
        #endregion
    }
}
