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
    /// Changepd.xaml 的交互逻辑
    /// </summary>
    public partial class Changepd : Window
    {
        DataTable dt = SqlHelper.ExecuteDataTable("select * from Users");
        string mima ="";
        string textFoucs = "Oldpassword";
        public Changepd()
        {
            InitializeComponent();
            mima = dt.Rows[0]["Pwd"].ToString();
        }
        private void Btn0_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "0"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "0"; }
                    break;
                case "Repass":
                    { Repass.Password += "0"; }
                    break;
            }
        }
        private void Changepd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "1"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "1"; }
                    break;
                case "Repass":
                    { Repass.Password += "1"; }
                    break;
            }
        }
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "2"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "2"; }
                    break;
                case "Repass":
                    { Repass.Password += "2"; }
                    break;
            }
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "3"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "3"; }
                    break;
                case "Repass":
                    { Repass.Password += "3"; }
                    break;
            }
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "4"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "4"; }
                    break;
                case "Repass":
                    { Repass.Password += "4"; }
                    break;
            }
        }

        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "5"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "5"; }
                    break;
                case "Repass":
                    { Repass.Password += "5"; }
                    break;
            }
        }

        private void Btn6_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "6"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "6"; }
                    break;
                case "Repass":
                    { Repass.Password += "6"; }
                    break;
            }
        }

        private void Btn7_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "7"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "7"; }
                    break;
                case "Repass":
                    { Repass.Password += "7"; }
                    break;
            }
        }

        private void Btn8_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "8"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "8"; }
                    break;
                case "Repass":
                    { Repass.Password += "8"; }
                    break;
            }
        }

        private void Btn9_Click(object sender, RoutedEventArgs e)
        {
            switch (textFoucs)
            {
                case "Oldpassword":
                    { Oldpassword.Password += "9"; }
                    break;
                case "Newpassword":
                    { Newpassword.Password += "9"; }
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Oldpassword.Password != mima)
            {
                MessageBox.Show("原密码错误");
            }
            else if (Newpassword.Password != Repass.Password)
            {
                MessageBox.Show("两次密码输入不一样");
            }
            else
            {
                MessageBox.Show("ok");
                SqlHelper.ExecuteNonQuery("update Users set Pwd='" + Newpassword.Password + "' where Pwd='" + Oldpassword.Password + "'");
                this.Close();
            }
        }

        private void Oldpassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((PasswordBox)sender).Name == "Oldpassword")
            {
                textFoucs = "Oldpassword";
            }
            else if (((PasswordBox)sender).Name == "Newpassword")
            {
                textFoucs = "Newpassword";
            }
            else if (((PasswordBox)sender).Name == "Repass")
            {
                textFoucs = "Repass";
            }

        }
        private void Ok_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
