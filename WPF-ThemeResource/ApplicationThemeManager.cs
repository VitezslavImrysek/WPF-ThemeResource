using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WPF_ThemeResource.Extensions;

namespace WPF_ThemeResource
{
    public static class ApplicationThemeManager
    {
        /// <summary>
        /// The maximum value of the background HSV brightness after which the text on the accent will be turned dark.
        /// </summary>
        private const double BackgroundBrightnessThresholdValue = 80d;

        private static readonly List<WeakReference<ThemeResourceDictionary>> _Dictionaries = new List<WeakReference<ThemeResourceDictionary>>();

        static ApplicationThemeManager()
        {
            Apply((Color)ColorConverter.ConvertFromString("#3379d9"), ApplicationTheme.Light);
        }

        /// <summary>
        /// Gets the SystemAccentColor.
        /// </summary>
        public static Color SystemAccentColor { get; private set; }

        /// <summary>
        /// Gets the SystemAccentColorPrimary.
        /// </summary>
        public static Color SystemAccentColorPrimary { get; private set; }

        /// <summary>
        /// Gets the SystemAccentColorSecondary.
        /// </summary>
        public static Color SystemAccentColorSecondary { get; private set; }

        /// <summary>
        /// Gets the SystemAccentColorTertiary.
        /// </summary>
        public static Color SystemAccentColorTertiary { get; private set; }

        private static ApplicationTheme _RequestedTheme = ApplicationTheme.Light;
        public static ApplicationTheme RequestedTheme
        {
            get { return _RequestedTheme; }
            set 
            {
                if (_RequestedTheme != value)
                {
                    Apply(SystemAccentColor, value);
                }
            }
        }

        /// <summary>
        /// Changes the color accents of the application based on the color entered.
        /// </summary>
        /// <param name="systemAccent">Primary accent color.</param>
        /// <param name="applicationTheme">If <see cref="ApplicationTheme.Dark"/>, the colors will be different.</param>
        /// <param name="systemGlassColor">If the color is taken from the Glass Color System, its brightness will be increased with the help of the operations on HSV space.</param>
        public static void Apply(
            Color systemAccent,
            ApplicationTheme applicationTheme = ApplicationTheme.Light,
            bool systemGlassColor = false
        )
        {
            if (systemGlassColor)
            {
                // WindowGlassColor is little darker than accent color
                systemAccent = systemAccent.UpdateBrightness(6f);
            }

            Color primaryAccent;
            Color secondaryAccent;
            Color tertiaryAccent;

            if (applicationTheme == ApplicationTheme.Dark)
            {
                primaryAccent = systemAccent.Update(15f, -12f);
                secondaryAccent = systemAccent.Update(30f, -24f);
                tertiaryAccent = systemAccent.Update(45f, -36f);
            }
            else
            {
                primaryAccent = systemAccent.UpdateBrightness(-5f);
                secondaryAccent = systemAccent.UpdateBrightness(-10f);
                tertiaryAccent = systemAccent.UpdateBrightness(-15f);
            }

            Apply(systemAccent, primaryAccent, secondaryAccent, tertiaryAccent, applicationTheme);
        }

        /// <summary>
        /// Changes the color accents of the application based on the entered colors.
        /// </summary>
        /// <param name="systemAccent">Primary color.</param>
        /// <param name="primaryAccent">Alternative light or dark color.</param>
        /// <param name="secondaryAccent">Second alternative light or dark color (most used).</param>
        /// <param name="tertiaryAccent">Third alternative light or dark color.</param>
        public static void Apply(
            Color systemAccent,
            Color primaryAccent,
            Color secondaryAccent,
            Color tertiaryAccent,
            ApplicationTheme applicationTheme
        )
        {
            var accentColorChanged = SystemAccentColor != systemAccent || SystemAccentColorPrimary != primaryAccent || SystemAccentColorSecondary != secondaryAccent || SystemAccentColorTertiary != tertiaryAccent;
            var applicationThemeChanged = applicationTheme != RequestedTheme;

            _RequestedTheme = applicationTheme;
            SystemAccentColor = systemAccent;
            SystemAccentColorPrimary = primaryAccent;
            SystemAccentColorSecondary = secondaryAccent;
            SystemAccentColorTertiary = tertiaryAccent;
            
            if (accentColorChanged || applicationThemeChanged)
            {
                UpdateColorResources();
                OnThemeChanged();
            }
        }

