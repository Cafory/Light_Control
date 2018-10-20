using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// SelfCheck.xaml 的交互逻辑
    /// </summary>
    public partial class SelfCheck : Window
    {
        public SelfCheck()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void checkConfirm_Click(object sender, RoutedEventArgs e)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!File.Exists(path+"\\selfCheck.txt"))
            {
                File.Create(path + "\\selfCheck.txt");
            }
           string []text ={m1.Text+"."+m2.Text+"."+m3.Text+" "+m4.Text+":"+m5.Text+":"+m6.Text,y1.Text+"."+y2.Text+"."+y3.Text+" "+y4.Text+":"+y5.Text+":"+y6.Text,speed.Text };
            File.WriteAllLines(path + "\\selfCheck.txt", text);
            ((MainWindow)this.Owner).dateCheck.Content = text[0];
            MessageBox.Show("修改成功！", "提示");
            
        }


            
    }
}
