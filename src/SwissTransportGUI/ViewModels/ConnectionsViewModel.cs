using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SwissTransport.Core;
using SwissTransport.Models;
using SwissTransportGUI.Services.Interfaces;

namespace SwissTransportGUI.ViewModels
{
    internal class ConnectionsViewModel : BindableBase
    {
        private string _departureStationNameSearchInput = string.Empty;
        public string DepartureStationNameSearchInput
        {
            get => _departureStationNameSearchInput;
            set => SetProperty(ref _departureStationNameSearchInput, value, 
                OnDepartureStationNameInputChanged);
        }

        private string _arrivalStationNameSearchInput = string.Empty;
        public string ArrivalStationNameSearchInput
        {
            get => _arrivalStationNameSearchInput;
            set => SetProperty(ref _arrivalStationNameSearchInput, value, 
                OnArrivalStationNameInputChanged);
        }

        private bool _departureSuggestionsListIsVisible;
        public bool DepartureSuggestionsListIsVisible
        {
            get => _departureSuggestionsListIsVisible;
            set => SetProperty(ref _departureSuggestionsListIsVisible, value);
        }

        private bool _arrivalSuggestionsListIsVisible;
        public bool ArrivalSuggestionsListIsVisible
        {
            get => _arrivalSuggestionsListIsVisible;
            set => SetProperty(ref _arrivalSuggestionsListIsVisible, value);
        }

        private Station? _selectedDepartureStation;
        public Station? SelectedDepartureStation
        {
            get => _selectedDepartureStation;
            set => SetProperty(ref _selectedDepartureStation, 
                value, OnSelectedDepartureStationChanged);
        }

        private Station? _selectedArrivalStation;
        public Station? SelectedArrivalStation
        {
            get => _selectedArrivalStation;
            set => SetProperty(ref _selectedArrivalStation, 
                value, OnSelectedArrivalStationChanged);
        }

        private bool _applyDateFilter;
        public bool ApplyDateFilter
        {
            get => _applyDateFilter;
            set => SetProperty(ref _applyDateFilter, value);
        }

        private DateTime _selectedDepartureDate = DateTime.Now;
        public DateTime SelectedDepartureDate
        {
            get => _selectedDepartureDate;
            set => SetProperty(ref _selectedDepartureDate, value);
        }
        
        public ObservableCollection<Connection> ConnectionsList { get; }
        public ObservableCollection<Station> DepartureStationSuggestions { get; }
        public ObservableCollection<Station> ArrivalStationSuggestions { get; }

        public ICommand SearchConnectionsCommand { get; }

        private readonly ITransport _swissTransport;
        private readonly IStationAutoComplete _stationAutoComplete;

        public ConnectionsViewModel(
            ITransport swissTransport, 
            IStationAutoComplete stationAutoComplete)
        {
            _swissTransport = swissTransport;
            _stationAutoComplete = stationAutoComplete;

            SearchConnectionsCommand = new DelegateCommand(OnSearchConnections);

            ConnectionsList = new ObservableCollection<Connection>();
            DepartureStationSuggestions = new ObservableCollection<Station>();
            ArrivalStationSuggestions = new ObservableCollection<Station>();
        }

        private void OnDepartureStationNameInputChanged()
        {
            if (DepartureStationNameSearchInput == SelectedDepartureStation?.Name) return;

            DepartureStationSuggestions.Clear();

            List<Station>? suggestions = _stationAutoComplete.GetSuggestions(
                DepartureStationNameSearchInput);
            if (suggestions == null) return;

            List<Station> filteredSuggestions = _stationAutoComplete
                .PopulateSuggestions(DepartureStationSuggestions.ToList(), ref suggestions)
                .ToList();

            DepartureStationSuggestions.AddRange(filteredSuggestions);

            DepartureSuggestionsListIsVisible = true;
        }

        private void OnSelectedDepartureStationChanged()
        {
            if (SelectedDepartureStation != null)
            {
                DepartureSuggestionsListIsVisible = false;
                DepartureStationNameSearchInput = SelectedDepartureStation.Name;
            }
        }

        private void OnSelectedArrivalStationChanged()
        {
            if (SelectedArrivalStation != null)
            {
                ArrivalSuggestionsListIsVisible = false;
                ArrivalStationNameSearchInput = SelectedArrivalStation.Name;
            }
        }

        private void OnArrivalStationNameInputChanged()
        {
            if (ArrivalStationNameSearchInput == SelectedArrivalStation?.Name) return;

            ArrivalStationSuggestions.Clear();

            List<Station>? suggestions = _stationAutoComplete.GetSuggestions(
                ArrivalStationNameSearchInput);
            if (suggestions == null) return;

            List<Station> filteredSuggestions = _stationAutoComplete
                .PopulateSuggestions(ArrivalStationSuggestions.ToList(), ref suggestions)
                .ToList();

            ArrivalStationSuggestions.AddRange(filteredSuggestions);

            ArrivalSuggestionsListIsVisible = true;
        }

        private void OnSearchConnections()
        {
            ConnectionsList.Clear();

            if (SelectedArrivalStation == null 
                || SelectedDepartureStation == null) return;

            DateTime departureDate = ApplyDateFilter
                ? SelectedDepartureDate
                : DateTime.Now;

            List<Connection> connections = _swissTransport
                .GetConnections(
                    SelectedDepartureStation.Name,
                    SelectedArrivalStation.Name, 
                    departureDate).ConnectionList
                .Take(4)
                .ToList();

            ConnectionsList.AddRange(connections);
        }
    }
}
