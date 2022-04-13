using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SwissTransport.Core;
using SwissTransport.Models;

namespace SwissTransportGUI.ViewModels
{
    internal class ConnectionsViewModel : BindableBase
    {
        private string _departureStationNameInput;
        public string DepartureStationNameInput
        {
            get => _departureStationNameInput;
            set => SetProperty(ref _departureStationNameInput, value, OnDepartureStationNameInputChanged);
        }

        private string _arrivalStationNameInput;
        public string ArrivalStationNameInput
        {
            get => _arrivalStationNameInput;
            set => SetProperty(ref _arrivalStationNameInput, value, OnArrivalStationNameInputChanged);
        }

        public ObservableCollection<Connection> ConnectionsList { get; }
        public ObservableCollection<Station> DepartureStationSearchResult { get; }
        public ObservableCollection<Station> ArrivalStationSearchResult { get; }

        public Station? SelectedDepartureStation { get; set; }
        public Station? SelectedArrivalStation { get; set; }

        public ICommand SearchDepartureStationCommand { get; }
        public ICommand SearchArrivalStationCommand { get; }
        public ICommand SearchConnectionsCommand { get; }

        private ITransport _swissTransport;

        public ConnectionsViewModel(ITransport swissTransport)
        {
            _swissTransport = swissTransport;

            SearchDepartureStationCommand = new DelegateCommand(OnSearchDepartureStation);
            SearchArrivalStationCommand = new DelegateCommand(OnSearchArrivalStation);
            SearchConnectionsCommand = new DelegateCommand(OnSearchConnections);

            ConnectionsList = new ObservableCollection<Connection>();
            DepartureStationSearchResult = new ObservableCollection<Station>();
            ArrivalStationSearchResult = new ObservableCollection<Station>();

            DepartureStationNameInput = string.Empty;
            ArrivalStationNameInput = string.Empty;
        }

        private void OnDepartureStationNameInputChanged()
        {

        }

        private void OnArrivalStationNameInputChanged()
        {

        }


        private void OnSearchConnections()
        {
            if (SelectedArrivalStation == null || SelectedDepartureStation == null) return;
            List<Connection> connections = _swissTransport
                .GetConnections(SelectedDepartureStation.Name, SelectedArrivalStation.Name).ConnectionList.Take(10)
                .ToList();
            ConnectionsList.AddRange(connections);
        }

        private void OnSearchArrivalStation()
        {
            ArrivalStationSearchResult.Clear();
            List<Station> stationsFound = _swissTransport.GetStations(ArrivalStationNameInput).StationList;
            stationsFound.ForEach(station => ArrivalStationSearchResult.Add(station));
        }

        private void OnSearchDepartureStation()
        {
            DepartureStationSearchResult.Clear();
            List<Station> stationsFound = _swissTransport.GetStations(DepartureStationNameInput).StationList;
            stationsFound.ForEach(station => DepartureStationSearchResult.Add(station));
        }
    }
}
