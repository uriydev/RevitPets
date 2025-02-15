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

    private double imageSize = 32;
    
    private Timer _walkTimer;
    
    public UtilsViewModel(RibbonController ribbonController)
    {
        Console.WriteLine($"CREATE VM");
        
        _ribbonController = ribbonController;
        AsyncEventHandler = new AsyncEventHandler();
        
        AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
        XOffset = 0;
    }
    
    public async Task StartWalking()
    {
        await AsyncEventHandler.RaiseAsync(async application =>
        {
            // Направление анимации
            double direction = _random.Next(0, 2) == 0 ? 1 : -1;

            // Устанавливаем начальную анимацию
            AnimatedSource = direction == 1
                ? "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif"
                : "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";

            // Запускаем асинхронное движение
            await MoveCatAsync(direction);
        });
    }



    private async Task MoveCatAsync(double direction)
    {
        // Ограничиваем количество действий, которые могут выполняться одновременно
        const int maxActions = 10;
        var actionQueue = new Queue<Task>();

        while (IsWalking)
        {
            Console.WriteLine("Walking");

            // Устанавливаем значение по умолчанию для barActualWidth
            double barActualWidth = _ribbonController?.GetBarActualWidth() ?? 0;

            // Случайная пауза для остановки кота
            Task standingTask = null;
            if (_random.Next(0, 1000) < 5) // 10% вероятность остановки
            {
                AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
                standingTask = Task.Delay(_random.Next(1000, 3000)); // Стоим от 1 до 3 секунд
            }

            // Обновляем позицию кота только если он не стоит
            if (standingTask == null)
            {
                XOffset += direction * walkSpeed;
            }

            // Добавляем действие в очередь (движение или пауза)
            var moveTask = Task.Delay(50); // Задержка перед следующим циклом
            actionQueue.Enqueue(moveTask);

            // Если очередь переполнена, ждём завершения первого действия
            while (actionQueue.Count >= maxActions)
            {
                var completedTask = await Task.WhenAny(actionQueue);
                actionQueue.Dequeue(); // Убираем завершённое действие
            }

            // Ожидаем завершения текущего действия (движение или стояние)
            if (standingTask != null)
            {
                actionQueue.Enqueue(standingTask); // Добавляем паузу для стояния в очередь
                await standingTask; // Ожидаем завершения паузы
                AnimatedSource = direction == 1
                    ? "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif"
                    : "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";
            }

            // Проверяем границы и меняем направление, если нужно
            if (XOffset > barActualWidth - imageSize)
            {
                XOffset = barActualWidth - imageSize;
                direction = -1;
                AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunLeft.gif";
            }
            else if (XOffset < 0)
            {
                XOffset = 0;
                direction = 1;
                AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif";
            }

            // Делаем задержку для контроля частоты выполнения
            await moveTask; // Ожидаем завершения движения (задержка между обновлениями)
        }

        // Остановка анимации
        IsWalking = false;
        AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
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