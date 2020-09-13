using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FocusAccess;
using FocusMonitoring;

namespace FocusMonitorngTray
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
            var key = new FocusKey("3208d29d15c507395db770d0e65f3711e40374df");
            using (var tray = new IconContext(new Monitorer(new Api(key), new DifferentialApi(key), new MonitoringFactory()))) 
                Application.Run(tray);
        }
    }
}