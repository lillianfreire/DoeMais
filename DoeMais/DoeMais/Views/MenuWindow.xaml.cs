using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DoeMais.Views
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            MinimizeWindow.Click += (s, e) => WindowState = WindowState.Minimized;
            CloseApp.Click += (s, e) => System.Windows.Application.Current.Shutdown();
        }

        private void Button_perfil_Click(object sender, RoutedEventArgs e)
        {
            new Perfil_Itens.PerfilWindow().Show();
        }

        private void Button_doacoes_Click(object sender, RoutedEventArgs e)
        {
            grid_doacoes.Visibility = Visibility.Visible;
            grid_instituicao.Visibility = Visibility.Hidden;
            button_instituicao.Style = Application.Current.FindResource("button_transparent") as Style;
            button_doacoes.Style = Application.Current.FindResource("button_gradient") as Style;
        }

        private void Button_instituicao_Click(object sender, RoutedEventArgs e)
        {
            grid_doacoes.Visibility = Visibility.Hidden;
            grid_instituicao.Visibility = Visibility.Visible;
            button_instituicao.Style = Application.Current.FindResource("button_gradient") as Style;
            button_doacoes.Style = Application.Current.FindResource("button_transparent") as Style;
        }
    }
}
