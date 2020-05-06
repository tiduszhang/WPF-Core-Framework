using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Themes
{
    /// <summary>
    /// Interaction logic for Palette.xaml
    /// </summary>
    public partial class PaletteSelector : MetroWindow
    {
        public PaletteSelector()
        {
            InitializeComponent();

            this.DataContext = new PaletteSelectorViewModel();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            this.Hide();
        }
    }
}
