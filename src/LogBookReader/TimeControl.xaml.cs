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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogBookReader
{
    /// <summary>
    /// Логика взаимодействия для TimeControl.xaml
    /// </summary>
    public partial class TimeControl : UserControl
    {
        public TimeControl()
        {
            InitializeComponent();
        }

        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(TimeSpan),
                typeof(TimeControl),
                new UIPropertyMetadata(DateTime.Now.TimeOfDay, new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimeControl control = obj as TimeControl;
            control.Hours = ((TimeSpan)e.NewValue).Hours;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.Seconds = ((TimeSpan)e.NewValue).Seconds;
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register(
                "Hours",
                typeof(int),
                typeof(TimeControl),
                new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register("Minutes",
                typeof(int),
                typeof(TimeControl),
                new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register("Seconds",
                typeof(int),
                typeof(TimeControl),
                new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));


        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimeControl control = obj as TimeControl;

            control.Hours = GetMinMaxTimeValue(control.Hours, 23);
            control.Minutes = GetMinMaxTimeValue(control.Minutes, 59);
            control.Seconds = GetMinMaxTimeValue(control.Seconds, 59);

            control.Value = new TimeSpan(control.Hours,
                                         control.Minutes,
                                         control.Seconds);
        }

        private static int GetMinMaxTimeValue(int timeValue, int maxValue)
        {
            if (timeValue < 0)
                timeValue = 0;
            else if (timeValue > maxValue)
                timeValue = maxValue;

            return timeValue;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            //if (e.Action == ValidationErrorEventAction.Added)
            //{
            //    ((TextBox)sender).Text = "1";
            //    e.Handled = true;
            //}
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:

                    string textSender = ((TextBox)sender).Text;
                    string selectedTextSender = ((TextBox)sender).SelectedText;

                    if (textSender.Length > 1
                        && selectedTextSender.Length == 0)
                        e.Handled = true;

                    break;

                case Key.Tab:
                case Key.Home:
                case Key.End:
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                case Key.Insert:
                case Key.Back:
                case Key.Delete:
                    break;

                default:
                    e.Handled = true;
                    break;
            }
        }
    }
}
