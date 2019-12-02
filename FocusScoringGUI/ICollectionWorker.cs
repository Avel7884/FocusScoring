using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FocusScoringGUI
{
    public interface ICollectionWorker
    {
        bool IsBusy();
        void WorkOn<T>(ICollection<T> collection,Func<int,T,object> work);

        void StopWork();
        //event Action<T,DoWorkEventArgs> DoWork;
        event ProgressChangedEventHandler ProgressChanged;
        event RunWorkerCompletedEventHandler RunWorkerCompleted;
    }
}