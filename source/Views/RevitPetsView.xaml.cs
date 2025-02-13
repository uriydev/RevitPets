using RevitPets.ViewModels;

namespace RevitPets.Views;

public sealed partial class RevitPetsView
{
    public RevitPetsView(RevitPetsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}