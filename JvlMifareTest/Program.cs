/** JvlMifareTest.cs
 * Decription: Programm to test mifare functionalities of the HID Omnikey readers 
 * Author: Jorge Villicana 
 * Last Update: August 26th 2014
 * */

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using JvlMifare;

namespace JvlMifareTest
{
    class Program
    {
       

        
        static void Main(string[] args)
        {
            ReaderDetector readerDetector = new ReaderDetector();
            MifareFunctions mifareFunc = new MifareFunctions(readerDetector);

            ConsoleMenu menu = new ConsoleMenu();

            menu.MenuTitle = "MIFARE COMMANDS DEMO";


            menu.AddMenuChoice("1", "List Readers ", new SampleFunctionDelegate(mifareFunc.SelectReader));
            
            menu.AddMenuChoice("2", "Check If Card is Present ", new SampleFunctionDelegate(mifareFunc.CheckIfCardIsPresent));
            menu.AddMenuChoice("3", "Connect Card ", new SampleFunctionDelegate(mifareFunc.ConnectCard));
            menu.AddMenuChoice("4", "Disconnect Card ", new SampleFunctionDelegate(mifareFunc.DisconnectCard));
            menu.AddMenuChoice("5", "Get CSN ", new SampleFunctionDelegate(mifareFunc.GetCSN));
            menu.AddMenuChoice("6", "Load Key ", new SampleFunctionDelegate(mifareFunc.LoadKey));
            menu.AddMenuChoice("7", "Authenticate ", new SampleFunctionDelegate(mifareFunc.Authenticate));
            menu.AddMenuChoice("8", "Read Block ", new SampleFunctionDelegate(mifareFunc.ReadBlock));
            menu.AddMenuChoice("9", "Write Block ", new SampleFunctionDelegate(mifareFunc.WriteBlock));
            menu.AddMenuChoice("10", "Increment ", new SampleFunctionDelegate(mifareFunc.Increment));
            menu.AddMenuChoice("11", "Decrement ", new SampleFunctionDelegate(mifareFunc.Decrement));
            menu.AddMenuChoice("12", "GetAccessBitsInfo ", new SampleFunctionDelegate(mifareFunc.GetAccessBitsInfo));
            menu.AddMenuChoice("13", "Verify  Mifare 1k Card", new SampleFunctionDelegate(mifareFunc.VerifyCard));
            menu.DrawMenu();
            Console.Write(">");
            do
            {
                bool WasIdentified = false;
                string ans = Console.ReadLine();

                if (ans.ToUpper() == "EXIT")

                    break;
                if (ans.ToUpper() == "CLEAR")
                {
                    WasIdentified = true;
                    Console.Clear();
                }

                if (ans.ToUpper() == "MENU")
                {
                    WasIdentified = true;
                    menu.DrawMenu();
                }

                if (!WasIdentified)
                    menu.Execute(ans);
                Console.Write(">");

            } while (true);

        }
    }

}
