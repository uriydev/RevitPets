using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitPets.ViewModels;
using RevitPets.Views;

namespace RevitPets.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override void Execute()
    {
        var viewModel = new RevitPetsViewModel();
        var view = new RevitPetsView(viewModel);
        view.Show(UiApplication.MainWindowHandle);
    }
}