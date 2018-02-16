﻿using Interpretap.Interfaces.ViewModels;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Interpretap.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class EmployeesListBaseViewModel
    {
        public ObservableCollection<IEmployeeListItemViewModel> Employees { get; set; }

        public ICommand RefreshCommand { get; set; }
        public ICommand AddCommand { get; set; }

        public bool IsRefreshing { get; set; }

        public EmployeesListBaseViewModel()
        {
            Employees = new ObservableCollection<IEmployeeListItemViewModel>();
            RefreshCommand = new Command(async () => await ExecuteRefreshCommandAsync());
            AddCommand = new Command(async () => await ExecuteAddCommandAsync());
        }

        protected virtual async Task ExecuteAddCommandAsync()
        {
        }

        private async Task ExecuteRefreshCommandAsync()
        {
            IsRefreshing = true;

            Employees.Clear();
            await LoadDataAsync();

            IsRefreshing = false;
        }

        protected virtual async Task LoadDataAsync()
        {
        }

        public void OnAppearing()
        {
            if (Employees.Count == 0)
            {
                RefreshCommand.Execute(null);
            }
        }

        public virtual void OnItemSelected(IEmployeeListItemViewModel selectedItem)
        {
        }
    }
}
