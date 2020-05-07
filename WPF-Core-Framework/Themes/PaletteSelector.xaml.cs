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

        /// <summary>
        /// 单例模式
        /// </summary>
        public static PaletteSelector Instance { get; } = new PaletteSelector();

        /// <summary>
        /// 关闭时隐藏
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            this.Hide();
        }
    }
}
