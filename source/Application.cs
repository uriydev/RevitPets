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
        CreateRibbon();
    }

    private void CreateRibbon()
    {
        var panel = Application.CreatePanel("Commands", "RevitPets");

        panel.AddPushButton<StartupCommand>("Execute")
            .SetImage("/RevitPets;component/Resources/Icons/RibbonIcon16.png")
            .SetLargeImage("/RevitPets;component/Resources/Icons/RibbonIcon32.png");
    }
}