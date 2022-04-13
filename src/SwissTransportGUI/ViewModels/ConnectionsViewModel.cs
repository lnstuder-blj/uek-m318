using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SwissTransport.Core;
using SwissTransport.Models;
using SwissTransportGUI.Services.Interfaces;

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

        private bool _departureStationsDropDownIsOpen;
        public bool DepartureStationsDropDownIsOpen
        {
            get => _departureStationsDropDownIsOpen;
            set => SetProperty(ref _departureStationsDropDownIsOpen, value);
        }

        private bool _arrivalStationsDropDownIsOpen;
        public bool ArrivalStationsDropDownIsOpen
        {
            get => _arrivalStationsDropDownIsOpen;
            set => SetProperty(ref _arrivalStationsDropDownIsOpen, value);
        }

        private Station? _selectedDepartureStation;
        public Station? SelectedDepartureStation
        {
            get => _selectedDepartureStation;
            set => SetProperty(ref _selectedDepartureStation, value);
        }

        private Station? _selectedArrivalStation;
        public Station? SelectedArrivalStation
        {
            get => _selectedArrivalStation;
            set => SetProperty(ref _selectedArrivalStation, value);
        }

        private bool _applyDateFilter;
        public bool ApplyDateFilter
        {
            get => _applyDateFilter;
            set => SetProperty(ref _applyDateFilter, value);
        }

        private DateTime? _selectedDepartureDate;
        public DateTime? SelectedDepartureDate
        {
            get => _selectedDepartureDate;
            set => SetProperty(ref _selectedDepartureDate, value);
        }
        
        public ObservableCollection<Connection> ConnectionsList { get; }
        public ObservableCollection<Station> DepartureStationSearchResult { get; }
        public ObservableCollection<Station> ArrivalStationSearchResult { get; }

        public ICommand SearchConnectionsCommand { get; }

        private ITransport _swissTransport;
        private IStationAutoComplete _stationAutoComplete;
        private IDialogService _dialogService;

        public ConnectionsViewModel(ITransport swissTransport, IStationAutoComplete stationAutoComplete, IDialogService dialogService)
        {
            _swissTransport = swissTransport;
            _stationAutoComplete = stationAutoComplete;
            _dialogService = dialogService;

            SearchConnectionsCommand = new DelegateCommand(OnSearchConnections);

            ConnectionsList = new ObservableCollection<Connection>();
            DepartureStationSearchResult = new ObservableCollection<Station>();
            ArrivalStationSearchResult = new ObservableCollection<Station>();

            DepartureStationNameInput = string.Empty;
            ArrivalStationNameInput = string.Empty;
        }

        private void OnDepartureStationNameInputChanged()
        {
            if (DepartureStationNameInput == SelectedDepartureStation?.Name) return;

            DepartureStationSearchResult.Clear();

            List<Station>? suggestions = _stationAutoComplete.GetSuggestions(DepartureStationNameInput);
            if (suggestions == null) return;

            List<Station> filteredSuggestions = _stationAutoComplete
                .PopulateSuggestions(DepartureStationSearchResult.ToList(), ref suggestions).ToList();

            DepartureStationSearchResult.AddRange(filteredSuggestions);

            DepartureStationsDropDownIsOpen = true;
        }

        private void OnArrivalStationNameInputChanged()
        {
            if (ArrivalStationNameInput == SelectedArrivalStation?.Name) return;

            ArrivalStationSearchResult.Clear();

            List<Station>? suggestions = _stationAutoComplete.GetSuggestions(ArrivalStationNameInput);
            if (suggestions == null) return;

            List<Station> filteredSuggestions = _stationAutoComplete
                .PopulateSuggestions(ArrivalStationSearchResult.ToList(), ref suggestions).ToList();

            ArrivalStationSearchResult.AddRange(filteredSuggestions);

            ArrivalStationsDropDownIsOpen = true;
        }
        
        private void OnSearchConnections()
        {
            if (SelectedArrivalStation == null 
                || SelectedDepartureStation == null 
                || SelectedDepartureDate == null) return;

            DateTime departureDate = SelectedDepartureDate.HasValue && ApplyDateFilter 
                ? SelectedDepartureDate.Value : DateTime.Now;

            List<Connection> connections = _swissTransport
                .GetConnections(SelectedDepartureStation.Name, SelectedArrivalStation.Name, departureDate).ConnectionList.Take(10)
                .ToList();

            ConnectionsList.AddRange(connections);
        }
    }
}
