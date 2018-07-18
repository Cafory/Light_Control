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

namespace Light_Control_Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 后台改背景  MainGrid.Background = new ImageBrush
            //{
            //    ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/主界面.png"))
            //};

            FireWork.Visibility = Visibility.Visible;
        }
            
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            button1.Visibility = Visibility.Collapsed;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            menuImage.Opacity = 1;
            menuImage.Visibility = Visibility.Visible;

        }

        private void menuImage_MouseLeave(object sender, MouseEventArgs e)
        {
            this.menuImage.Opacity = 0;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((Control)sender).Name!="menuImage")
            {
                menuImage.Opacity = 0;
                menuImage.Visibility = Visibility.Collapsed;
                SecondMenuDisplay(-1);
            }       
        }

       

        private void SecondMenuDisplay(int i)   //二级菜单显示，只能同时显示一个或者零个
        {
            System.Windows.Controls.Primitives.Popup[] popue = new System.Windows.Controls.Primitives.Popup[] { SecondMenuPop, SecondMenuPop2, SecondMenuPop3 };
            foreach (System.Windows.Controls.Primitives.Popup item in popue)
            {
                item.IsOpen = false;
            }
           if(i>=0)
                popue[i].IsOpen = true; 
                
        }

        private void menuImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)  //结束路由事件的传递
        {
            e.Handled = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            SecondMenuDisplay(-1);
            menuImage.Opacity = 0;
        }

        private void FireWorkBack_Click(object sender, RoutedEventArgs e)
        {
            FireWork.Visibility = Visibility.Collapsed;
        }


        private void MainMenuSet_Click(object sender, RoutedEventArgs e)
        {
            SecondMenuDisplay(0);
        }

        private void DataCenter_Click(object sender, RoutedEventArgs e)
        {
            SecondMenuDisplay(1);
        }

        private void DataLoad_Click(object sender, RoutedEventArgs e)
        {
            SecondMenuDisplay(2);
        }
    }
}
