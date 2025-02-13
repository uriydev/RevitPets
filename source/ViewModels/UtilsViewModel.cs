using System.Windows.Threading;
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitPets.Views.Utils;

namespace RevitPets.ViewModels
{
    public partial class UtilsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isWalking;

        [ObservableProperty]
        private string animatedSource;

        [ObservableProperty]
        private double xOffset;  // Смещение по X

        private double walkSpeed = 1;  // Скорость движения
        private DispatcherTimer walkTimer;

        public UtilsViewModel()
        {
            AsyncEventHandler = new AsyncEventHandler();
            
            AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Anim399.gif";  // Стоять

            // Инициализация таймера для плавного обновления
            walkTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1) // Обновление каждую 10-ую миллисекунду
            };
            walkTimer.Tick += OnWalkTimerTick;
        }
        
        public AsyncEventHandler AsyncEventHandler { get; }

        // Команда для переключения состояния (используем атрибут [RelayCommand])
        [RelayCommand]
        private void ToggleWalking()
        {
            IsWalking = !IsWalking;

            // Переключение анимации
            AnimatedSource = IsWalking
                ? "pack://application:,,,/RevitPets;component/Resources/Animations/Anim193x200.gif"  // Бег
                : "pack://application:,,,/RevitPets;component/Resources/Animations/Anim399.gif";      // Стоять

            // Запуск или остановка движения
            if (IsWalking)
                StartWalking();
            else
                StopWalking();
        }

        // private void StartWalking()
        // {
        //     // Запуск таймера для анимации смещения
        //     walkTimer.Start();
        // }
        
        // Метод для запуска движения
        public async Task StartWalking()
        {
            IsWalking = true; // Начинаем движение

            // Запускаем таймер на 5 секунд
            var stopTime = DateTime.Now.AddSeconds(5);

            while (IsWalking && DateTime.Now < stopTime)
            {
                // Двигаем кота вправо
                XOffset += walkSpeed;

                // Если кот выходит за пределы экрана, сбрасываем его позицию
                if (XOffset > 300)
                    XOffset = 0;

                // Плавная задержка для анимации
                await Task.Delay(10); // ~100 FPS
            }

            IsWalking = false; // Останавливаем движение
        }

        private void StopWalking()
        {
            // Останавливаем таймер и сбрасываем позицию
            walkTimer.Stop();
            XOffset = 0;
        }

        // Метод для обновления смещения
        private void OnWalkTimerTick(object sender, EventArgs e)
        {
            // Двигаем кота вправо
            XOffset += walkSpeed;

            // Если кот выходит за пределы экрана, сбрасываем его позицию
            if (XOffset > 300)
                XOffset = 0;
        }

        // Команда для удаления панели опций
        [RelayCommand]
        private void RemoveOptionsBar()
        {
            RibbonController.RemoveOptionsBar();
        }
    }
}