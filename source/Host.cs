using Microsoft.Extensions.DependencyInjection;
using RevitPets.ViewModels;
using RevitPets.Views;
using RevitPets.Views.Utils;

namespace RevitPets;

public static class Host
{
    private static IServiceProvider _serviceProvider;
    
    public static void Start()
    {
        var services = new ServiceCollection();
        
        services.AddTransient<UtilsView>();
        services.AddTransient<UtilsViewModel>();
        services.AddTransient<PetWindow>();
        services.AddTransient<PetWindowViewModel>();
        
        services.AddTransient<RibbonController>();
        
        _serviceProvider = services.BuildServiceProvider();
    }
    
    public static T GetService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}