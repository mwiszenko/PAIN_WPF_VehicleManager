namespace Vehicles.MVVM
{
    public interface IWindowService
    {
        void Show(IViewModel viewModel);
        void ShowDialog(IViewModel viewModel);
    }
}
