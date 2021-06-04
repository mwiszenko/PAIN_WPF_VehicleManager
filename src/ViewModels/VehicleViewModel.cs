using System;
using Vehicles.MVVM;
using System.ComponentModel;
using static Vehicles.Vehicle;

namespace Vehicles.ViewModels
{
    public class VehicleViewModel : IDataErrorInfo, IViewModel, INotifyPropertyChanged
    {
        private VehiclesModel VehiclesModel { get; }
        private Vehicle Vehicle { get; }
        public Action Close { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private Types type;

        public int TypeIndex
        {
            get
            {
                return (int) this.type;
            }

            set
            {
                Type = (Types) value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypeIndex)));
            }
        }

        public Types Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
            }
        }
        public string Model { get; set; }
        public string MaxVelocity { get; set; }
        public DateTime ProductionDate { get; set; }

        string IDataErrorInfo.Error
        {
            get { throw new NotImplementedException(); }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Model")
                {
                    if(String.IsNullOrEmpty(Model))
                    {
                        result = "Model is required";
                        ModelIsValid = false;
                    }
                    else
                    {
                        ModelIsValid = true;
                    }
                }
                if (columnName == "MaxVelocity")
                {
                    double n;
                    if (!Double.TryParse(MaxVelocity, out n))
                    {
                        result = "Velocity has to be a number";
                        MaxVelocityIsValid = false;
                    }
                    else if (n < 0)
                    {
                        result = "Velocity cannot be negative";
                        MaxVelocityIsValid = false;
                    }
                    else
                    {
                        MaxVelocityIsValid = true;
                    }
                }
                return result;
            }
        }

        private RelayCommand<VehicleViewModel> okCommand;
        public RelayCommand<VehicleViewModel> OkCommand => okCommand = okCommand ?? new RelayCommand<VehicleViewModel>(o => Ok(), o => ModelIsValid && MaxVelocityIsValid);

        public RelayCommand<VehicleViewModel> CancelCommand { get; } = new RelayCommand<VehicleViewModel>
            (
                (vehicleViewModel) => { vehicleViewModel.Cancel(); }
            );

        public RelayCommand<VehicleViewModel> ChangeTypeCommand { get; } = new RelayCommand<VehicleViewModel>
            (
                (vehicleViewModel) => { vehicleViewModel.ChangeType(); }
            );

        public VehicleViewModel(VehiclesModel vehiclesModel, Vehicle vehicle)
        {
            VehiclesModel = vehiclesModel;
            Vehicle = vehicle;
            if (Vehicle != null)
            {
                Type = Vehicle.Type;
                Model = Vehicle.Model;
                MaxVelocity = Vehicle.MaxVelocity.ToString();
                ProductionDate = Vehicle.ProductionDate;
            } else
            {
                ProductionDate = DateTime.Now;
                Type = Types.Motorcycle;
            }
        }

        private bool modelIsValid;
        private bool ModelIsValid
        {
            get
            {
                return modelIsValid;
            }
            set
            {
                modelIsValid = value;
                OkCommand.NotifyCanExecuteChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModelIsValid)));
            }
        }

        private bool maxVelocityIsValid;
        private bool MaxVelocityIsValid
        {
            get
            {
                return maxVelocityIsValid;
            }
            set
            {
                maxVelocityIsValid = value;
                OkCommand.NotifyCanExecuteChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxVelocityIsValid)));
            }
        }

        public void Ok()
        {
            if (Vehicle == null)
            {
                Vehicle vehicle = new Vehicle(Model, Type, Double.Parse(MaxVelocity), ProductionDate);
                VehiclesModel.Vehicles.Add(vehicle);
            }
            else
            {
                Vehicle.Type = Type;
                Vehicle.Model = Model;
                Vehicle.MaxVelocity = Double.Parse(MaxVelocity);
                Vehicle.ProductionDate = ProductionDate;
            }
            Close?.Invoke();
        }
        public void Cancel() => Close?.Invoke();

        public void ChangeType()
        {
            TypeIndex = (TypeIndex + 1) % Enum.GetNames(typeof(Types)).Length;
        }
    }
}
