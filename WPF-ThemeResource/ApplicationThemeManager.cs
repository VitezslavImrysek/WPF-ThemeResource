using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace WPF_ThemeResource
{
    public static class ApplicationThemeManager
    {
        private static readonly List<WeakReference<ResourceDictionaryEx>> _Dictionaries = new List<WeakReference<ResourceDictionaryEx>>();

        private static ApplicationTheme _RequestedTheme = ApplicationTheme.Light;
        public static ApplicationTheme RequestedTheme
        {
            get { return _RequestedTheme; }
            set { _RequestedTheme = value; OnThemeChanged(value); }
        }

        public static readonly DependencyProperty DesignThemeProperty =
            DependencyProperty.RegisterAttached(
                "DesignTheme", 
                typeof(ApplicationTheme), 
                typeof(ApplicationThemeManager), 
                new PropertyMetadata(ApplicationTheme.Light, new PropertyChangedCallback(OnDesignThemeChanged)));

        private static void OnDesignThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
            {
                RequestedTheme = (ApplicationTheme)e.NewValue;
            }
        }

        public static ApplicationTheme GetDesignTheme(FrameworkElement obj)
        {
            return (ApplicationTheme)obj.GetValue(DesignThemeProperty);
        }

        public static void SetDesignTheme(FrameworkElement obj, ApplicationTheme value)
        {
            obj.SetValue(DesignThemeProperty, value);
        }

        internal static void AddThemeDictionary(ResourceDictionaryEx resourceDictionary)
        {
            _Dictionaries.Add(new WeakReference<ResourceDictionaryEx>(resourceDictionary));
        }

        private static void OnThemeChanged(ApplicationTheme theme)
        {
            var dictionaries = _Dictionaries;
            var oldDictionaries = dictionaries.ToList();

            dictionaries.Clear();

            foreach (var dictionaryRef in oldDictionaries)
            {
                ResourceDictionaryEx dictionary;
                if (dictionaryRef.TryGetTarget(out dictionary))
                {
                    dictionary.OnThemeChanged();
                    dictionaries.Add(dictionaryRef);
                }
            }
        }
    }
}