        internal static void AddThemeDictionary(ThemeResourceDictionary resourceDictionary)
        {
            _Dictionaries.Add(new WeakReference<ThemeResourceDictionary>(resourceDictionary));
        }

        private static void OnThemeChanged()
        {
            var dictionaries = _Dictionaries;
            var oldDictionaries = dictionaries.ToList();

            dictionaries.Clear();

            foreach (var dictionaryRef in oldDictionaries)
            {
                ThemeResourceDictionary dictionary;
                if (dictionaryRef.TryGetTarget(out dictionary))
                {
                    dictionary.OnThemeChanged();
                    dictionaries.Add(dictionaryRef);
                }
            }
        }

        internal static void UpdateColorResources()
        {
            UpdateColorResources(SystemAccentColor, SystemAccentColorPrimary, SystemAccentColorSecondary, SystemAccentColorTertiary);
        }

        private static void UpdateColorResources(
            Color systemAccent,
            Color primaryAccent,
            Color secondaryAccent,
            Color tertiaryAccent)
        {
            var application = Application.Current;
            if (application == null)
            {
                return;
            }

            var resources = Application.Current.Resources;
            if (resources == null)
            {
                return;
            }

            if (secondaryAccent.GetBrightness() > BackgroundBrightnessThresholdValue)
            {
                resources[ThemeResourceKey.TextOnAccentFillColorPrimary.ToString()] = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
                resources[ThemeResourceKey.TextOnAccentFillColorSecondary.ToString()] = Color.FromArgb(0x80, 0x00, 0x00, 0x00);
                resources[ThemeResourceKey.TextOnAccentFillColorDisabled.ToString()] = Color.FromArgb(0x77, 0x00, 0x00, 0x00);
                resources[ThemeResourceKey.TextOnAccentFillColorSelectedText.ToString()] = Color.FromArgb(0x00, 0x00, 0x00, 0x00);
                resources[ThemeResourceKey.AccentTextFillColorDisabled.ToString()] = Color.FromArgb(0x5D, 0x00, 0x00, 0x00);
            }
            else
            {
                resources[ThemeResourceKey.TextOnAccentFillColorPrimary.ToString()] = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                resources[ThemeResourceKey.TextOnAccentFillColorSecondary.ToString()] = Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF);
                resources[ThemeResourceKey.TextOnAccentFillColorDisabled.ToString()] = Color.FromArgb(0x87, 0xFF, 0xFF, 0xFF);
                resources[ThemeResourceKey.TextOnAccentFillColorSelectedText.ToString()] = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                resources[ThemeResourceKey.AccentTextFillColorDisabled.ToString()] = Color.FromArgb(0x5D, 0xFF, 0xFF, 0xFF);
            }

            resources[ThemeResourceKey.SystemAccentColor.ToString()] = systemAccent;
            resources[ThemeResourceKey.SystemAccentColorPrimary.ToString()] = primaryAccent;
            resources[ThemeResourceKey.SystemAccentColorSecondary.ToString()] = secondaryAccent;
            resources[ThemeResourceKey.SystemAccentColorTertiary.ToString()] = tertiaryAccent;
            
            resources[ThemeResourceKey.SystemAccentBrush.ToString()] = secondaryAccent.ToBrush();
            resources[ThemeResourceKey.SystemFillColorAttentionBrush.ToString()] = secondaryAccent.ToBrush();
            resources[ThemeResourceKey.AccentTextFillColorPrimaryBrush.ToString()] = tertiaryAccent.ToBrush();
            resources[ThemeResourceKey.AccentTextFillColorSecondaryBrush.ToString()] = tertiaryAccent.ToBrush();
            resources[ThemeResourceKey.AccentTextFillColorTertiaryBrush.ToString()] = secondaryAccent.ToBrush();
            resources[ThemeResourceKey.AccentFillColorSelectedTextBackgroundBrush.ToString()] = systemAccent.ToBrush();
            resources[ThemeResourceKey.AccentFillColorDefaultBrush.ToString()] = secondaryAccent.ToBrush();
            
            resources[ThemeResourceKey.AccentFillColorSecondaryBrush.ToString()] = secondaryAccent.ToBrush(0.9);
            resources[ThemeResourceKey.AccentFillColorTertiaryBrush.ToString()] = secondaryAccent.ToBrush(0.8);
        }
    }
}
