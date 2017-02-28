using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Net;
using System.IO;
using System.Collections.Concurrent;
using gx;
using cm;

namespace GBU_Server_Monitor
{
    class ImageImpoter
    {
        private string _savepath = "";
        public string SavePath
        {
            get
            {
                return _savepath;
            }
            set
            {
                _savepath = value;
            }
        }

        private Database db;
        private Thread MediaThread, ANPRThread;
        private Uri camUrl;
        private string camUsername, camPassword;
        private int mediaThreadInterval = 100; // ms
        private bool _isMediaThreadRunning = false, _isANPRThreadRunning = false;
        private LimitedQueue<byte[]> imageList = new LimitedQueue<byte[]>(128);

        // Creates the ANPR object
        private cmAnpr anpr = new cmAnpr("default");

        private int cameraID;

        public delegate void OnANPRDetected(int channel, DateTime dateTime, string plateStr, string imagePath);
        public event OnANPRDetected ANPRDetected;

        private string[] KOREA_LOCALAREA_LIST = {"서울","인천","세종","대전","대구","부산","광주","울산",
                                            "경기","강원","충북","충남","경북","경남","전북","전남","제주"};

        private string[] KOREA_CHAR_LIST = {"가","나","다","라","마","바","사","아","자",
                                           "거","너","더","러","머","버","서","어","저",
                                           "고","노","도","로","모","보","소","오","조",
                                           "구","누","두","루","무","부","수","우","주",
                                           "하","허","호","배"};

        public ImageImpoter()
        {
            MediaThread = new Thread(MediaThreadFunction);
            ANPRThread = new Thread(ANPRThreadFunction);
            db = new Database();
        }

        ~ImageImpoter()
        {
            StopMediaThread();
        }

        public void InitCamera(int camID, string url, int interval, string username, string password, int anprTimeout)
        {
            camUrl = new Uri(url, UriKind.Absolute);
            mediaThreadInterval = interval;
            cameraID = camID;

            camUsername = username;
            camPassword = password;

            // anpr init
            initANPR(anprTimeout);
        }

        public void Play()
        {
            StartMediaThread();
            StartANPRThread();
        }

        public void Stop()
        {
            StopANPRThread();
            StopMediaThread();
        }

        public byte[] GetImage()
        {
            byte[] image = null;

            try
            {
                image = imageList.Dequeue();
            }
            catch (InvalidOperationException e)
            {
                image = null; // queue empty
            }

            return image;
        }

        private void initANPR(int anprTimeout)
        {
            // set anpr property
            anpr.SetProperty("anprname", "cmanpr-7.3.9.5:kor");
            anpr.SetProperty("size", "47"); // default 25  (20-->15)
            anpr.SetProperty("size_min", "8"); //"8"); // Default 6
            anpr.SetProperty("size_max", "82"); //"40"); // Default 93

            anpr.SetProperty("nchar_min", "4"); // "7"); // Default 8
            anpr.SetProperty("nchar_max", "9"); // Default 9

            anpr.SetProperty("slope", "4"); // "-5"); // Default -22
            anpr.SetProperty("slope_min", "-31"); //-20"); // Default -22
            anpr.SetProperty("slope_max", "37"); // "10"); // Default 34

            anpr.SetProperty("slant", "4"); // "0"); // Default 10
            anpr.SetProperty("slant_min", "-13"); // "-10"); // Default -55
            anpr.SetProperty("slant_max", "56"); // "10"); // Default 27

            anpr.SetProperty("timeout", anprTimeout); //"300"); // default 100 

            anpr.SetProperty("contrast_min", "9");
            anpr.SetProperty("xtoyres", "100");
            anpr.SetProperty("colortype", "0");
            anpr.SetProperty("gaptospace", "0");
            anpr.SetProperty("unicode_in_text", "0");
            anpr.SetProperty("general", "1");
            anpr.SetProperty("depth", "205");
            anpr.SetProperty("adapt_environment", "1");
            anpr.SetProperty("unit", "1");
            anpr.SetProperty("analyzecolors", "0");
            anpr.SetProperty("whitebalance", "100");
            anpr.SetProperty("general", "13");
            anpr.SetProperty("plateconf", "0");
        }

