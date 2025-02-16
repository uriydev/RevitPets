using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitPets.ViewModels;
using RevitPets.Views.Utils;

namespace RevitPets.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class HideBarCommand : ExternalCommand
{
    public override async void Execute()
    {
        var ribbonController = Host.GetService<RibbonController>();
        if (ribbonController == null) return;
        
        var utilsViewModel = Host.GetService<UtilsViewModel>();
        if (utilsViewModel == null) return;
        
        await Task.Run(() => utilsViewModel.StopActionForever());
        await Task.Delay(500);
        
        ribbonController.RemoveOptionsBar();
    }
}