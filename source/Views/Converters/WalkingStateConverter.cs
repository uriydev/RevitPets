using System.Globalization;
using System.Windows.Data;

namespace RevitPets.Views.Converters;

public class WalkingStateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Проверка состояния бега (IsWalking)
        if (value is bool isWalking)
        {
            return isWalking
                ? "pack://application:,,,/RevitPets;component/Resources/Animations/Anim193x200.gif"  // Анимация бега
                : "pack://application:,,,/RevitPets;component/Resources/Animations/Anim399.gif";      // Анимация стояния
        }
        return "pack://application:,,,/RevitPets;component/Resources/Animations/Anim399.gif"; // По умолчанию анимация стояния
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Не требуется двусторонняя конвертация, так что возвращаем null
        return null;
    }
}
