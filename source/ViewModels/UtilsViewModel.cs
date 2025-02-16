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

    private readonly double _walkSpeed = 2;
    private readonly double _imageSize = 32;
    private readonly Random _random = new();

    // Определяем возможные состояния питомца
    private enum PetState
    {
        Idle,
        Walking,
        PerformingAction
    }

    private PetState _currentState = PetState.Idle;

    public UtilsViewModel(RibbonController ribbonController)
    {
        _ribbonController = ribbonController;
        AsyncEventHandler = new AsyncEventHandler();

        AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
        XOffset = 0;
    }

    public void StartActionLoop()
    {
        if (_currentState != PetState.Idle) return;

        _currentState = PetState.Walking;
        IsWalking = true;
        XOffset = 0;

        _ = PerformActionsAsync();
    }

    public void StopActionForever()
    {
        if (_currentState == PetState.Idle) return;

        _currentState = PetState.Idle;
        IsWalking = false;
        XOffset = 0;
        AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
    }

    private async Task PerformActionsAsync()
    {
        while (_currentState != PetState.Idle)
        {
            try
            {
                await AsyncEventHandler.RaiseAsync(async application =>
                {
                    if (_currentState != PetState.Walking) return;

                    _currentState = PetState.PerformingAction;

                    double direction = _random.Next(0, 2) == 0 ? 1 : -1;
                    AnimatedSource = direction == 1
                        ? "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif"
                        : "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";

                    int frameRate = 30;
                    int totalDuration = 1000;
                    int frameDelay = 1000 / frameRate;
                    int iterations = totalDuration / frameDelay;

                    for (int i = 0; i < iterations; i++)
                    {
                        // Проверяем, не изменилось ли состояние
                        if (_currentState != PetState.PerformingAction) break;

                        XOffset += direction * _walkSpeed;

                        // Логика смены направления при достижении границ
                        double barActualWidth = _ribbonController?.GetBarActualWidth() ?? 0;

                        if (XOffset > barActualWidth - _imageSize)
                        {
                            XOffset = barActualWidth - _imageSize;
                            direction = -1;
                            AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";
                        }
                        else if (XOffset < 0)
                        {
                            XOffset = 0;
                            direction = 1;
                            AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif";
                        }

                        await Task.Delay(frameDelay);
                    }

                    AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
                    _currentState = PetState.Walking;
                });
            }
            catch
            {
                // Обработка исключений при необходимости
            }

            await Task.Delay(2000);
        }
    }
}