using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FocusMonitoring;

namespace FocusMonitorngTray
{
    public class IconContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Timer timer;
        private IMonitorer mon;

        public IconContext(IMonitorer mon)
        {
            this.mon = mon;
            
            trayIcon = new NotifyIcon
            {
                Icon = new Icon(".\\Icon1.ICO"),
                ContextMenu = new ContextMenu(new []{
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };
            
            Notify();
            InitTimer();
            
        }

        private void Notify()
        {
            trayIcon.BalloonTipText = File.ReadAllText("./ShortLog");
            trayIcon.ShowBalloonTip(3000);
            mon.PerformMonitoring();
        }

        private void InitTimer()
        {
            timer = new Timer();
            timer.Interval = (TimeSpan.FromHours(9) + 
                              (DateTime.Now.Hour > 9 ? TimeSpan.FromDays(1) : TimeSpan.Zero) -
                              DateTime.Now.TimeOfDay).Milliseconds;
            timer.Tick += (o, a) =>
            {
                if (timer.Interval < TimeSpan.TicksPerDay)
                    timer.Interval = unchecked((int)TimeSpan.TicksPerDay);
                Notify();
            };
            timer.Start();
        }

        //internal event Action<object,EventArgs> ResultCalled;

        private void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;

            Application.Exit();
        }
    }
}