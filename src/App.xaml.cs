using System.Windows;

namespace Vehicles
{
    public partial class App : Application
    {
        public MVVM.IWindowService WindowService { get; } = new MVVM.WindowService();
        private VehiclesModel VehiclesModel { get; } = new VehiclesModel();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ViewModels.VehiclesViewModel vehiclesViewModel = new ViewModels.VehiclesViewModel(VehiclesModel);
            WindowService.Show(vehiclesViewModel);
        }
    }
}
