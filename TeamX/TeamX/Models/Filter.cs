using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TeamX.Models
{
   public class Filter: INotifyPropertyChanged
    {
       private string citta;
       private string cat;
       private double dis;
       private string diff;
    
       public  string City
        {
            get { return citta; }
            set { SetField(ref citta, value, "City"); }
        }

       public  string Category
        {
            get { return cat; }
            set { SetField(ref cat, value, "Category"); }
        }

        public string Difficulty
        {
            get { return diff; }
            set { SetField(ref diff, value, "Category"); }
        }

        public  double Distance
        {
            get { return dis; }
            set { SetField(ref dis, value, "Distance"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
