using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JvlMifareTest
{
    public delegate void SampleFunctionDelegate();

    struct CmdInfo
    {
        public String strCmd;
        public String Description;
    }

    public class ConsoleMenu
    {
        public string MenuTitle = String.Empty;

        Dictionary<CmdInfo, SampleFunctionDelegate> actions = new Dictionary<CmdInfo, SampleFunctionDelegate>();

        public void AddMenuChoice(String keyCmd, String commandDescription, SampleFunctionDelegate functionToCall)
        {
            CmdInfo k = new CmdInfo();
            k.strCmd = keyCmd;
            k.Description = commandDescription;

            actions.Add(k, functionToCall);
        }

        public void DrawMenu()
        {
            Console.Title = "MIFARE TEST Ver 1.2.1.3";

           
            
            Console.WriteLine("=======================================");
            
            Console.WriteLine("         {0}  ", MenuTitle);
            
            Console.WriteLine("=======================================");
            
            Console.WriteLine();

            foreach (KeyValuePair<CmdInfo, SampleFunctionDelegate> kvp in actions)
            {
                Console.WriteLine("[{0}]  {1}", kvp.Key.strCmd, kvp.Key.Description);
            }
            Console.WriteLine();
            Console.WriteLine("=======================================");
            Console.WriteLine("  Clear");
            Console.WriteLine("  Menu");
            Console.WriteLine("  Exit");
            Console.WriteLine("=======================================");
            //Console.Write("> ");
        }

        public void Execute(String cmd)
        {
            bool cmdExits = false;
            foreach (KeyValuePair<CmdInfo, SampleFunctionDelegate> kvp in actions)
            {
                if (kvp.Key.strCmd == cmd)
                {
                    try
                    {
                        actions[kvp.Key]();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An unhandled exception - " + e);
                    }
                    cmdExits = true;
                    break;
                }
            }
            if (!cmdExits)
                Console.WriteLine("What?");
        }
    }
}
