using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoeMais.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        TimeSpan time;
        public TimeSpan Time { get => time; set => time = value; }

        public TimePicker()
        {
            InitializeComponent();
        }

        private void setTimeScroll(object sender, MouseWheelEventArgs e)
        {
            validaHora();
            if (e.Delta > 0)
            {
                //DoActionUp();
                upHour();
            }
            else if (e.Delta < 0)
            {
                //DoActionDown();
                downHour();
            }
        }

        private void upButton(object s, RoutedEventArgs e)
        {
            validaHora();
            upHour();
        }

        private void downButton(object s, RoutedEventArgs e)
        {
            validaHora();
            downHour();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TMouseLeave(object sender, MouseEventArgs e)
        {
            validaHora();
        }

        public void upHour()
        {
            byte hora = Convert.ToByte(textBox_hora.Text);
            byte minuto = Convert.ToByte(textBox_minuto.Text);

            minuto += 15;

            if (minuto > 59)
            {
                hora++;
                minuto -= 60;
            }
            if (hora > 23)
                hora = 0;

            textBox_hora.Text = "" + hora;
            textBox_minuto.Text = "" + minuto;
            Time = new TimeSpan(hora, minuto, 0);
        }

        public void downHour()
        {
            int hora = Convert.ToByte(textBox_hora.Text);
            int minuto = Convert.ToByte(textBox_minuto.Text);

            minuto -= 15;

            if (minuto < 1)
            {
                hora--;
                minuto += 45;
            }
            if (hora < 0)
                hora = 23;

            textBox_hora.Text = "" + hora;
            textBox_minuto.Text = "" + minuto;
            Time = new TimeSpan(hora, minuto, 0);
        }

        public void validaHora()
        {
            if (textBox_hora.Text.Trim().Equals(""))
                textBox_hora.Text = "0";

            if (textBox_minuto.Text.Trim().Equals(""))
                textBox_minuto.Text = "0";
        }

    }
}
