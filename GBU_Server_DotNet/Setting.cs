using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GBU_Server_Monitor
{
    [Serializable]
    public class Setting : INotifyPropertyChanged
    {
        private string _savePath;

        public string savePath
        {
            get
            {
                return _savePath;
            }
            set
            {
                if (value != _savePath)
                {
                    _savePath = value;
                    NotifyPropertyChanged("savePath");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Setting()
        {
            _savePath = @"c:\anprtest\";
        }

    }
}
