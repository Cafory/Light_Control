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
            string[] text = null;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (!File.Exists(path + "\\selfCheck.txt"))
            {
                File.Create(path + "\\selfCheck.txt");
              
            }
            else
            {
                text = File.ReadAllLines(path + "\\selfCheck.txt");
                if (text.Length==3)
                {
                    if (text[0].Split('.', ':', ' ').Length==6)
                    {
                        m1.Text = text[0].Split('.', ':', ' ')[0];
                        m2.Text = text[0].Split('.', ':', ' ')[1];
                        m3.Text = text[0].Split('.', ':', ' ')[2];
                        m4.Text = text[0].Split('.', ':', ' ')[3];
                        m5.Text = text[0].Split('.', ':', ' ')[4];
                        m6.Text = text[0].Split('.', ':', ' ')[5];
                    }
                    if (text[1].Split('.', ':', ' ').Length==6)
                    {
                        y1.Text = text[1].Split('.', ':', ' ')[0];
                        y2.Text = text[1].Split('.', ':', ' ')[1];
                        y3.Text = text[1].Split('.', ':', ' ')[2];
                        y4.Text = text[1].Split('.', ':', ' ')[3];
                        y5.Text = text[1].Split('.', ':', ' ')[4];
                        y6.Text = text[1].Split('.', ':', ' ')[5];
                    }
                }
            }
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
            ((MainWindow)this.Owner).dateCheck.Content = text[0]+"+"+text[1]+"+"+text[2];
            ((MainWindow)this.Owner).dateCheck.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            MessageBox.Show("修改成功！", "提示");
        }


            
    }
}
