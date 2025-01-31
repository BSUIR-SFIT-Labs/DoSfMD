﻿using Xamarin.Forms;
using XamarinApp.Views;

namespace XamarinApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(AddComputerPage), typeof(AddComputerPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ComputerDetailsPage), typeof(ComputerDetailsPage));
            Routing.RegisterRoute(nameof(GoogleMapPage), typeof(GoogleMapPage));

            MessagingCenter.Subscribe<SettingsPage>(this, "LanguageChanged",
                (sender) =>
                {
                    Application.Current.MainPage = new AppShell();
                });
            MessagingCenter.Subscribe<SettingsPage>(this, "FontFamilyChanged",
                (sender) =>
                {
                    Application.Current.MainPage = new AppShell();
                });
            MessagingCenter.Subscribe<SettingsPage>(this, "FontSizeChanged",
                (sender) =>
                {
                    Application.Current.MainPage = new AppShell();
                });
        }
    }
}