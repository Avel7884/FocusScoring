﻿/*using System;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    [Serializable]
    public class CompanyData
    {
        //TODO Attempt to remove this class
        
        
        [XmlAttribute]
        private int score;
        internal Company Company { get; set; }
        
        public CompanyData(){}//Required for XML serialization
        
        public CompanyData(string Inn,FocusScoringManager manager)
        {
            this.Inn = Inn;
            Company = manager.CreateFromInn(Inn);
            Name = Company.CompanyName();
            /*IsChecked = false;#1#
            score = -1;
        }
         /*

        [XmlAttribute]
        public bool IsChecked { get; set; }#1#
        [XmlIgnore]
        public string Inn { get; set; }
        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore] public string Score => score == -1 ? "" : score.ToString();

        [XmlIgnore]
        public Light Light { get; set; }

        [XmlIgnore] public BitmapImage CLight => /*IsChecked ?#1# new BitmapImage(ShieldCode(Light));/* : null;#1#

        public void Check(FocusScoringManager manager, bool force = false)
        {
/*
            IsChecked = true;
#1#
            Company = Company ?? manager.CreateFromInn(Inn);
            if(force)
                Company.ForcedMakeScore();
            else
                Company.MakeScore();
            score = Company.Score;
            InitLight();
        }

        private void InitLight()
        {
            if (score < 0)
                return;
            
            if (score <= 39)
            {
                Light = Light.Red;
                //CLight = new BitmapImage(ShieldCode(Light.Red));                    
            }
            if (score > 39 && score <= 69)
            {
                Light = Light.Yellow;
                //CLight = new BitmapImage(ShieldCode(Light.Yellow));
            }

            if (score > 69)
            {
                Light = Light.Green;
                //CLight = new BitmapImage(ShieldCode(Light.Green));
            }
        }
        
        private Uri ShieldCode(Light light)
        {
            switch (light)
            {
                case Light.Green: return new Uri("pack://application:,,,/src/green-shield.png");
                case Light.Red: return new Uri("pack://application:,,,/src/red-shield.png");
                case Light.Yellow: return new Uri("pack://application:,,,/src/yellow-shield.png");
                default: throw new AggregateException();
            }
        }
    }
    
}*/