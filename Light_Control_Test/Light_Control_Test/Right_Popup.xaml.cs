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
    /// Right_Popup.xaml 的交互逻辑
    /// </summary>
    public partial class Right_Popup : Window
    {
        public Right_Popup(string _addName, string status) //_addName里面有灯码和配电箱码，                                                        
        {                                                    //  如果是配电箱的右键只有配电箱码，灯的右键第一个码是灯，第二个是配电箱，status才是状态标识符
            InitializeComponent();
            _status = status;
            code = _addName.Split('+');
            windowTitle.Content = status + code[0];
            replace_BeforeCode.Content = code[0];
            replace_BeforeTip.Content = "原" + status + "码:";
            replace_NowTip.Content = status + "码";
            if (_status == "配电箱")
            {
                set.Content = "安装位置";
            }
            else
            {
                set.Content = "设置";
            }

        }
        string _status;
        string[] code;
        private void Window_Deactivated(object sender, EventArgs e) //窗口失去焦点直接关闭配电箱右键窗体
        {
            this.Close();
        }


        private void control_Click(object sender, RoutedEventArgs e)
        {
            btnGrid.Visibility = Visibility.Collapsed;
            controlGrid.Visibility = Visibility.Visible;
        }

        private void controlGrid_Back_Click(object sender, RoutedEventArgs e)
        {
            controlGrid.Visibility = Visibility.Collapsed;
            btnGrid.Visibility = Visibility.Visible;
        }

        private void replace_Click(object sender, RoutedEventArgs e)
        {
            btnGrid.Visibility = Visibility.Collapsed;
            lightReplace.Visibility = Visibility.Visible;
        }

        private void lightReplace_Back_Click(object sender, RoutedEventArgs e)
        {
            lightReplace.Visibility = Visibility.Collapsed;
            btnGrid.Visibility = Visibility.Visible;
        }

        private void set_Click(object sender, RoutedEventArgs e)
        {
            if (_status == "配电箱")    //这里是因为配电箱是安装位置，灯具是设置稍有不同
            {
                btnGrid.Visibility = Visibility.Collapsed;
                setGrid_LightLocation.Visibility = Visibility.Visible;
                this.setGrid_LightLocation_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                Pos_Tip.Content = "  配电箱位置";
            }
            else
            {
                btnGrid.Visibility = Visibility.Collapsed;
                setGrid.Visibility = Visibility.Visible;
                Pos_Tip.Content = "  灯具位置";
            }
        }

        private void setGrid_IniStatus_btn_Click(object sender, RoutedEventArgs e)
        {
            setGrid.Visibility = Visibility.Collapsed;
            setGrid_IniStatus.Visibility = Visibility.Visible;
        }

        private void setGrid_PrePlnModify_btn_Click(object sender, RoutedEventArgs e)
        {
            setGrid.Visibility = Visibility.Collapsed;
            setGrid_prePlanModify.Visibility = Visibility.Visible;
        }

        private void setGrid_LightLocation_btn_Click(object sender, RoutedEventArgs e)   //设置界面的位置修改按钮
        {
            if (_status == "配电箱")
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select Position from Pei where Name='" + code[0] + "'");
                if (String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    IniPosition.Content = "原位置：未设置";
                }
                else
                    IniPosition.Content = "原位置：" + dt.Rows[0][0].ToString();
            }
            else
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select Position from  " + code[1] + "  where Name='" + code[0] + "'");
                if (String.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    IniPosition.Content = "原位置：未设置";
                }
                else
                    IniPosition.Content = "原位置：" + dt.Rows[0][0].ToString();
            }
            setGrid.Visibility = Visibility.Collapsed;
            setGrid_LightLocation.Visibility = Visibility.Visible;
        }


        private void setGrid_Back_Click(object sender, RoutedEventArgs e)
        {
            setGrid.Visibility = Visibility.Collapsed;
            btnGrid.Visibility = Visibility.Visible;
        }

        private void IniStatus_Back_Click(object sender, RoutedEventArgs e)
        {
            setGrid_IniStatus.Visibility = Visibility.Collapsed;
            setGrid.Visibility = Visibility.Visible;
        }

        private void IniStatus_Confirm_Click(object sender, RoutedEventArgs e)
        {
            //写入方法
            if (IniAllBright.IsChecked == true)
                IniAllBrightChecked();
            if (IniAllOff.IsChecked == true)
                IniAllOffChecked();
            if (IniLeftBright.IsChecked == true)
                IniLeftBrightChecked();
            if (IniRightBright.IsChecked == true)
                IniRightBrightChecked();
        }

        private void prePlanModify_Back_Click(object sender, RoutedEventArgs e)
        {
            setGrid_prePlanModify.Visibility = Visibility.Collapsed;
            setGrid.Visibility = Visibility.Visible;
        }

        private void prePlanModify_Confirm_Click(object sender, RoutedEventArgs e)
        {
            //写入方法
            MessageBox.Show("确认");
        }

        private void lightLocation_Back_Click(object sender, RoutedEventArgs e)
        {
            setGrid_LightLocation.Visibility = Visibility.Collapsed;
            setGrid.Visibility = Visibility.Visible;
        }

        private void lightLocation_Confirm_Click(object sender, RoutedEventArgs e)   //修改位置的方法
        {
            //写入方法
            if (_status == "配电箱")
            {
                string command = "update Pei set Position='" + Area.Text + "区" + Building.Text + "栋" + Layer.Text + "层" + Room.Text + "号" + "'  where Name='" + code[0] + "'";
                SqlHelper.ExecuteNonQuery(command);
            }
            else
            {
                string command = "update " + code[1] + " set Position='" + Area.Text + "区" + Building.Text + "栋" + Layer.Text + "层" + Room.Text + "号" + "' where Name='" + code[0] + "'";
                SqlHelper.ExecuteNonQuery(command);
            }
            // MessageBox.Show("修改成功！");
            this.setGrid_LightLocation_btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void All_Shine_Click(object sender, RoutedEventArgs e)  //全亮
        {
            if (_status == "配电箱")
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x0C, 0x77 });  //配电箱下所有灯
            else
              if (Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x01, 0x77 }))  //指定灯码
              {
                ChangeLNowStatus(SelectPic("1"), "全亮");
              }
        }

        private void All_Off_Click(object sender, RoutedEventArgs e)  //全灭
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x0C, 0x70 });
            }
            else
            {
                if (Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x01, 0x70 }))
                {
                    ChangeLNowStatus(SelectPic("2"), "全灭");
                }
            }
        }

        private void Left_Shine_Click(object sender, RoutedEventArgs e)    //左亮
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x0C, 0x73 });
            }
            else
            {
                if (Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x01, 0x73 }))
                {
                    ChangeLNowStatus(SelectPic("3"), "左亮");
                }
            }
        }

        private void Right_Shine_Click(object sender, RoutedEventArgs e)    //右亮
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x0C, 0x76 });
            }
            else
            {
                if (Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x01, 0x76 }))
                {
                    ChangeLNowStatus(SelectPic("4"), "右亮");
                }
            }
        }

        private void Shan_Click(object sender, RoutedEventArgs e)   //闪
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x07, 0x00 });
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x02, 0x01 });
            }
        }

        private void Shan_Before_Click(object sender, RoutedEventArgs e)  //闪前
        {
            if (_status == "配电箱")
            {
                Instruction.P_AllLightsStatues(code[0].Split('_')[1], new byte[] { 0x08, 0x00 });
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x02, 0x00 });
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)   //替换灯码/配电箱码确认按钮
        {
            if (_status == "配电箱")
            {
                string command = "update Pei set Name='_" + nowCode.Text + "' where Name='" + code[0] + "'";  //更新数据库
                SqlHelper.ExecuteNonQuery(command);
                string content = "Id int identity(1,1) primary key,Name varchar(50),IdCode varchar(50),LogicCode varchar(50),LStatus varchar(10),Bright varchar(10),Position varchar(50),PrePlanLeft varchar(50),PrePlanRight varchar(50)";  //为每一个配电箱创建一个专用的表
                SqlHelper.CreateTable("LightControl", content, "_" + nowCode.Text);
                ((Button)((MainWindow)this.Owner).MainGrid.FindName("FireWorkBack")).RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                command = "insert into  _" + nowCode.Text + "  " + "(Name,IdCode,LogicCode,LStatus,Bright,Position,PrePlanLeft,PrePlanRight) select Name,IdCode,LogicCode,LStatus,Bright,Position,PrePlanLeft,PrePlanRight from " + code[0] + " ;";
                SqlHelper.ExecuteNonQuery(command);
                command = "drop table " + code[0] + "";
                SqlHelper.ExecuteNonQuery(command);
                lightReplace.Visibility = Visibility.Collapsed;
                btnGrid.Visibility = Visibility.Visible;
            }
            else
            {
                string command = "update " + code[1] + " " + "set Name='" + nowCode.Text + "' where Name='" + code[0] + "'";
                SqlHelper.ExecuteNonQuery(command);
            }
        }

        private void IniAllBrightChecked()  //修改指定灯状态为全亮
        {
            if (_status == "配电箱")
            {
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x03, 0x77 });
            }
        }

        private void IniAllOffChecked()//全灭
        {
            if (_status == "配电箱")
            {
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x03, 0x70 });
            }
        }

        private void IniRightBrightChecked()  //右亮
        {
            if (_status == "配电箱")
            {
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x03, 0x11 });
            }
        }

        private void IniLeftBrightChecked()   //左亮
        {
            if (_status == "配电箱")
            {
            }
            else
            {
                Instruction.L_SetSelectedLightStatus(code[0].Split('_')[1], new byte[] { 0x03, 0x44 });
            }
        }

        private void full_Click(object sender, RoutedEventArgs e)
        {
            btnGrid.Visibility = Visibility.Collapsed;
            Info.Visibility = Visibility.Visible;
            DataTable dt = SqlHelper.ExecuteDataTable("select * from " + code[1] + " where Name='" + code[0] + "'");
            inWhichPei.Content = code[1];
            location.Content = dt.Rows[0]["Position"];
            lightCode.Content = code[0];
            iniStatus.Content = dt.Rows[0]["IniStatus"];

        }

        private void emergency_Click(object sender, RoutedEventArgs e)
        {

        }


        public void ChangeLNowStatus(string path, string lStatus)  //改变灯的状态
        {
            foreach (Button item in ((MainWindow)this.Owner).addPanel.Children.OfType<Button>())
            {
                if (!item.Content.ToString().Contains("添加灯具"))
                {
                    if (((TextBlock)item.Content).Text == code[0])
                    {
                        item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(path)), Stretch = Stretch.Uniform };
                        SqlHelper.ExecuteNonQuery("update " + code[1] + " set Status='" + lStatus + "' where Name='" + code[0] + "' ");
                    }
                }
            }
        }



        public string SelectPic(string status)
        {
            string flag = code[0].Split('_')[1].ToCharArray()[0].ToString() + status;   //1 全亮 ，2 全灭，3 左亮 ，4 右亮，
            string pic = "pack://application:,,,/Resources/配电箱黄.png";
            switch (code[0].Split('_')[1].ToCharArray()[0].ToString())    //根据高位划分灯具类型并添加不同图片显示
            {
                //case "1": { item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")), Stretch = Stretch.Uniform }; break; }
                case "21": { pic = "pack://application:,,,/Resources/标志灯双向绿.png"; break; }
                case "22": { pic = "pack://application:,,,/Resources/标志灯双向灭.png"; break; }
                case "23": { pic = "pack://application:,,,/Resources/标志灯左绿.png.png"; break; }
                case "24": { pic = "pack://application:,,,/Resources/标志灯右绿.png.png"; break; }
                //case "3": { item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/双头灯绿.png")), Stretch = Stretch.Uniform }; break; }
                //case "4": { item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/地埋灯双向绿.png")), Stretch = Stretch.Uniform }; break; }
                //case "5":
                //    {
                //        if (code[0].Split('_')[1].ToCharArray()[4].ToString() == "0")
                //        {
                //            item.Background = new ImageBrush
                //            {
                //                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        else
                //        {
                //            item.Background = new ImageBrush
                //            {
                //                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        break;
                //    }
                //case "7":
                //    {
                //        if (code[0].Split('_')[1].ToCharArray()[4].ToString() == "0")
                //        {
                //            item.Background = new ImageBrush
                //            {
                //                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        else
                //        {
                //            item.Background = new ImageBrush
                //            {
                //                ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")),
                //                Stretch = Stretch.Uniform
                //            };
                //        }
                //        break;
                //    }
                //case "8": { item.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/配电箱黄.png")), Stretch = Stretch.Uniform }; break; }

                default:
                    break;
            }
            return pic;
        }
    }
}
