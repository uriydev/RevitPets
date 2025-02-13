using RevitPets.Views.Utils;

namespace RevitPets.ViewModels;

public partial class UtilsViewModel : ObservableObject
{
    [RelayCommand]
    private void RemoveOptionsBar()
    {
        RibbonController.RemoveOptionsBar();
    }
}