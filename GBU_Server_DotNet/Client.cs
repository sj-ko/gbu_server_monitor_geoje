using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql;
using FirebirdSql.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Threading;
using System.Data;

namespace GBU_Server_Monitor
{
    class Client
    {
        private string _serverAddr;

        // User=sysdba;Password=masterkey;Database=d:/anprdb/gbuanpr_geoje.fdb;Server=127.0.0.1;Port=3050;
        private string strConn = "";
        private FbConnection conn;
        private FbRemoteEvent fbEvent;
        private Thread dbReadThread;
        private bool _isdbReadThreadRunning = false;

        public delegate void OnANPRDetected(int channel, DateTime dateTime, string plateStr, string imagePath);
        public event OnANPRDetected ANPRDetected;

        public Client()
        {
            
        }

        public void InitConnection(string serverAddr)
        {
            _serverAddr = serverAddr;
            strConn = "User=sysdba;Password=masterkey;Database=d:/anprdb/gbuanpr_geoje.fdb;Server=" + serverAddr + ";Port=3050";
            conn = new FbConnection(strConn);
            conn.Open();
            fbEvent = new FbRemoteEvent(conn);
            fbEvent.AddEvents(new string[] { "item_inserted" });
            fbEvent.RemoteEventCounts += new EventHandler<FbRemoteEventEventArgs>(fbEvent_RemoteEventCounts);
            fbEvent.QueueEvents();

            dbReadThread = new Thread(DbReadThreadFunction);
        }

        public void Play()
        {
            StartClientThread();
        }

        public void Stop()
        {
            conn.Close();
            StopClientThread();
        }

        private void StartClientThread()
        {
            _isdbReadThreadRunning = true;
            if (dbReadThread != null)
            {
                dbReadThread.Start();
            }
        }

        private void StopClientThread()
        {
            _isdbReadThreadRunning = false;
            if (dbReadThread != null && dbReadThread.IsAlive)
            {
                dbReadThread.Join();
            }
        }

        private void DbReadThreadFunction()
        {
            while (_isdbReadThreadRunning)
            {

                // end of thread cycle
                Thread.Sleep(1);
            }
        }

        private void fbEvent_RemoteEventCounts(object sender, FbRemoteEventEventArgs e)
        {
            Console.WriteLine("Event {0} has {1} counts.", e.Name, e.Counts);

            // SELECT first 1 * FROM anprtable ORDER BY serialno DESC
            try
            {
                DataTable resultTable;
                DataSet ds = new DataSet();

                FbDataAdapter da = new FbDataAdapter("SELECT first 1 * FROM anprtable ORDER BY serialno DESC", conn);
                da.Fill(ds, "mytable");

                DataTable dt = ds.Tables["mytable"];
                foreach (DataRow dr in dt.Rows)
                {
                    Console.WriteLine(string.Format("Name = {0}, Desc = {1}", dr["ANPRTIME"], dr["PLATE"]));
                }
                resultTable = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "::" + ex.StackTrace);
            }
        }

        ~Client()
        {

        }

    }
}
