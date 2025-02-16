using Nice3point.Revit.Toolkit.External.Handlers;
using RevitPets.Views.Utils;

namespace RevitPets.ViewModels;

public partial class UtilsViewModel : ObservableObject
{
    private const int ActionLoopDelay = 2000;
    private const int FrameRate = 30;
    private const int TotalDuration = 1000;
    
    private readonly RibbonController _ribbonController;
    public AsyncEventHandler AsyncEventHandler { get; }

    [ObservableProperty]
    private bool _isWalking;

    [ObservableProperty]
    private string _animatedSource;

    [ObservableProperty]
    private double _xOffset;

    private readonly double _walkSpeed = 2;
    private readonly double _imageSize = 32;
    private readonly Random _random = new();
    private readonly string _idleAnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
    private readonly string _runRightAnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif";
    private readonly string _runLeftAnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";

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

        AnimatedSource = _idleAnimatedSource;
        XOffset = 0;
    }

    public void StartActionLoop()
    {
        if (_currentState != PetState.Idle) return;
        
        XOffset = Math.Max(XOffset, 0);
        Console.WriteLine(XOffset);
        
        _currentState = PetState.Walking;
        IsWalking = true;

        _ = PerformActionsAsync();
    }

    public void StopActionForever()
    {
        if (_currentState == PetState.Idle) return;
        
        _currentState = PetState.Idle;
        IsWalking = false;
        
        AnimatedSource = _idleAnimatedSource;
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
                    AnimatedSource = direction == 1 ? _runRightAnimatedSource : _runLeftAnimatedSource;
                    
                    int frameDelay = 1000 / FrameRate;
                    int iterations = TotalDuration / frameDelay;

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
                            AnimatedSource = _runLeftAnimatedSource;
                        }
                        else if (XOffset < 0)
                        {
                            XOffset = 0;
                            direction = 1;
                            AnimatedSource = _runRightAnimatedSource;
                        }

                        await Task.Delay(frameDelay);
                    }

                    AnimatedSource = _idleAnimatedSource;
                    _currentState = PetState.Walking;
                });
            }
            catch
            {
                // Обработка исключений при необходимости
            }

            await Task.Delay(ActionLoopDelay);
        }
    }
}