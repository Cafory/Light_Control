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

namespace Light_Control_Test
{
    /// <summary>
    /// FireWorkTogether.xaml 的交互逻辑
    /// </summary>
    public partial class FireWorkTogether : Window
    {
        public FireWorkTogether()
        {
            InitializeComponent();
        }

        private void manualCheck_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Name;
            switch (name)
            {
                case "workSiginal":
                    {
                        workSiginalGrid.Visibility = Visibility.Visible;
                        workMoniGrid.Visibility = Visibility.Collapsed;
                        workSetGrid.Visibility = Visibility.Collapsed;
                        mainGrid.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/空界面1.png")) };
                    }; break;
                case "workMoni":
                    {
                        workSiginalGrid.Visibility = Visibility.Collapsed;
                        workMoniGrid.Visibility = Visibility.Visible;
                        workSetGrid.Visibility = Visibility.Collapsed;
                        mainGrid.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/空界面2.png")) };
                    }
                    break;
                case "workSet":
                    {
                        workSiginalGrid.Visibility = Visibility.Collapsed;
                        workMoniGrid.Visibility = Visibility.Collapsed;
                        workSetGrid.Visibility = Visibility.Visible;
                        mainGrid.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/空界面3.png")) };
                    }
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
