using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitPets.Views.Utils;

namespace RevitPets.ViewModels;

public partial class UtilsViewModel : ObservableObject
{
    private readonly RibbonController _ribbonController;
    
    public AsyncEventHandler AsyncEventHandler { get; }
    
    [ObservableProperty]
    private bool isWalking;
    
    [ObservableProperty]
    private string animatedSource;
    
    [ObservableProperty]
    private double xOffset;
    
    private readonly double walkSpeed = 1;
    private Random _random = new Random();
    
    public UtilsViewModel(RibbonController ribbonController)
    {
        _ribbonController = ribbonController;
        AsyncEventHandler = new AsyncEventHandler();
        
        AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
        XOffset = 0;
    }
    
    // public void StartWalking()
    public async Task StartWalking()
    {
        AsyncEventHandler.RaiseAsync(async application =>
        {
            // var stopTime = DateTime.Now.AddSeconds(5);
            double direction = _random.Next(0, 2) == 0 ? 1 : -1; // Randomly choose left (1) or right (-1)
        
            // Set initial animation based on direction
            AnimatedSource = direction == 1 
                ? "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif" 
                : "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";
        
            // while (IsWalking && DateTime.Now < stopTime)
            while (IsWalking)
            {
                XOffset += direction * walkSpeed;
    
                // If the cat goes out of bounds, reverse direction
                if (XOffset > _ribbonController._panelPresenter.ActualWidth)
                {
                    XOffset = _ribbonController._panelPresenter.ActualWidth; // Keep within bounds
                    direction = -1; // Change direction to left
                    AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";
                }
                else if (XOffset < 0)
                {
                    XOffset = 0; // Keep within bounds
                    direction = 1; // Change direction to right
                    AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif";
                }
    
                await Task.Delay(10);
            }
    
            IsWalking = false;
            AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif"; // Return to idle animation
        });
    }

    //(плавно)
    // public async Task RestartWalking()
    // {
    //     AsyncEventHandler.RaiseAsync(application =>
    //     {
    //         IsWalking = true;
    //         Task.Run(StartWalking);
    //     });
    // }
    
    //(плавно)
    public void RestartWalking()
    {
        if (_ribbonController._panelPresenter is null)
        {
            TaskDialog.Show("Error", "Panel is null!");
            return;
        }
        
        AsyncEventHandler.RaiseAsync(async application =>
        {
            IsWalking = true;
            Task.Run(StartWalking);
        });
        // IsWalking = true;
        // Task.Run(StartWalking);
    }
}