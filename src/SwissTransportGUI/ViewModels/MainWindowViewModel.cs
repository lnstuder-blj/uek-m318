using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SwissTransport.Core;
using SwissTransportGUI.Views;

namespace SwissTransportGUI.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly IDialogService _dialogService;

        public MainWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            CheckInternetConnection();
        }

        private void CheckInternetConnection()
        {
            string host = "http://transport.opendata.ch/v1/";

            try
            {
                HttpClient webClient = new HttpClient();
                HttpResponseMessage response = webClient.Send(
                    new HttpRequestMessage(HttpMethod.Get, host));

                if (!response.IsSuccessStatusCode)
                    NoConnectionError();
            }
            catch (Exception exception)
            {
                NoConnectionError(exception.Message);
            }
        }

        private void NoConnectionError(string additionalErrorMessage = "")
        {
            _dialogService.ShowDialog("ApplicationDialog", new DialogParameters
            {
                { "message", "You are either not connected to the internet, " +
                             "or the SwissTransport service is currently unavailable.\n" +
                             $"({additionalErrorMessage})"
                }
            }, _ => Environment.Exit(1));
        }
    }
}
