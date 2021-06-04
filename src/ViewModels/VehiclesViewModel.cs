using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Vehicles.MVVM;

namespace Vehicles.ViewModels
{
    public class VehiclesViewModel : IViewModel, INotifyPropertyChanged
    {
        private VehiclesModel VehiclesModel { get; }
        public CollectionViewSource collectionViewSource;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICollectionViewLiveShaping Vehicles { get; }

        private Vehicle selectedVehicle;
        
        public Vehicle SelectedVehicle
        {
            get { return selectedVehicle; }
            set
            {
                selectedVehicle = value;
                EditCommand.NotifyCanExecuteChanged();
                DeleteCommand.NotifyCanExecuteChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedVehicle)));
            }
        }

        private int filterIndex = 0;
        public int FilterIndex
        {
            get
            {
                return filterIndex;
            }
            set 
            {
                filterIndex = value;
                UpdateFilter();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterIndex)));
            }
        }
        private void UpdateFilter()
        {
            collectionViewSource.View.Refresh();
        }
        bool FilterVehicle(Vehicle vehicle)
        {
            if(FilterIndex == 1)
            {
                return vehicle.MaxVelocity >= 100;
            } else if(FilterIndex == 2)
            {
                return vehicle.MaxVelocity < 100;
            } else
            {
                return true;
            }
        }
          
        public Action Close { get; set; }
        private RelayCommand<object> addCommand;
        public RelayCommand<object> AddCommand => addCommand = addCommand ?? new RelayCommand<object>(o=>AddVehicle());
        private RelayCommand<object> editCommand;
        public RelayCommand<object> EditCommand => editCommand = editCommand ?? new RelayCommand<object>(o => EditVehicle(SelectedVehicle), o => SelectedVehicle != null);
        private RelayCommand<object> deleteCommand;
        public RelayCommand<object> DeleteCommand => deleteCommand = deleteCommand ?? new RelayCommand<object>(o => DeleteVehicle(SelectedVehicle), o => SelectedVehicle != null);

        private RelayCommand<object> newViewCommand;
        public RelayCommand<object> NewViewCommand => newViewCommand = newViewCommand ?? new RelayCommand<object>(o => NewView());


        public VehiclesViewModel(VehiclesModel vehiclesModel)
        {
            VehiclesModel = vehiclesModel;
            collectionViewSource = new CollectionViewSource
            {
                Source = VehiclesModel.Vehicles
            };
            collectionViewSource.View.Filter = (o) => FilterVehicle((Vehicle)o);
            Vehicles = collectionViewSource.View as ICollectionViewLiveShaping;

            if (Vehicles.CanChangeLiveFiltering)
            {
                Vehicles.LiveFilteringProperties.Add("MaxVelocity");
                Vehicles.IsLiveFiltering = true;
            }
        }

        public void NewView()
        {
            VehiclesViewModel vehiclesViewModel = new VehiclesViewModel(VehiclesModel);
            ((App)Application.Current).WindowService.Show(vehiclesViewModel);
        }

        public void AddVehicle()
        {
            VehicleViewModel vehicleViewModel = new VehicleViewModel(VehiclesModel, null);
            ((App)Application.Current).WindowService.ShowDialog(vehicleViewModel);
        }

        public void EditVehicle(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                VehicleViewModel vehicleViewModel = new VehicleViewModel(VehiclesModel, vehicle);
                ((App)Application.Current).WindowService.ShowDialog(vehicleViewModel);
                UpdateFilter();
            }
        }
        public void DeleteVehicle(Vehicle vehicle)
        {
            if( vehicle != null )
                VehiclesModel.Vehicles.Remove(vehicle);
        }
    }
}
