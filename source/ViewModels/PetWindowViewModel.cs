using RevitPets.Views;
using RevitPets.Views.Utils;

namespace RevitPets.ViewModels;

public partial class PetWindowViewModel : ObservableObject
{
    private RibbonController _ribbonController;
    private UtilsViewModel _utilsViewModel;
    
    public PetWindowViewModel(RibbonController ribbonController, UtilsViewModel utilsViewModel)
    {
        _ribbonController = ribbonController;
        _utilsViewModel = utilsViewModel;
    }
    
    [RelayCommand]
    private void RemoveOptionsBar()
    {
        // _ribbonController.RemoveOptionsBar();
    }

    [RelayCommand]
    private void ShowBottomOptionsBar()
    {
        _ribbonController.ShowOptionsBar(Host.GetService<UtilsView>());
    }
    
    [RelayCommand]
    private void ToggleWalkingMain()
    {
        _utilsViewModel.ToggleWalking();
    }
}