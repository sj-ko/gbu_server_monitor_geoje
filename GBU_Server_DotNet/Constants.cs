using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBU_Server_Monitor
{
    public static class Constants
    {
        public const int CANDIDATE_REMOVE_TIME = 1000; //60000; // ms
        public const int CANDIDATE_COUNT_FOR_PASS = 2; // default is 3, for gas station stop is 20
        public const int MAX_IMAGE_BUFFER = 512; //30;
    }
}
