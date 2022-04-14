using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Windows.Media;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SwissTransport.Core;
using SwissTransport.Models;
using SwissTransportGUI.Models;

namespace SwissTransportGUI.ViewModels
{
    internal class NearbyStationsViewModel : BindableBase
    {
        public ObservableCollection<Station> NearbyStations { get; }

        private readonly ITransport _swissTransport;
        private readonly IDialogService _dialogService;
        private readonly HttpClient _httpClient;

        private const string LocationInfoUrl = "http://ip-api.com/json";
        private const string IpInfoUrl = "https://icanhazip.com";

        public NearbyStationsViewModel(
            ITransport swissTransport, 
            IDialogService dialogService)
        {
            NearbyStations = new ObservableCollection<Station>();

            _httpClient = new HttpClient();
            _swissTransport = swissTransport;
            _dialogService = dialogService;

            GetNearbyStations();
        }

        private string GetPublicIp()
        {
            HttpResponseMessage ipInfoResponse = _httpClient.Send(
                new HttpRequestMessage(HttpMethod.Get, IpInfoUrl));

            return ipInfoResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        private LocationDetails? GetLocationDetails(string publicIp)
        {
            HttpResponseMessage locationInfoResponse = _httpClient.Send(
                new HttpRequestMessage(HttpMethod.Get, $"{LocationInfoUrl}/{publicIp}"));

            string locationInfoResponseContent =
                locationInfoResponse.Content
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

            return JsonConvert
                .DeserializeObject<LocationDetails>(locationInfoResponseContent);
        }

        private void GetNearbyStations()
        {
            try
            {
                string myPublicIp = GetPublicIp();
                LocationDetails? locationDetails = GetLocationDetails(myPublicIp);
                    
                if (locationDetails == null)
                {
                    ShowGetLocationFailedDialog();
                    return;
                }

                PopulateNearbyStationsList(locationDetails);
            }
            catch
            {
                ShowGetLocationFailedDialog();
            }
        }

        private void PopulateNearbyStationsList(LocationDetails locationDetails)
        {
            List<Station> foundNearbyStations = _swissTransport
                .GetStations(locationDetails.Longitude, locationDetails.Latitude).StationList
                .Take(4)
                .ToList();

            NearbyStations.AddRange(foundNearbyStations);
        }

        private void ShowGetLocationFailedDialog(string additionalInformation = "")
        {
            _dialogService.ShowDialog("ApplicationDialog", new DialogParameters
            {
                {
                    "message", "Could not get your geolocation.\n" +
                                $"({additionalInformation})"
                }
            }, _ => { });
        }
    }
}
