using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WPF_ThemeResource.TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var theme = ApplicationThemeManager.RequestedTheme;
            switch (theme)
            {
                case ApplicationTheme.Dark:
                    theme = ApplicationTheme.Light;
                    break;
                case ApplicationTheme.Light:
                    theme = ApplicationTheme.Dark;
                    break;
            }

            ApplicationThemeManager.Apply(ApplicationThemeManager.SystemAccentColor, theme);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var colors = new List<Color>()
            {
                (Color)ColorConverter.ConvertFromString("#3379d9"),
                Colors.Purple,
                Colors.Green,
                Colors.CornflowerBlue,
                Colors.Orange
            };

            var colorIndex = colors.IndexOf(ApplicationThemeManager.SystemAccentColor);
            colorIndex = (colorIndex + 1) % colors.Count;

            var color = colors[colorIndex];

            ApplicationThemeManager.Apply(color, ApplicationThemeManager.RequestedTheme);
        }
    }
}