        private void StartMediaThread()
        {
            _isMediaThreadRunning = true;
            if (MediaThread != null)
            {
                MediaThread.Start();
            }
        }

        private void StopMediaThread()
        {
            _isMediaThreadRunning = false;
            if (MediaThread != null && MediaThread.IsAlive)
            {
                MediaThread.Join();
            }
        }

        private void StartANPRThread()
        {
            _isANPRThreadRunning = true;
            if (ANPRThread != null)
            {
                ANPRThread.Start();
            }
        }

        private void StopANPRThread()
        {
            _isANPRThreadRunning = false;
            if (ANPRThread != null && ANPRThread.IsAlive)
            {
                ANPRThread.Join();
            }
        }

        private void MediaThreadFunction()
        {
            WebClient webClient = new WebClient();
            //webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(imageDownload_openReadCompleted);
            webClient.Credentials = new NetworkCredential(camUsername, camPassword); // to do : change to SecureString
            //webClient.OpenRead(camUrl);

            while (_isMediaThreadRunning)
            {
                

                try
                {
                    byte[] imageData = webClient.DownloadData(camUrl); // get jpeg byte from http
                    imageList.Enqueue(imageData);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Connection error Camid " + cameraID + " to " + camUrl + "   exception : " + e.Message);
                }
                
                // end of thread cycle - mediaThreadInterval
                Thread.Sleep(mediaThreadInterval); 
            }

        }

        private void ANPRThreadFunction()
        {
            while (_isANPRThreadRunning)
            {
                byte[] imageData = GetImage(); // dequeue image
                int foundcount = 0;

                if (imageData != null)
                {
                    try
                    {
                        gxImage gximage = new gxImage("default");
                        gximage.LoadFromMem(imageData, (int)GX_PIXELFORMATS.GX_FORMAT_FIRST);

                        // Finds the first plate and displays it
                        bool isFound = anpr.FindFirst(gximage);
                        while (isFound)
                        {
                            foundcount++;
                            // plate found
                            DateTime datetime = DateTime.Now;
                            string plateStr = anpr.GetText();

                            if (isValidPlateString(plateStr))
                            {
                                string plateImageFilepath = _savepath + "\\ch" + cameraID;
                                string dtStr = String.Format("{0:yyyyMMdd_HHmmss}", datetime);
                                string plateImageFilename = plateImageFilepath + "\\CAM-" + cameraID + "_" + dtStr + "_" + plateStr + ".jpg";
                                Console.WriteLine("Result: '{0}', ch {1}", plateStr, cameraID);

                                // write anpr snapshot
                                if (!Directory.Exists(plateImageFilepath))
                                {
                                    Directory.CreateDirectory(plateImageFilepath);
                                }
                                File.WriteAllBytes(plateImageFilename, imageData);
                                // write db
                                db.InsertPlate(cameraID, datetime, plateStr, plateImageFilename); // db write 
                                //db.InsertPlateText(camera.camID, DateTime.Now, plateStr, returnImage); // file write test

                                // invoke anpr event to mainform
                                if (ANPRDetected != null)
                                {
                                    ANPRDetected(cameraID, datetime, plateStr, plateImageFilename);
                                }
                            }
                            else
                            {
                                // wrong plate
                                Console.WriteLine("Wrong plate found ch " + cameraID);
                            }

                            isFound = anpr.FindNext();
                        }
                        /*else
                        {
                            // no plate
                            Console.WriteLine("No plate found ch " + cameraID);
                        }*/
                        if (foundcount == 0)
                        {
                            // no plate
                            Console.WriteLine("No plate found ch " + cameraID);
                        }
                    }
                    catch (gxException e)
                    {
                        Console.WriteLine("Exception occurred: {0}", e.Message);
                    }
                }

                // end of thread cycle
                Thread.Sleep(1); 
            }
        }

