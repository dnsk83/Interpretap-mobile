﻿using Interpretap.Core;
using Interpretap.Interfaces;
using Interpretap.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Interpretap.Common.Constants;
using Interpretap.Common;

namespace Interpretap
{
    public partial class App : Application
    {
        public static bool ToUpdateLogsFlag { get; set; }
        public static bool ToUpdateQueueFlag { get; set; }
        public static INotificationPayloadService NotificationPayloadService { get; set; }
        public static ActiveCallModel ActiveCall { get; set; }
        public static UserModel User { get; set; }
        public static MessagingCenterListenter MessagingCenterListenter { get; set; }

        public App()
        {
            InitializeComponent();
            InitActiveCall();
            InitUser();
            InitMessagingCenterListener();
            MainPage = new NavigationPage(new Interpretap.Views.LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            //CancelActiveCallRequest(); // TODO: manage so that call to be cancelled if the app really being closed
        }

        protected override void OnResume()
        {
            MessagingCenter.Send(this, Constants.Strings.AppResumed);
        }

        public static void OnUnhandledException()
        {
            ActiveCall.OnAppCrash();
        }

        public static void ReportException(Exception ex)
        {
            var message = ex.Message ?? string.Empty;
            var stackTrace = ex.StackTrace ?? string.Empty;

            string innerMessage = string.Empty;
            string innerStackTrace = string.Empty;

            if (ex.InnerException != null)
            {
                innerMessage = ex.InnerException.Message ?? string.Empty;
                innerStackTrace = ex.InnerException.StackTrace ?? string.Empty;
            }

            var request = new Models.ExceptionDataModel()
            {
                Message = message,
                StackTrace = stackTrace,
                InnerMessage = innerMessage,
                InnerStackTrace = innerStackTrace,
            };

            var syncService = new SyncService();
            syncService.Post(Interpretap.Common.ConfigApp.NotifyCrashAPI, request);
        }

        static void OnNotificationPayloadReceived(object sender, Services.Misc.PayloadReceivedEventArgs e)
        {
            var msg = e.Payload;

            if (msg.EventType == "TIMER")
            {
                if (msg.Event == "CALL_ASSIGN")
                {
                    OpenClientTimerPage();
                }
            }
            if (msg.EventType == "CALL")
            {
                if (msg.Event == "OPEN_CALL")
                {
                    ActivateQueueTab();
                }
            }
        }

        public static void InitNotificationService()
        {
            NotificationPayloadService = new PushNotificationPayloadService();
            NotificationPayloadService.PayloadReceived += OnNotificationPayloadReceived;
        }

        private static void InitActiveCall()
        {
            ActiveCall = new ActiveCallModel();
        }

        private void InitUser()
        {
            User = new UserModel();
        }

        private void InitMessagingCenterListener()
        {
            MessagingCenterListenter = new MessagingCenterListenter();
        }


        public static void ActivateLogsTab()
        {
            ActivateMainPageTab(1);
        }

        private static void OpenClientTimerPage()
        {
            if (User.UserType != UserTypes.Client) return;
            Device.BeginInvokeOnMainThread(() =>
            {
                Current.MainPage.Navigation.PushAsync(new Views.UserViews.TimerPage());
            });
        }

        private static void ActivateQueueTab()
        {
            if (User.UserType != UserTypes.Interpreter) return;
            ActivateMainPageTab(0);
        }

        private static void ActivateMainPageTab(int tabIndex)
        {
            var mainNavPage = Current.MainPage as NavigationPage;
            if (mainNavPage == null) return;
            var mainTabbedPage = mainNavPage.CurrentPage as TabbedPage;
            if (mainTabbedPage == null) return;
            if (mainTabbedPage.Children.Count < tabIndex + 1) return;
            Device.BeginInvokeOnMainThread(() => { mainTabbedPage.CurrentPage = mainTabbedPage.Children[tabIndex]; });
        }

        public static async Task LogoutAsync()
        {
            var lm = new LoginManager();
            await lm.LogoutAsync();
        }
    }
}
