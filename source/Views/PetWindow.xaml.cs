using System.Windows;
using RevitPets.ViewModels;

namespace RevitPets.Views;

public partial class PetWindow : Window
{
    public PetWindow(PetWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}