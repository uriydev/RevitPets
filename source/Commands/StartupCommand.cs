using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitPets.ViewModels;
using RevitPets.Views;
using RevitPets.Views.Utils;

namespace RevitPets.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class StartupCommand : ExternalCommand
{
    public override async void Execute()
    {
        var ribbonController = Host.GetService<RibbonController>();
        var utilsViewModel = Host.GetService<UtilsViewModel>();

        if (ribbonController == null || utilsViewModel == null) return;

        await Task.Run(() => utilsViewModel.StopActionForever());
        await Task.Delay(500);
        await Task.Run(() => utilsViewModel.StartActionLoop());

        ribbonController.ShowOptionsBar(Host.GetService<UtilsView>());
    }
}