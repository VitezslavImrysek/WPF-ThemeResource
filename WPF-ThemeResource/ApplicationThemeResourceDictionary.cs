using System;
using System.Windows;

namespace WPF_ThemeResource
{
    public class ApplicationThemeResourceDictionary : ThemeResourceDictionary
    {
        public ApplicationThemeResourceDictionary()
        {
            MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/WPF-ThemeResource;component/Themes/Common.xaml") });

            ThemeDictionaries.Add("Light", new ResourceDictionary() { Source = new Uri("pack://application:,,,/WPF-ThemeResource;component/Themes/Light.xaml") });
            ThemeDictionaries.Add("Dark", new ResourceDictionary() { Source = new Uri("pack://application:,,,/WPF-ThemeResource;component/Themes/Dark.xaml") });

            ApplicationThemeManager.UpdateColorResources();
        }

        public ApplicationTheme RequestedTheme
        {
            get { return ApplicationThemeManager.RequestedTheme; }
            set { ApplicationThemeManager.RequestedTheme = value; }
        }
    }
}
