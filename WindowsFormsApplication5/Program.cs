using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*List<KeyValuePair<Heatmap.Position, int>> list = new List<KeyValuePair<Heatmap.Position, int>>();

            list.Add(new KeyValuePair<Heatmap.Position, int>(new Heatmap.Position() { col = 'A', row = 1 }, 3));
            list.Add(new KeyValuePair<Heatmap.Position, int>(new Heatmap.Position() { col = 'C', row = 2 }, 2));
            list.Add(new KeyValuePair<Heatmap.Position, int>(new Heatmap.Position() { col = 'D', row = 4 }, 1));
            list.Add(new KeyValuePair<Heatmap.Position, int>(new Heatmap.Position() { col = 'B', row = 3 }, 5));
            */
            //var list = TeamStatistics.CreatePositionsList(2630);

            //Application.Run(new Heatmap(@"D:\Scouting2017\TestDBs\FieldCropped.PNG", list));
            Application.Run(new ControlPanel());
            //Application.Run(new Form1());
        }
    }
}
