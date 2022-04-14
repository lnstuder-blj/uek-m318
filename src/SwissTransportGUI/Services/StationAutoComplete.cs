using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SwissTransport.Core;
using SwissTransport.Models;

namespace SwissTransportGUI.Services
{
    internal class StationAutoComplete : Interfaces.IStationAutoComplete
    {
        private readonly ITransport _swissTransport;

        public StationAutoComplete(ITransport swissTransport)
        {
            _swissTransport = swissTransport;
        }

        public List<Station> PopulateSuggestions(
            List<Station> inputCollection, ref List<Station> suggestions)
        {
            // Remove all displayed search results that are not in the suggestions
            inputCollection.Except(suggestions)
                .ToList().ForEach(station => inputCollection.Remove(station));

            // Find all auto complete suggestions that are not already displayed.
            List<Station> notInList = suggestions
                .FindAll(station => !inputCollection.Contains(station));

            // Adds the missing elements to the displayed suggestions.
            if (notInList.Any())
                inputCollection.AddRange(notInList);

            return inputCollection;
        }

        public List<Station>? GetSuggestions(string stationNameQuery)
        {
            if (stationNameQuery.Trim().Length <= 2) return null;

            List<Station> suggestions = _swissTransport.GetStations(
                stationNameQuery).StationList;

            return suggestions;
        }
    }
}
