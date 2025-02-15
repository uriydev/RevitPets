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
    public override void Execute()
    {
        // var viewModel = new UtilsViewModel();
        // var view = new UtilsView(viewModel);
        // RibbonController.ShowOptionsBar(view);

        // var view = Host.GetService<PetWindow>();
        // view.Show();

        var view = Host.GetService<UtilsView>();
        var bar = Host.GetService<RibbonController>();
        bar.ShowOptionsBar(view);
    }
}