using System.Windows;
using System.Data.SqlClient;
using System;
using System.Data;
using ArkaPRP83.Core.Helper;
using System.Configuration;

namespace SimpleLoginSystem
{
    public partial class MainWindow : Window
    {
        private static readonly string connectionString = "Data Source=DESKTOP-QI77AJ8\\SQLEXPRESS;Initial Catalog=ArkaPRP83_UserAudit;User ID=sa;Password=Test#123";
        //private string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string username = UsernameTextBox.Text.Trim();
            string username = "SuperAdmin123";
            string password = "SuperAdmin@123";
            //string password = PasswordBox.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }
            string encryptedPassword = PasswordHelper.Encrypt(password);
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                   new SqlParameter("@UserName",username),
                   new SqlParameter("@Password",encryptedPassword)
                };
                object result = SqlHelper.ExecuteScalar(connectionString,CommandType.StoredProcedure,"CheckNewMyUserLogin",sqlParameters);
                int count = Convert.ToInt32(result);
                if (count > 0)
                {
                    SwitchingWindow switchingWindow = new SwitchingWindow();
                    switchingWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to Login");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message);
            }
        }
        private void UsernameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Placeholder1.Visibility = Visibility.Hidden;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Placeholder2.Visibility = Visibility.Hidden;
        }
        private void Placeholder2_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PasswordBox.Focus();
        }
        private void Placeholder1_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UsernameTextBox.Focus();
        }
    }
}
