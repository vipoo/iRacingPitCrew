using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSDK;
using System.Threading;

namespace iRacingPitCrew
{
    public class DataCollector
    {
        Task process;
        bool requestCancel = false;
        DataSample lastestData;
        iRacingConnection iracing = new iRacingConnection();

        public event Action Connected
        {
            add { iracing.Connected += value;}
            remove { iracing.Connected -= value;}
        }
        
        public event Action Disconnected
        {
            add { iracing.Disconnected += value;}
            remove { iracing.Disconnected -= value;}
        }

        public bool IsConnectedToiRacing
        {
            get
            {
                return lastestData != null && lastestData.IsConnected;
            }
        }

        internal void Start()
        {
            if( process != null)
                return;

            lastestData = null;
            requestCancel = false;
            process = Task.Factory.StartNew(Process);
        }

        internal void Stop()
        {
            var p = process;
            process = null;

            if( p == null )
                return;

            requestCancel = true;
            p.Wait();
        }

        internal DataSample Data
        {
            get
            {
                return lastestData;
            }
        }

        void Process()
        {
            foreach( var data in iracing.GetDataFeed())
            {
                if (requestCancel)
                    return;

                lastestData = data;
            }
        }
    }
}
