﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DevDaysSpeakers.Model;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DevDaysSpeakers.ViewModel
{
    public class SpeakersViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Speaker> Speakers { get; set; }
        public SpeakersViewModel()
        {
            Speakers = new ObservableCollection<Speaker>();
        }

        bool busy;

        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public async Task GetSpeakers()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;


                var items = await AzureStore.Current.GetSpeakers();

                Speakers.Clear();
                foreach (var item in items)
                    Speakers.Add(item);
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            var changed = PropertyChanged;

            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}