using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace GBU_Server_Monitor
{
    [Serializable]
    public class Setting : INotifyPropertyChanged, ISerializable
    {
        private string _savePath;
        private int _anprTimeout;
        private int _importInterval;

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

        public int anprTimeout
        {
            get
            {
                return _anprTimeout;
            }
            set
            {
                if (value != _anprTimeout)
                {
                    _anprTimeout = value;
                    NotifyPropertyChanged("anprTimeout");
                }
            }
        }

        public int importInterval
        {
            get
            {
                return _importInterval;
            }
            set
            {
                if (value != _importInterval)
                {
                    _importInterval = value;
                    NotifyPropertyChanged("importInterval");
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
            _savePath = @"d:\anprtest\";
            _anprTimeout = 300;
            _importInterval = 500;
        }

        public Setting(SerializationInfo info, StreamingContext context)
        {
            _savePath = (string)info.GetValue("savePath", typeof(string));
            _anprTimeout = (int)info.GetValue("anprTimeout", typeof(int));
            _importInterval = (int)info.GetValue("importInterval", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("savePath", this._savePath);
            info.AddValue("anprTimeout", this._anprTimeout);
            info.AddValue("importInterval", this._importInterval);
        }

    }
}
