using System;
using System.Collections.ObjectModel;

namespace Vehicles
{
    public class VehiclesModel
    {
        public ObservableCollection<Vehicle> Vehicles { get; private set; } = new ObservableCollection<Vehicle>();
        public VehiclesModel() { }
    }
}
