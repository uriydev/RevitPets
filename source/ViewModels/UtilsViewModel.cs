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
    
    
    
    
    
    // public async Task StartWalking()
    // {
    //     double barActualWidth = 0;
    //
    //     if (_ribbonController != null)
    //     {
    //         barActualWidth = _ribbonController.GetBarActualWidth();
    //     }
    //     else
    //     {
    //         barActualWidth = 0; // Значение по умолчанию, если _ribbonController == null
    //     }
    //     
    //     AsyncEventHandler.RaiseAsync(async application =>
    //     {
    //         double direction = _random.Next(0, 2) == 0 ? 1 : -1;
    //     
    //         AnimatedSource = direction == 1 
    //             ? "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif" 
    //             : "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";
    //     
    //         while (IsWalking)
    //         {
    //             XOffset += direction * walkSpeed;
    //             
    //             if (XOffset > barActualWidth)
    //             {
    //                 XOffset = barActualWidth;
    //                 direction = -1;
    //                 AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";
    //             }
    //             else if (XOffset < 0)
    //             {
    //                 XOffset = 0;
    //                 direction = 1;
    //                 AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif";
    //             }
    //
    //             await Task.Delay(10);
    //         }
    //
    //         IsWalking = false;
    //         AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
    //     });
    // }
    public async Task StartWalking()
    {
        // Используем асинхронное событие
        await AsyncEventHandler.RaiseAsync(async application =>
        {
            // Направление анимации
            double direction = _random.Next(0, 2) == 0 ? 1 : -1;

            // Устанавливаем начальную анимацию
            AnimatedSource = direction == 1
                ? "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif"
                : "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";

            // Выполнение анимации при движении
            while (IsWalking)
            {
                // Устанавливаем значение по умолчанию для barActualWidth
                double barActualWidth = _ribbonController?.GetBarActualWidth() ?? 0;
                
                XOffset += direction * walkSpeed;

                if (XOffset > barActualWidth - 32)
                {
                    XOffset = barActualWidth - 32;
                    direction = -1;
                    AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";
                }
                else if (XOffset < 0)
                {
                    XOffset = 0;
                    direction = 1;
                    AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif";
                }

                await Task.Delay(10); // Задержка для анимации
            }

            // Остановка анимации
            IsWalking = false;
            AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
        });
    }

    
    
    
    
    public void RestartWalking()
    {
        if (_ribbonController is null)
        {
            // TaskDialog.Show("Error", "Panel is null!");
            return;
        }
        
        AsyncEventHandler.RaiseAsync(async application =>
        {
            IsWalking = true;
            Task.Run(StartWalking);
        });
    }
}