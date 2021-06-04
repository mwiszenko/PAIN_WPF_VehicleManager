using System;
using System.ComponentModel;

namespace Vehicles
{
    public class Vehicle : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Vehicle(string model, Types type, double maxVelocity, DateTime productionDate)
        {
            Model = model;
            Type = type;
            MaxVelocity = maxVelocity;
            ProductionDate = productionDate;
        }

        private string model;
        public string Model
        {
            get { return model; }
            set { model = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Model")); }
        }

        public enum Types { Motorcycle = 0, Car = 1, Truck = 2 };


        private Types type;
        public Types Type
        {
            get { return type; }
            set { type = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Type")); }
        }

        private double maxVelocity;
        public double MaxVelocity
        {
            get { return maxVelocity; }
            set { maxVelocity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxVelocity")); }
        }

        private DateTime productionDate;
        public DateTime ProductionDate
        {
            get { return productionDate; }
            set { productionDate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductionDate")); }
        }
    }
}