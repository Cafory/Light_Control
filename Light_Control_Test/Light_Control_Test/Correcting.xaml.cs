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
    /// Correcting.xaml 的交互逻辑
    /// </summary>
    public partial class Correcting : Window
    {
        public Correcting()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            trueMore button = new trueMore();
            button.Show();
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string name = tb_1.Text.ToString();
            //dt中存放着数据库所有表的名字
            DataTable dt = SqlHelper.ExecuteDataTable("SELECT NAME FROM SYSOBJECTS WHERE TYPE='U'");
            //将dt中的值取出来放入数组,再将数组放入mylist中。
            DataTable dt1 = dt;
            int len = dt1.Rows.Count;
            string[] str = new string[len];
            for (int i = 0; i < len; i++)
            {
                str[i] = dt1.Rows[i]["Name"].ToString();
            }
            List<string> mylist = new List<string>();
            mylist.AddRange(str);
            //目前，表的名称已经在mylist中，现在要进行将name和表中的name比对。
            DataTable dt_1 = null;
            try
            {
                foreach (var item in mylist)
                {

                    dt_1 = SqlHelper.ExecuteDataTable("select * from " + item + " where Name='" + "_" + name + "'");
                    if (dt_1.Rows.Count > 0)//如果name与name匹配则在RichTextBox中显示数据
                    {
                        DataTable dt2 = SqlHelper.ExecuteDataTable("select Position,PrePlanLeft,Status,PrePlanRight from " + item + " where Name='" + "_" + name + "'");
                        rtb_1.Items.Add("位置");
                        rtb_1.Items.Add(dt2.Rows[0]["Position"]);
                        rtb_1.Items.Add("左预案");
                        rtb_1.Items.Add(dt2.Rows[0]["PrePlanLeft"]);
                        rtb_1.Items.Add("右预案");
                        rtb_1.Items.Add(dt2.Rows[0]["PrePlanRight"]);
                        rtb_1.Items.Add(dt2.Rows[0]["Status"]);
                        break;
                    }

                }
            }
            catch 
            {
                MessageBox.Show("未找到此灯具!", "提示");
            }
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            string btName = ((Button)sender).Name;

            switch (btName)
            {
                case "b_1": tb_1.Text = tb_1.Text + "1"; break;
                case "b_2": tb_1.Text = tb_1.Text + "2"; break;
                case "b_3": tb_1.Text = tb_1.Text + "3"; break;
                case "b_4": tb_1.Text = tb_1.Text + "4"; break;
                case "b_5": tb_1.Text = tb_1.Text + "5"; break;
                case "b_6": tb_1.Text = tb_1.Text + "6"; break;
                case "b_7": tb_1.Text = tb_1.Text + "7"; break;
                case "b_8": tb_1.Text = tb_1.Text + "8"; break;
                case "b_9": tb_1.Text = tb_1.Text + "9"; break;
                case "b_0": tb_1.Text = tb_1.Text + "0"; break;
                case "b_back": { this.Close(); this.Owner.Show(); } break;
                case "b_dh": tb_1.Text = tb_1.Text + "-"; break;
                case "b_delect":
                    {
                        if (tb_1.Text.Length <= 0)
                            tb_1.Text = "";
                        else
                            tb_1.Text = tb_1.Text.Substring(0, tb_1.Text.Length - 1);
                    }; break;
                case "b_check":
                    {
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from Users where tb_1='" + tb_1.Text.ToString() + "'");
                        if (dt.Rows.Count > 0)
                        {
                            ((MainWindow)this.Owner).manager = true;
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
