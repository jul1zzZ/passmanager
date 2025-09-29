using System.Globalization;

namespace pass.Converters;

public class SelectedIconConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is string thisIcon && values[1] is string selectedIcon)
        {
            return thisIcon == selectedIcon ? Colors.DeepSkyBlue : Colors.Gray;
        }
        return Colors.Gray;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