        private void imageDownload_openReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            Stream s = e.Result;
            //
            byte[] imageData = ReadFully(s); // get jpeg byte from http
            imageList.Enqueue(imageData);

            s.Close();
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        bool isValidPlateString(string plateValue)
        {
            bool isValidChar = false;

            if (plateValue.Length > 8 && (plateValue[0] < 0 || Char.IsDigit(plateValue[0]) == false))
            {
                // Check Old (Loca-12-Kr-1234)
                if (plateValue[2] < 0 || Char.IsDigit(plateValue[2]) == false) return false;
                if (plateValue[3] < 0 || Char.IsDigit(plateValue[3]) == false) return false;
                if (plateValue[5] < 0 || Char.IsDigit(plateValue[5]) == false) return false;
                if (plateValue[6] < 0 || Char.IsDigit(plateValue[6]) == false) return false;
                if (plateValue[7] < 0 || Char.IsDigit(plateValue[7]) == false) return false;
                if (plateValue[8] < 0 || Char.IsDigit(plateValue[8]) == false) return false;

                foreach (string str in KOREA_CHAR_LIST)
                {
                    if (plateValue.Substring(4, 1).Equals(str))
                    {
                        isValidChar = true;
                    }
                }
            }
            else
            {
                // 2006 yr. (12-Kr-1234)
                if (plateValue.Length != 7) return false;
                if (plateValue[1] < 0 || Char.IsDigit(plateValue[1]) == false) return false;
                if (plateValue[3] < 0 || Char.IsDigit(plateValue[3]) == false) return false;
                if (plateValue[4] < 0 || Char.IsDigit(plateValue[4]) == false) return false;
                if (plateValue[5] < 0 || Char.IsDigit(plateValue[5]) == false) return false;
                if (plateValue[6] < 0 || Char.IsDigit(plateValue[6]) == false) return false;

                foreach (string str in KOREA_CHAR_LIST)
                {
                    if (plateValue.Substring(2, 1).Equals(str))
                    {
                        isValidChar = true;
                    }
                }
            }

            return isValidChar;
        }

        private string getAdjustPlate(string plate)
        {
            string str = plate;

            bool isWrongArea = false;

            if (str.Length > 8)
            {
                // Check Old and Biz (Loca-12-Kr-1234)
                if (Char.IsDigit(str[0]) == false)
                {
                    // Check Old (Loca-12-Kr-1234) Local area
                    foreach (string areaName in KOREA_LOCALAREA_LIST)
                    {
                        if (!str.Contains(areaName))
                        {
                            isWrongArea = true;
                        }
                    }

                    // adjust area
                    if (isWrongArea == true)
                    {
                        if (str.StartsWith("서"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "서울");
                        }

                        else if (str.Substring(0, 2).EndsWith("울"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "서울");
                        }

                        else if (str.Substring(0, 2).Equals("무산"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "부산");
                        }

                        else if (str.StartsWith("부"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "부산");
                        }

                        /*else if (str.Substring(0, 2).EndsWith("산"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "부산");
                        }*/

                        else if (str.Substring(0, 2).Equals("경거"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "경기");
                        }

                        else if (str.Substring(0, 2).EndsWith("기"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "경기");
                        }

                        else if (str.Substring(0, 2).Equals("천북"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "전북");
                        }

                        else if (str.Substring(0, 2).Equals("천남"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "전남");
                        }

                        else if (str.StartsWith("울"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "울산");
                        }

                        else if (str.Substring(0, 2).Equals("경허"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "경기");
                        }

                        else if (str.StartsWith("세"))
                        {
                            str = str.Remove(0, 2);
                            str = str.Insert(0, "세종");
                        }
                    }
                }

            }
            else
            {

            }

            return str;
        }

        public class LimitedQueue<T> : Queue<T>
        {
            public int Limit { get; set; }

            public LimitedQueue(int limit)
                : base(limit)
            {
                Limit = limit;
            }

            public new void Enqueue(T item)
            {
                while (Count >= Limit)
                {
                    Dequeue();
                }
                base.Enqueue(item);
            }
        }

    }
}
