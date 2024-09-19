using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Gui;
using System.Windows.Forms;
using System.IO;
/*
 *@Author Yusuf Sait ERDEM
 */
    static class Test
    {
        public static ChartPlotter form;

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new ChartPlotter();

            Application.Run(form);
            Console.ReadLine();
        }
    }