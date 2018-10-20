using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
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
    /// AddItems.xaml 的交互逻辑
    /// </summary>
    public partial class AddItems : Window
    {
        public AddItems()
        {
            InitializeComponent();
            RangeDis();
        }

        public AddItems(string comDis)  //这个是通信设置的构造方法
        {
            InitializeComponent();
            elecOrLightAddGrid.Visibility = Visibility.Collapsed;
            ConnectionSet.Visibility = Visibility.Visible;
            ComDis();
        }
        private string flag;   //状态标志
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (flag == "配电箱")
            {
                if (!String.IsNullOrEmpty(_addname.Text))
                {
                    if (Instruction.P_FindpeiCode(_addname.Text) == true)  //输入判断语句,看有没有找到
                    {
                        ButtonAdd(_addname.Text);
                        MessageBox.Show("成功！");
                        SqlHelper.ExecuteNonQuery("insert into Pei (Name) values ('_" + _addname.Text + "')");
                        string content = "Id int identity(1,1) primary key,Name varchar(50),IdCode varchar(50),LogicCode varchar(50),Status varchar(10),Bright varchar(10),Position varchar(50),PrePlanLeft varchar(50),PrePlanRight varchar(50),IniStatus varchar(20)";  //为每一个配电箱创建一个专用的表
                        SqlHelper.CreateTable("LightControl", content, "_" + _addname.Text);
                        elecOrLightAddGrid.Visibility = Visibility.Collapsed;
                        afterEleAdd.Visibility = Visibility.Visible;
                    }
                    else
                        MessageBox.Show("未找到配电箱");
                }
                else
                {
                    MessageBox.Show("请输入配电箱码！", "提示");
                }


            }
            else
            {
                if (!String.IsNullOrEmpty(_addname.Text))
                {
                
                    string lightHost = ((MainWindow)this.Owner)._lightHost.Text.Split('_')[1];
                    if (Instruction.P_AskSelectedLightCode(_addname.Text, lightHost))    //这个是找灯码的，上面那个是配电箱码的
                    {
                        ButtonAdd(_addname.Text);
                        string command = "insert into " +"_"+ lightHost + " (Name) values ( '_" + _addname.Text + "')";
                        SqlHelper.ExecuteNonQuery(command);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("未找到指定灯码！", "提示");
                    }
                }
                else
                {
                    MessageBox.Show("请输入灯码", "提示");
                }
            }
            range.SelectedIndex = 0;
        }

        public void ButtonAdd(string name)   //这个是界面上添加配电箱和灯具的主要方法
        {
            Button bt = ((MainWindow)this.Owner).addPanel.FindName("addNew") as Button;
            ((MainWindow)this.Owner).addPanel.Children.Remove(bt);
            ((MainWindow)this.Owner).addPanel.UnregisterName("addNew");
            ((MainWindow)this.Owner).Add("_" + name);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)     //属于界面逻辑
        {
            flag = ((MainWindow)this.Owner)._addName.Text;   //更改窗体显示,这个flag很重要，标识着是灯具还是配电箱的新增
            if (flag != "配电箱")
            {
                windowTip.Content = "灯具码";
                windowTitle.Content = "添加灯具";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)   //开始搜索
        {
            //添加开始搜索的代码
            searchGrid.Visibility = Visibility.Visible;
            afterEleAdd.Visibility = Visibility.Collapsed;

            Thread thread = new Thread(new ThreadStart(ThreadUpdate));
            thread.Start();

        }


        public void ThreadUpdate()
        {
            Thread.Sleep(3000);
            searchGrid.Dispatcher.BeginInvoke(new Action (()=> 
            {
                SearchLights();
                searchGrid.Visibility = Visibility.Collapsed; }));
            afterSearch.Dispatcher.BeginInvoke(new Action(() => { afterSearch.Visibility = Visibility.Visible; }));
        }

       


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Activate();
        }

        private void AddRange_Click(object sender, RoutedEventArgs e)   //增加灯码搜索范围
        {
            Configuration ca = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ca.AppSettings.Settings.Add(Min1.Text + Min2.Text + Min3.Text + Min4.Text + Min5.Text + "001", Max1.Text + Max2.Text + Max3.Text + Max4.Text + Max5.Text + "050");
            ca.Save();
            ConfigurationManager.RefreshSection("appSettings");
            RangeDis();
        }

        public void RangeDis()   //灯码搜索范围显示
        {
            range.Items.Clear();
            Configuration ca = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            foreach (string key in ca.AppSettings.Settings.AllKeys)
            {
                if (key != "alarmPcBps" && key != "alarmPcCom" && key != "peiPcCom" && key != "pcBanPcCom")
                {
                    range.Items.Add(key + "-" + ca.AppSettings.Settings[key].Value);
                }
            }
        }

        private void DeleteRange_Click(object sender, RoutedEventArgs e)  //删除灯码搜索范围
        {
            try
            {
                string key = range.SelectedItem.ToString().Split('-')[0];
                Configuration ca = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ca.AppSettings.Settings.Remove(key);
                ca.Save();
                ConfigurationManager.RefreshSection("appSettings");
                RangeDis();
            }
            catch 
            {
                MessageBox.Show("请先选中要删除的范围!", "提示");
            }
        }

        public bool SearchLights()  //开始搜寻指定灯码
        {
            try
            {
                flag = "灯具";
                string[] min_max = range.SelectedItem.ToString().Split('-');
                for (int i = Convert.ToInt32(min_max[0]); i <=Convert.ToInt32(min_max[1]); i++)
                {
                    if (Instruction.P_AskSelectedLightCode(i.ToString(), _addname.Text) == true)
                    {
                        resultDis.Items.Add(i.ToString());
                        string lightHost = ((MainWindow)this.Owner)._lightHost.Text;
                        SqlHelper.ExecuteNonQuery("insert into  _" + _addname.Text + " (Name) values ( '_" + i.ToString() + "')");  //找到后插入数据库
                    }
                }
                if (AllShine.IsChecked == true)
                {
                    Instruction.P_LightShine(_addname.Text);    //全亮
                }
                return true;
            }
            catch
            {
                MessageBox.Show("错误");
                return false;
            }
        }

        private void ManualAddB_Click(object sender, RoutedEventArgs e)  //手动添加灯具
        {
            if (Instruction.P_AskSelectedLightCode(ManuaAddLightT.Text,_addname.Text))    //这个是找灯码的，上面那个是配电箱码的
            {
                ButtonAdd(ManuaAddLightT.Text);
                string lightHost = ((MainWindow)this.Owner)._lightHost.Text;
                SqlHelper.ExecuteNonQuery("insert into " + _addname.Text + " (Name) values ( '_" + ManuaAddLightT.Text + "')");

            }
        }



    
        Configuration ca = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public void ComDis()   //通信设置的显示
        {
            windowTitle.Content = "通信设置";
            AlarmPcCom.Items.Clear();
            PcBanPcCom.Items.Clear();
            PeiPcCom.Items.Clear();
            for (int i = 0; i < PortHelper.portNames.Length; i++)
            {
                AlarmPcCom.Items.Add(PortHelper.portNames[i]);
                PcBanPcCom.Items.Add(PortHelper.portNames[i]);
                PeiPcCom.Items.Add(PortHelper.portNames[i]);
            }

            try
            { H2.Text = ca.AppSettings.Settings["alarmPcBps"].Value; }
            catch
            {
                H2.Text = "未设置";
                ca.AppSettings.Settings.Add("alarmPcBps", "未设置");
            }

            try
            { AlarmPcCom.Text = ca.AppSettings.Settings["alarmPcCom"].Value; }
            catch
            {
                AlarmPcCom.Text = "未设置";
                ca.AppSettings.Settings.Add("alarmPcCom", "未设置");
            }

            try
            {PeiPcCom.Text = ca.AppSettings.Settings["peiPcCom"].Value; }
            catch
            {
                PeiPcCom.Text = "未设置";
                ca.AppSettings.Settings.Add("peiPcCom", "未设置");
            }

            try
            { PcBanPcCom.Text = ca.AppSettings.Settings["pcBanPcCom"].Value; }
            catch
            {
                PcBanPcCom.Text = "未设置";
                ca.AppSettings.Settings.Add("pcBanPcCom", "未设置");
            }
            ca.Save();
            ConfigurationManager.RefreshSection("appSettings");


        }

        private void ConnectSetConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ca.AppSettings.Settings["alarmPcBps"].Value = H2.Text;
                ca.AppSettings.Settings["alarmPcCom"].Value = AlarmPcCom.Text.ToString();
                ca.AppSettings.Settings["peiPcCom"].Value = PeiPcCom.Text.ToString();
                ca.AppSettings.Settings["pcBanPcCom"].Value = PcBanPcCom.Text.ToString();
                ca.Save();
                ConfigurationManager.RefreshSection("appSettings");
                PortHelper.serialPort.Close();
                PortHelper.isOpen = false;
                PortHelper.OpenPort();
                MessageBox.Show("设置成功！", "提示");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ConnectionSet_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();

        }
        

       
    }
}
