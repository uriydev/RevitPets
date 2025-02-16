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
    public override void Execute()
    {
        var ribbonController = Host.GetService<RibbonController>();

        var utilsViewModel = Host.GetService<UtilsViewModel>();
        utilsViewModel.StopActionForever();
        
        ribbonController.RemoveOptionsBar();
    }
}