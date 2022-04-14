using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissTransportGUI.Models
{
    internal class Departure
    {
        public string Train { get; set; }
        public string To { get; set; }
        public DateTime DepartureTime { get; set; }
        public int Delay { get; set; }
        public string Platform { get; set; }

        public Departure(string train, string to, DateTime departureTime, string platform, int delay = 0)
        {
            Train = train;
            To = to;
            DepartureTime = departureTime;
            Platform = platform;
            Delay = delay;
        }
    }
}
