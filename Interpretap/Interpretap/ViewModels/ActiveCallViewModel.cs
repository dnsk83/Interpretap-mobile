﻿using Interpretap.Models;
using Interpretap.Services;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Interpretap.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ActiveCallViewModel
    {
        public bool IsVisible { get; set; }
        public bool IsActivityIndicatorVisible { get; set; }

        public string CallId { get; set; }

        public ICommand CancelCallCommand { get; set; }

        public ActiveCallViewModel()
        {
            CancelCallCommand = new Command(async () => await ExecuteCancelCallAsync());
        }

        private async Task ExecuteCancelCallAsync()
        {
            var service = new ClientService();
            var request = new CancelCallRequestModel();
            request.CallId = CallId;
            var responce = await service.CancelCallRequest(request);
            if (responce.Status == true)
            {
                IsVisible = false;
            }
        }

        async Task GetRequestedCallAsync()
        {
            try
            {
                IsVisible = true;
                IsActivityIndicatorVisible = true;

                var service = new ClientService();
                var request = new BaseModel();
                var responce = await service.FetchOpenCallRequest(request);

                IsVisible = responce.CallId != "0";
                CallId = responce.CallId;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        public void OnAppearing()
        {
            GetRequestedCallAsync();
        }
    }
}