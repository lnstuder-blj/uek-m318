using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Mvvm;
using SwissTransport.Core;
using SwissTransport.Models;
using SwissTransportGUI.Models;
using SwissTransportGUI.Services;
using SwissTransportGUI.Services.Interfaces;

namespace SwissTransportGUI.ViewModels
{
    internal class DepartureBoardViewModel : BindableBase
    {
        public ObservableCollection<Station> StationSuggestions { get; }
        
        public ObservableCollection<Departure> DeparturesList { get; }

        private string _stationNameSearchInput = string.Empty;
        public string StationNameSearchInput
        {
            get => _stationNameSearchInput;
            set => SetProperty(ref _stationNameSearchInput, 
                value, OnStationNameInputChanged);
        }

        private bool _suggestionsListIsVisible;
        public bool SuggestionsListIsVisible
        {
            get => _suggestionsListIsVisible;
            set => SetProperty(ref _suggestionsListIsVisible, value);
        }

        private bool _searchButtonIsEnabled;
        public bool SearchButtonIsEnabled
        {
            get => _searchButtonIsEnabled;
            set => SetProperty(ref _searchButtonIsEnabled, value);
        }

        private Station? _selectedStation;
        public Station? SelectedStation
        {
            get => _selectedStation;
            set => SetProperty(ref _selectedStation, 
                value, OnSelectedStationChanged);
        }

        public ICommand ShowDeparturesCommand { get; }
        public ICommand SelectedStationChanged { get; }
        public ICommand StationNameInputChanged { get; }

        private readonly ITransport _swissTransport;
        private readonly IStationAutoComplete _stationAutoComplete;

        public DepartureBoardViewModel(
            ITransport swissTransport, 
            IStationAutoComplete stationAutoComplete)
        {
            _swissTransport = swissTransport;
            _stationAutoComplete = stationAutoComplete;

            StationSuggestions = new ObservableCollection<Station>();
            StationNameSearchInput = string.Empty;
            DeparturesList = new ObservableCollection<Departure>();
            SearchButtonIsEnabled = false;
            SuggestionsListIsVisible = true;

            ShowDeparturesCommand = new DelegateCommand(OnShowDepartures);
            SelectedStationChanged = new DelegateCommand(OnSelectedStationChanged);
            StationNameInputChanged = new DelegateCommand(OnStationNameInputChanged);
        }

        private void OnStationNameInputChanged()
        {

            if (StationNameSearchInput == SelectedStation?.Name) return;

            StationSuggestions.Clear();

            List<Station>? suggestions = _stationAutoComplete.GetSuggestions(StationNameSearchInput);
            if (suggestions == null) return;

            List<Station> filteredSuggestions = _stationAutoComplete
                .PopulateSuggestions(StationSuggestions.ToList(), ref suggestions).ToList();

            StationSuggestions.AddRange(filteredSuggestions);

            SuggestionsListIsVisible = true;
        }

        private void OnSelectedStationChanged()
        {
            if (SelectedStation != null)
            {
                SearchButtonIsEnabled = true;
                SuggestionsListIsVisible = false;
                StationNameSearchInput = SelectedStation.Name;
            }
        }

        private void OnShowDepartures()
        {
            DeparturesList.Clear();
            if (SelectedStation == null) return;

            List<StationBoard> stationBoards = _swissTransport.GetStationBoard(SelectedStation.Name,
                SelectedStation.Id).Entries.Take(4).ToList();

            foreach (StationBoard stationBoard in stationBoards)
            {
                Connection? connection = _swissTransport.GetConnections(
                    SelectedStation.Name, stationBoard.To, stationBoard.Stop.Departure).ConnectionList
                    .FirstOrDefault();

                if (connection == null) return;

                string trainName = stationBoard.Category + stationBoard.Number;

                Departure connectionDeparture = new Departure(trainName, stationBoard.To, stationBoard.Stop.Departure,
                    connection.From.Platform, connection.From.Delay ?? 0);

                DeparturesList.Add(connectionDeparture);
            }
        }
    }
}
