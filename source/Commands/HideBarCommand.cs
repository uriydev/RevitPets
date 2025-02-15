using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
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
        var bar = Host.GetService<RibbonController>();
        bar.RemoveOptionsBar();
    }
}