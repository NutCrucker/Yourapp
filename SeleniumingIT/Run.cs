using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumingIT
{
    public class Run
    {
        static void Main(string[] args)
        {
            FirstSelenium Check = new FirstSelenium();
            Check.Login();
            string path = Check.getPath();
            string[] Groups = Check.StringOfGroups();
            for(int i=0;i<Groups.Length;i++)
            { 
                Check.PostToGroup(path, Groups[i]);
            }
            Console.WriteLine("Thank you for using the app, GoodBye!");
            Check.exitChrome();
            System.Windows.Forms.Application.Exit();
        }
    }
}
