using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitPets.ViewModels;

namespace RevitPets.Commands;

/// <summary>
///     External command entry point invoked from the Revit interface
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class RestartWalkCommand : ExternalCommand
{
    public AsyncEventHandler AsyncEventHandler { get; set; }

    public override void Execute()
    {
        AsyncEventHandler = new AsyncEventHandler();
        
        var vm = Host.GetService<UtilsViewModel>();
        
        AsyncEventHandler.RaiseAsync(async application =>
        {
            // vm.RestartWalking();
        });
    }
}