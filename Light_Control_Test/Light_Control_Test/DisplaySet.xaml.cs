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
    /// DisplaySet.xaml 的交互逻辑
    /// </summary>
    public partial class DisplaySet : Window
    {
        public DisplaySet()
        {
            InitializeComponent();
         
        }

        private void disSetBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

  

        private void disSetConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (disLocation.IsChecked == true)
            {
                ((MainWindow)this.Owner).disSetFlag = true;
            }
            else
            {
                ((MainWindow)this.Owner).disSetFlag = false;
            }
            if (((MainWindow)this.Owner).addStatus=="配电箱")
            {
                ((MainWindow)this.Owner).EleBoxDis();
            }
            else
            {
                ((MainWindow)this.Owner).LightDis(((MainWindow)this.Owner)._lightHost.Text);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool aa = ((MainWindow)this.Owner).disSetFlag;
            if (((MainWindow)this.Owner).disSetFlag == true)
            {
                disLocation.IsChecked = true;
            }
            else
            {
                phicsCode.IsChecked = true;
            }
        }
    }
}
