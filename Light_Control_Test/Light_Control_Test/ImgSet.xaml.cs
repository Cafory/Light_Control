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
    /// ImgSet.xaml 的交互逻辑
    /// </summary>
    public partial class ImgSet : Window
    {
        public ImgSet()
        {
            InitializeComponent();
        }
        bool trueFlag = false;
        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from information_schema.TABLES where table_name='_" + inWhichPei.Text + "'");
            if (dt.Rows.Count>0)
            {
                dt = SqlHelper.ExecuteDataTable("select * from _" + inWhichPei.Text + " where Name='_" + imgCode.Text + "'");
                if (dt.Rows.Count>0)
                {
                    NormolProcess.imgInfo = new string[] { "_" + imgCode.Text, "_" + inWhichPei.Text };
                    trueFlag = true;
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("灯具不存在，请先添加灯具!", "提示");

                }
            }
            else
            {
                MessageBox.Show("配电箱不存在，请先添加配电箱!", "提示");
            }
          
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!trueFlag)
            {
                trueFlag = false;
            }
        }
    }
}
