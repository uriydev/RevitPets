using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using RevitPets.Commands;

namespace RevitPets;

/// <summary>
///     Application entry point
/// </summary>
[UsedImplicitly]
public class Application : ExternalApplication
{
    public override void OnStartup()
    {
        Host.Start();
        CreatePanel(Application);
    }
    
    public static void CreatePanel(UIControlledApplication application)
    {
        var addinPanel = application.CreatePanel("Revit Pets");
        var pullButton = addinPanel.AddPullDownButton("RevitPets", "RevitPets");
        pullButton.SetImage("/RevitPets;component/Resources/Icons/RibbonIcon16.png");
        pullButton.SetLargeImage("/RevitPets;component/Resources/Icons/RibbonIcon32.png");
        
        pullButton.AddPushButton<StartupCommand>("Show");
        pullButton.AddPushButton<HideBarCommand>("Hide");
    }
}