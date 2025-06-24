using ArkaPRP83.Core.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace SimpleLoginSystem
{
    public partial class AddFormMasterPage : Window
    {
        public bool IsUpdated { get; private set; } = false;
        private static readonly string connectionString = "Data Source=DESKTOP-QI77AJ8\\SQLEXPRESS;Initial Catalog=ArkaPRP83_UserAudit;User ID=sa;Password=Test#123";
        public AddFormMasterPage()
        {
            InitializeComponent();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            bool isActive = IsActiveYes.IsChecked == true;
            bool isDeleted = false;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name");
                return;
            }
            SqlParameter[] sqlparams = new SqlParameter[]
            {
                new SqlParameter("@Name",name),
                new SqlParameter("@IsActive",isActive),
                new SqlParameter("@IsDeleted",isDeleted)
            };
            try
            {
                int rowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, "AddScreenMaster", sqlparams);
                bool success = rowAffected > 0;
                if (success)
                {
                    MessageBox.Show("MasterPage added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                    IsUpdated = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to insert Master Page. Please try again.", "Insert Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding parameter: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ClearForm()
        {
            NameTextBox.Clear();
            IsActiveYes.IsChecked = false;
            IsActiveNo.IsChecked = false;
        }
    }
}
