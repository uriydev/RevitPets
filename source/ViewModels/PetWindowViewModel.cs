using RevitPets.Views;
using RevitPets.Views.Utils;

namespace RevitPets.ViewModels;

public partial class PetWindowViewModel : ObservableObject
{
    private RibbonController _ribbonController;
    
    public PetWindowViewModel(RibbonController ribbonController)
    {
        _ribbonController = ribbonController;
    }
    
    [RelayCommand]
    private void RemoveOptionsBar()
    {
        _ribbonController.RemoveOptionsBar();
    }

    [RelayCommand]
    private void ShowBottomOptionsBar()
    {
        _ribbonController.ShowOptionsBar(Host.GetService<UtilsView>());
    }
}