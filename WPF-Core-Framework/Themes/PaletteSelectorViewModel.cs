using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace Themes
{
    public class PaletteSelectorViewModel : INotifyPropertyChanged
    {
        public PaletteSelectorViewModel()
        {
            Swatches = new SwatchesProvider().Swatches.Where(o => o.IsAccented);
        }

        public ICommand ToggleStyleCommand { get; } = new AnotherCommandImplementation(o => ApplyStyle((bool)o));

        public ICommand ToggleBaseCommand { get; } = new AnotherCommandImplementation(o => ApplyBase((bool)o));

        public IEnumerable<Swatch> Swatches { get; }

        public ICommand ApplyPrimaryCommand { get; } = new AnotherCommandImplementation(o => ApplyPrimary((Swatch)o));

        public ICommand ApplyAccentCommand { get; } = new AnotherCommandImplementation(o => ApplyAccent((Swatch)o));

        /// <summary>
        /// 应用样式
        /// </summary>
        private ICommand ApplyStyleCommand { get; } = new AnotherCommandImplementation(o =>
        {
            ApplyPrimary((Swatch)o);
            ApplyAccent((Swatch)o);
        });

        /// <summary>
        /// 属性更改通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 选中的样式
        /// </summary>
        Swatch _SelectSwatch;
        /// <summary>
        /// 选中的样式
        /// </summary>
        public Swatch SelectSwatch
        {
            get
            {
                return _SelectSwatch;
            }
            set
            {
                _SelectSwatch = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("SelectSwatch"));

                if (_SelectSwatch != null)
                {
                    ApplyStyleCommand.Execute(_SelectSwatch);
                }
            }
        }

        private static void ApplyStyle(bool alternate)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri(@"pack://application:,,,/Dragablz;component/Themes/materialdesign.xaml")
            };

            var styleKey = alternate ? "MaterialDesignAlternateTabablzControlStyle" : "MaterialDesignTabablzControlStyle";
            var style = (Style)resourceDictionary[styleKey];

            var template = (System.Windows.Controls.ControlTemplate)Application.Current.FindResource("MyTabablzControlControlTemplate");

            foreach (var tabablzControl in Dragablz.TabablzControl.GetLoadedInstances())
            {
                tabablzControl.Style = style;
                tabablzControl.Template = template;
                tabablzControl.ApplyTemplate();
            }


        }

        private static void ApplyBase(bool isDark)
        {
            ModifyTheme(theme => theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light));
        }

        private static void ApplyPrimary(Swatch swatch)
        {
            ModifyTheme(theme => theme.SetPrimaryColor(swatch.ExemplarHue.Color));
        }

        private static void ApplyAccent(Swatch swatch)
        {
            ModifyTheme(theme => theme.SetSecondaryColor(swatch.AccentExemplarHue.Color));
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}
