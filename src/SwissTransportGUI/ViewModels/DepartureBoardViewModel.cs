using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Mvvm;
using SwissTransport.Core;
using SwissTransport.Models;
using SwissTransportGUI.Services;
using SwissTransportGUI.Services.Interfaces;

namespace SwissTransportGUI.ViewModels
{
    internal class DepartureBoardViewModel : BindableBase
    {
        public ObservableCollection<Station> StationSearchResult { get; private set; }
        public ObservableCollection<StationBoard> DeparturesList { get; private set; }

        private string _stationNameInput;
        public string StationNameInput
        {
            get => _stationNameInput;
            set => SetProperty(ref _stationNameInput, value, OnStationNameInputChanged);
        }

        private bool _stationsDropDownIsOpen;
        public bool StationsDropDownIsOpen
        {
            get => _stationsDropDownIsOpen;
            set => SetProperty(ref _stationsDropDownIsOpen, value);
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
            set => SetProperty(ref _selectedStation, value);
        }

        public ICommand ShowDeparturesCommand { get; }
        public ICommand SelectionChangedCommand { get; }

        private readonly ITransport _swissTransport;
        private readonly IStationAutoComplete _stationAutoComplete;

        public DepartureBoardViewModel(ITransport swissTransport, IStationAutoComplete stationAutoComplete)
        {
            _swissTransport = swissTransport;
            _stationAutoComplete = stationAutoComplete;

            StationSearchResult = new ObservableCollection<Station>();
            StationNameInput = string.Empty;
            DeparturesList = new ObservableCollection<StationBoard>();
            SearchButtonIsEnabled = false;
            StationsDropDownIsOpen = false;

            ShowDeparturesCommand = new DelegateCommand(OnShowDepartures);
            SelectionChangedCommand = new DelegateCommand(OnSelectedStationChanged);
        }

        private void OnStationNameInputChanged()
        {
            if (StationNameInput == SelectedStation?.Name) return;

            StationSearchResult.Clear();

            List<Station>? suggestions = _stationAutoComplete.GetSuggestions(StationNameInput);
            if (suggestions == null) return;

            List<Station> filteredSuggestions = _stationAutoComplete
                .PopulateSuggestions(StationSearchResult.ToList(), ref suggestions).ToList();

            StationSearchResult.AddRange(filteredSuggestions);

            StationsDropDownIsOpen = true;
        }

        private void OnSelectedStationChanged()
        {
            if (SelectedStation != null)
            {
                SearchButtonIsEnabled = true;
                StationsDropDownIsOpen = false;
            }
        }

        private void OnShowDepartures()
        {
            DeparturesList.Clear();
            if (SelectedStation == null) return;
            List<StationBoard> stationBoards = _swissTransport.GetStationBoard(SelectedStation.Name, SelectedStation.Id).Entries;
            stationBoards.Take(10).ToList().ForEach(board => DeparturesList.Add(board));
        }
    }
}
