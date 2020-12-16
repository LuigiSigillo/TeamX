using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TeamX.Utils;

namespace TeamX.Models
{
    public class Team : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //Not visible attributes
        public string Id { get; set; }
        public string CreatorID { get; set; }

        //Visible attributes
        public string Name { get; set; }
        public int MaxMembers { get; set; }
        public int NumOfMembers { get; set; }
        
        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                if (_city == value)
                    return;

                _city = value;
                OnPropertyChanged();
            }
        }
        public (double Latitude,double Longitude) PlaceDetails { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime TerminationDate { get; set; }
        public int Difficulty { get; set; }

        public string Members => NumOfMembers.ToString() + "/" + MaxMembers.ToString();

        public Team()
        {
            Id = null; ;
            CreatorID = null;
            Name = null;
            NumOfMembers = 1;
            MaxMembers = 0;
            City = null;
            Description = null;
            CreationDate = DateTime.Now;
            Difficulty = 1;
            TerminationDate = DateTime.Today.AddMonths(1);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
