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

namespace SimpleLoginSystem
{
    public partial class SwitchingWindow : Window
    {
        public SwitchingWindow()
        {
            InitializeComponent();
        }
        private void SelectMasterPage_Click(object sender, RoutedEventArgs e)
        {
            AfterLoginPage selectmasterpage = new AfterLoginPage();
            selectmasterpage.ShowDialog();
        }
        private void SelectParameterMasterPage_Click(object sender, RoutedEventArgs e)
        {
            SelectMasterParameterScreen selectMasterParameterScreen = new SelectMasterParameterScreen();
            selectMasterParameterScreen.ShowDialog();
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
