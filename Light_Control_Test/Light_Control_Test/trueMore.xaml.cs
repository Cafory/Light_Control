using System;
using System.Collections.Generic;
using System.Data;
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
    /// trueMore.xaml 的交互逻辑
    /// </summary>
    public partial class trueMore : Window
    {
        public trueMore()
        {
            InitializeComponent();
        }
        private void b1(object sender, RoutedEventArgs e)
        {


        }

        private void b_1_Click(object sender, RoutedEventArgs e)
        {
            string btName = ((Button)sender).Name;

            switch (btName)
            {
                case "b_1": pwd.Password = pwd.Password + "1"; break;
                case "b_2": pwd.Password = pwd.Password + "2"; break;
                case "b_3": pwd.Password = pwd.Password + "3"; break;
                case "b_4": pwd.Password = pwd.Password + "4"; break;
                case "b_5": pwd.Password = pwd.Password + "5"; break;
                case "b_6": pwd.Password = pwd.Password + "6"; break;
                case "b_7": pwd.Password = pwd.Password + "7"; break;
                case "b_8": pwd.Password = pwd.Password + "8"; break;
                case "b_9": pwd.Password = pwd.Password + "9"; break;
                case "b_0": pwd.Password = pwd.Password + "0"; break;
                case "b_back": { this.Close(); this.Owner.Show(); } break;
                case "b_dh": pwd.Password = pwd.Password + "-"; break;
                case "b_delect":
                    {
                        if (pwd.Password.Length <= 0)
                            pwd.Password = "";
                        else
                            pwd.Password = pwd.Password.Substring(0, pwd.Password.Length - 1);
                    }; break;
                case "b_check":
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from Users where Pwd='" + pwd.Password.ToString() + "'");
                        if (dt.Rows.Count>0)
                        {
                            ((MainWindow)this.Owner).manager = true;
                            ((MainWindow)this.Owner).Login.Content = "注销"; 
                            MessageBox.Show("登录成功！", "提示");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("密码不正确！", "提示");
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}