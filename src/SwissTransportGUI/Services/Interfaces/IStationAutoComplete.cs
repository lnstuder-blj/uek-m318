using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwissTransport.Core;
using SwissTransport.Models;

namespace SwissTransportGUI.Services.Interfaces
{
    internal interface IStationAutoComplete
    {
        public List<Station> PopulateSuggestions(List<Station> inputCollection, ref List<Station> suggestions);

        public List<Station>? GetSuggestions(string stationNameQuery);
    }
}
