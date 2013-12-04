using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;

namespace JoinFile
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            EmbeddedAssembly.Load("JoinFile.SpreadsheetGear.dll", "SpreadsheetGear.dll");
            EmbeddedAssembly.Load("JoinFile.Ionic.Zip.dll", "Ionic.Zip.dll");

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return EmbeddedAssembly.Get(args.Name);
        }
    }
}
