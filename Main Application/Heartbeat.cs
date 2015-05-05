using System;
using Microsoft.SPOT;
using System.Threading;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Hardware;

namespace Main_Application
{
    class Heartbeat
    {
        private OutputPort _output;
        private int _delay;
        private Thread _heartbeat;
        private bool _stopRequested;

        public bool Running { get; set; }

        public Heartbeat(OutputPort heartbeatIndicator, uint delay)
        {
            _output = heartbeatIndicator;
            _delay = (int)delay;
            Running = false;
        }

        public void Start()
        {
            _stopRequested = false;
            _heartbeat = new Thread(HeartbeatThread);
            _heartbeat.Start();
            Running = true;
        }

        public void Stop()
        {
            _stopRequested = true;
        }

        private void HeartbeatThread()
        {
            while(!_stopRequested)
            {
                _output.Write(!_output.Read());
                Thread.Sleep(_delay);
            }
            _output.Write(false);
            Running = false;
        }
    }
}
