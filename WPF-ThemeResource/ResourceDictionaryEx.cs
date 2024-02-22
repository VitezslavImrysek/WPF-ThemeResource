using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace WPF_ThemeResource
{
    public class ResourceDictionaryEx : ResourceDictionary, ISupportInitialize
    {
        private ApplicationTheme? _theme;
        private bool _initialized = false;

        public ResourceDictionaryEx()
        {
            
        }

        public IDictionary ThemeDictionaries { get; set; } = new ResourceDictionary();

        public new void EndInit()
        {
            base.EndInit();

            _initialized = true;
            ApplicationThemeManager.AddThemeDictionary(this);
            RefreshTheme();
        }

        internal void OnThemeChanged()
        {
            RefreshTheme();
        }

        private void RefreshTheme()
        {
            if (!_initialized)
            {
                return;
            }

            if (_theme != null)
            {
                // Odeberu veškeré současné resources.
                UnloadResources(_theme.Value);
            }

            var theme = ApplicationThemeManager.RequestedTheme;
            LoadResources(theme);
        }

        private void LoadResources(ApplicationTheme theme)
        {
            ResourceDictionary resources;
            if (!TryGetThemeResources(theme.ToString(), out resources))
            {
                return;
            }

            _theme = theme;

            var enumerator = resources.GetEnumerator();
            while (enumerator.MoveNext())
            {
                this[enumerator.Key] = enumerator.Value;
            }
        }

        private void UnloadResources(ApplicationTheme theme)
        {
            ResourceDictionary resources;
            if (!TryGetThemeResources(theme.ToString(), out resources))
            {
                return;
            }

            _theme = null;

            var enumerator = resources.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Remove(enumerator.Key);
            }
        }

        private bool TryGetThemeResources(string key, out ResourceDictionary resources)
        {
            if (!ThemeDictionaries.Contains(key))
            {
                // Fallback to default
                key = "Default";
            }

            if (!ThemeDictionaries.Contains(key))
            {
                resources = null;
                return false;
            }

            var themeResources = ThemeDictionaries[key] as ResourceDictionary;
            if (themeResources == null)
            {
                resources = null;
                return false;
            }

            resources = themeResources;
            return true;
        }
    }
}
