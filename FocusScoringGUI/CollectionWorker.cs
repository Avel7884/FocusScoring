using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;

namespace FocusScoringGUI
{
    public class CollectionWorker: ICollectionWorker
    {
        private BackgroundWorker Worker;
        private ProgressWindow Progress;

        public CollectionWorker(string WindowName = "Прогресс проверки")
        {
             Progress = new ProgressWindow(WindowName);
            Progress.Show();
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
        }
        
        public void WorkOn<T>(ICollection<T> collection,Func<int,T,object> work)
        {
            if(Worker!= null && Worker.IsBusy)
                return;
            
            var len = collection.Count;
            //for(int i=0;i<len;i++)
            foreach (var (i,el) in Enumerable.Range(0,len).Zip(collection,ValueTuple.Create))
                Worker.DoWork += (o, e) =>
                {
                    var bv = o as BackgroundWorker;
                    if(bv.CancellationPending) return;
                    var data = work?.Invoke(i, el);
                    if(bv.CancellationPending) return;
                    bv.ReportProgress(i*100/len, data);
                };
            
            Worker.ProgressChanged += (o,e) =>
            {
                Progress.Bar.Value = e.ProgressPercentage;
                ProgressChanged?.Invoke(o,e);
            };
            
            Worker.RunWorkerCompleted += (o,e) =>
            {
                Progress.Close();
                RunWorkerCompleted?.Invoke(o,e);
            };
            
            Worker.RunWorkerAsync(100000);
        }

        public void StopWork()
        {
            Worker.CancelAsync();
        }
        
        //public event Action<T, DoWorkEventArgs> DoWork;
        public bool IsBusy()
        {
            return Worker?.IsBusy ?? false;
        }

        public event ProgressChangedEventHandler ProgressChanged;
        public event RunWorkerCompletedEventHandler RunWorkerCompleted;
    }
}