using System.Windows;
using Autodesk.Revit.UI;
using RevitPets.ViewModels;

namespace RevitPets.Views;

public partial class UtilsView
{
    public UtilsView(UtilsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}