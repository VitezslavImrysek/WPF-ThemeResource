using System.Windows;

namespace WPF_ThemeResource
{
    public class ApplicationEx : Application
    {
        public ApplicationTheme RequestedTheme 
        { 
            get => ApplicationThemeManager.RequestedTheme;
            set => ApplicationThemeManager.RequestedTheme = value;
        }
    }
}
