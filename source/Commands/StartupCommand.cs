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
        
        var view = Host.GetService<UtilsView>();
        var bar = Host.GetService<RibbonController>();
        bar.ShowOptionsBar(view);
        
        var vm = Host.GetService<UtilsViewModel>();
        // vm.RestartWalking();
        
        //
        // Вызов через ActionEventHandler для выполнения в другом контексте
        AsyncEventHandler.RaiseAsync(async application =>
        {
            // await vm.RestartWalking(); // Вызов асинхронного метода (плавно)
            
            // vm.RestartWalking(); // Вызов асинхронного метода (плавно)
        });
    }
}