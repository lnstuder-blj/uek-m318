using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Mvvm;
using SwissTransport.Core;
using SwissTransport.Models;

namespace SwissTransportGUI.ViewModels
{
    internal class DepartureBoardViewModel : BindableBase
    {
        public string StationNameInput { get; set; }
        public ObservableCollection<Station> StationSearchResult { get; private set; }
        public bool ShowStationsDropDown { get; set; }
        public ObservableCollection<StationBoard> DeparturesList { get; private set; }
        public Station SelectedStation { get; set; }

        public ICommand SearchStationsCommand { get; }
        public ICommand FindDeparturesCommand { get; }

        private readonly ITransport _swissTransport;

        public DepartureBoardViewModel(ITransport swissTransport)
        {
            StationNameInput = string.Empty;
            StationSearchResult = new ObservableCollection<Station>();
            DeparturesList = new ObservableCollection<StationBoard>();

            SearchStationsCommand = new DelegateCommand(SearchStations);
            FindDeparturesCommand = new DelegateCommand(FindDepartures);

            _swissTransport = swissTransport;
        }

        private void SearchStations()
        {
            List<Station> stationsFound = _swissTransport.GetStations(StationNameInput).StationList;
            stationsFound.ForEach(station => StationSearchResult.Add(station));

            if (stationsFound.Count > 0)
                ShowStationsDropDown = true;
        }

        private void FindDepartures()
        {
            List<StationBoard> stationBoards = _swissTransport.GetStationBoard(SelectedStation.Name, SelectedStation.Id).Entries;
            stationBoards.Take(10).ToList().ForEach(board => DeparturesList.Add(board));
        }
    }
}
