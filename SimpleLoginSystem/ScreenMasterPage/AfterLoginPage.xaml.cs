using ArkaPRP83.Core.Helper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace SimpleLoginSystem
{
    public partial class AfterLoginPage : Window
    {
        //private static readonly string connectionString = "Data Source=DESKTOP-QI77AJ8\\SQLEXPRESS;Initial Catalog=ArkaPRP83_UserAudit;User ID=sa;Password=Test#123";
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
        public AfterLoginPage()
        {
            InitializeComponent();
            LoadScreenMasterData();
        }
        private void LoadScreenMasterData()
        {
            DataTable dt = SqlHelper.ExecuteDataTable(connectionString, CommandType.StoredProcedure, "GetScreenMaster");
            if (dt != null)
            {
                DataGridMaster.ItemsSource = dt.DefaultView;
            }
            else
            {
                MessageBox.Show("Failed to load data.");
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AddFormMasterPage addFormMasterPage = new AddFormMasterPage();
            addFormMasterPage.ShowDialog();
            this.Show();
            if (addFormMasterPage.IsUpdated)
            {
                LoadScreenMasterData();
            }
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = DataGridMaster.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }
            int id = Convert.ToInt32(row["ID"]);
            string name = row["Name"].ToString();
            bool isActive = Convert.ToBoolean(row["IsActive"]);
            bool isDeleted = Convert.ToBoolean(row["IsDeleted"]);
            this.Hide();
            UpdateFormMasterPage updatePage = new UpdateFormMasterPage(id, name, isActive, isDeleted);
            updatePage.ShowDialog();
            this.Show();
            if (updatePage.IsUpdated)
            {
                LoadScreenMasterData();
            }
        }
        private void SoftDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = DataGridMaster.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Please select a record to disable");
                return;
            }
            int id = Convert.ToInt32(row["ID"]);
            string name = row["Name"].ToString();
            bool isDeleted = Convert.ToBoolean(row["IsDeleted"]);
            int isActive = 0;
            if (isDeleted)
            {
                MessageBox.Show($"{name} is already disabled", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to disable {name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@ID", id),
                    new SqlParameter("@IsDeleted", 1),
                    new SqlParameter("@IsActive",isActive)
                };
                try
                {
                    int rowAffected = SqlHelper.ExecuteNonQuery(connectionString,"SoftDeleteScreenMaster",sqlParameters);
                    if (rowAffected > 0)
                    {
                        MessageBox.Show(
                            $"{name} has been successfully disabled.",
                            "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        LoadScreenMasterData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to disable {name}. Error: {ex.Message}","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenMasterData();
        }
    }
}
