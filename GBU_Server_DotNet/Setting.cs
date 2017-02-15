using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GBU_Server_Monitor
{
    public class Setting : INotifyPropertyChanged
    {
        private string _savePath;
        private string _serverPath;
        private string _configPath;
        private int _nChannel;

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

        public string serverPath
        {
            get
            {
                return _serverPath;
            }
            set
            {
                if (value != _serverPath)
                {
                    _serverPath = value;
                    NotifyPropertyChanged("serverPath");
                }
            }
        }

        public string configPath
        {
            get
            {
                return _configPath;
            }
            set
            {
                if (value != _configPath)
                {
                    _configPath = value;
                    NotifyPropertyChanged("configPath");
                }
            }
        }

        public int nChannel
        {
            get
            {
                return _nChannel;
            }
            set
            {
                if (value != _nChannel)
                {
                    _nChannel = value;
                    NotifyPropertyChanged("nChannel");
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
            //_serverPath = @"C:\GBUANPR\Server";
            //_configPath = @"C:\GBUANPR\Config";
            _serverPath = @"d:\anpr_161201_test\";
            _configPath = @"d:\anpr_161201_test\cfg";
            _nChannel = 5;
        }

        public Setting(int id)
        {
            _savePath = @"c:\anprtest\";
            //_serverPath = @"C:\GBUANPR\Server";
            //_configPath = @"C:\GBUANPR\Config";
            _serverPath = @"d:\anpr_161201_test\";
            _configPath = @"d:\anpr_161201_test\cfg";
            _nChannel = 5;
        }

        public Setting(int id, string url)
        {
            //_savePath = @"c:\anprtest";
            //_serverPath = @"C:\Dev\GBU_Server_DotNet_Gaenari_Rev1\GBU_Server_DotNet\bin\x86\Debug";
            //_configPath = @"C:\Dev\gaenari_cfg\test2";
            _savePath = @"c:\anprtest\";
            //_serverPath = @"C:\GBUANPR\Server";
            //_configPath = @"C:\GBUANPR\Config";
            _serverPath = @"d:\anpr_161201_test\";
            _configPath = @"d:\anpr_161201_test\cfg";
            _nChannel = 5;
        }

    }
}
