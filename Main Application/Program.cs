using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Main_Application
{
    public class Program
    {
        private static Heartbeat _heartbeat;
        private static DateTime _lastInterupt = DateTime.MinValue;

        public static void Main()
        {
            // write your code here
            _heartbeat = new Heartbeat(new OutputPort(Pins.ONBOARD_LED, false), 250);
            _heartbeat.Start();

            var button = new InterruptPort(Pins.ONBOARD_BTN, true, Port.ResistorMode.PullUp, Port.InterruptMode.InterruptEdgeLow);
            button.OnInterrupt += new NativeEventHandler(OnButtonStateChange);

            Thread.Sleep(Timeout.Infinite);
        }

        static void OnButtonStateChange(uint data1, uint data2, DateTime time)
        {
            lock (_heartbeat)
            {
                if (_heartbeat == null) return;

                var now = DateTime.Now;
                if (now < _lastInterupt.AddMilliseconds(5)) return;
                _lastInterupt = now;

                if (_heartbeat.Running)
                {
                    _heartbeat.Stop();
                }
                else
                {
                    _heartbeat.Start();
                } 
            }
        }

    }
}
