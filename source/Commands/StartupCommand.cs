using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using Nice3point.Revit.Toolkit.External.Handlers;
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
    public AsyncEventHandler AsyncEventHandler { get; set; }

    public override void Execute()
    {
        AsyncEventHandler = new AsyncEventHandler();
        
        var bar = Host.GetService<RibbonController>();
        var view = Host.GetService<UtilsView>();
        bar.ShowOptionsBar(view);
        
        AsyncEventHandler.RaiseAsync(async application =>
        {
            await Task.Delay(5000);

            var viewModel = Host.GetService<UtilsViewModel>();
            viewModel?.RestartWalking();
        });
    }
}