namespace RevitPets.ViewModels;

public partial class UtilsViewModel : ObservableObject
{
    private double _panelWidth;
    
    public double PanelWidth
    {
        get => _panelWidth;
        set => SetProperty(ref _panelWidth, value);
    }
    
    [ObservableProperty]
    private bool isWalking;
    
    [ObservableProperty]
    private string animatedSource;
    
    [ObservableProperty]
    private double xOffset;  // Смещение по X
    
    private readonly double walkSpeed = 1;  // Скорость движения
    
    public UtilsViewModel()
    {
        // Начальная анимация - кот стоит
        AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
        
        ToggleWalking();
    }
    
    [RelayCommand]
    public void ToggleWalking()
    {
        IsWalking = !IsWalking;
    
        AnimatedSource = IsWalking
            ? "pack://application:,,,/RevitPets;component/Resources/Animations/RunRight.gif"
            : "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
    
        if (IsWalking)
            StartWalking();
    }
    
    private async Task StartWalking()
    {
        var stopTime = DateTime.Now.AddSeconds(5);
    
        while (IsWalking && DateTime.Now < stopTime)
        {
            XOffset += walkSpeed;
    
            if (XOffset > 300)
                XOffset = 0;
    
            await Task.Delay(10);
        }
    
        IsWalking = false;
        AnimatedSource = "pack://application:,,,/RevitPets;component/Resources/Animations/Idle.gif";
    }
}