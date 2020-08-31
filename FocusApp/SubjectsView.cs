/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using FocusApiAccess;
using FocusScoring;

namespace FocusApp
{
    public class SubjectsView : IProvidableInteractiveView<IFocusDataBase<INN>>,IDynamicInteractiveView<INN>
    {/*
        private string[] inns = @"6663003127	561100409545 7708503727	666200351548 7736050003	366512608416 7452027843	773173084809 6658021579	771409116994 7725604637	503115929542 4401006984	773400211252 3016003718	771902452360 5053051872	702100195003"
            .Split('	',' ');#1#
        private Api3 Api;//= new FocusKey("3208d29d15c507395db770d0e65f3711e40374df").StartApiAccess();
        private readonly IScorer<INN> scorer;
        private readonly IProvidableInteractiveView<MarkerResult<INN>[]> markersView;
        private IFocusDataBase<INN> focusDataBase;

        public SubjectsView(Api3 api, IScorer<INN> scorer,IProvidableInteractiveView<MarkerResult<INN>[]> markersView)
        {
            Api = api;
            this.scorer = scorer;
            this.markersView = markersView;
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            //return markersView.GetEnumerator();
            return focusDataBase?.GetEnumerator() ?? new int[0].GetEnumerator();
            //return (res = inns.Select(Name).ToArray()).GetEnumerator();
        }
        
        public void Selected(int index)
        {
            markersView.Provide(scorer.Score(focusDataBase[index].Subject).Markers);
        }

        public void Provide(IFocusDataBase<INN> focusData)
        {
            focusDataBase = focusData;
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Add(INN inn)
        {
            focusDataBase.Write(inn);
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,inn,0));
        }

        public void Delete(INN inn)
        {
            throw new NotImplementedException();
            focusDataBase.Delete(inn);
            //CollectionChanged?.Invoke(this,new EventArgs());
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}*/