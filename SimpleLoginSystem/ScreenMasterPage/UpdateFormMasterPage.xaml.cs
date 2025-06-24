using ArkaPRP83.Core.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace SimpleLoginSystem
{
    public partial class UpdateFormMasterPage : Window
    {
        public bool IsUpdated { get; private set; } = false;
        private int currentId;
        private static readonly string connectionString = "Data Source=DESKTOP-QI77AJ8\\SQLEXPRESS;Initial Catalog=ArkaPRP83_UserAudit;User ID=sa;Password=Test#123";
        public UpdateFormMasterPage(int id, string name, bool isActive, bool isDeleted)
        {
            InitializeComponent();
            currentId = id;
            NameTextBox.Text = name;
            IsActiveYes.IsChecked = isActive;
            IsActiveNo.IsChecked = !isActive;
            IsDeletedYes.IsChecked = isDeleted;
            IsDeletedNo.IsChecked = !isDeleted;
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            int isActive = IsActiveYes.IsChecked == true ? 1 : 0;
            int isDeleted = IsDeletedYes.IsChecked == true ? 1 : 0;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name is required.");
                return;
            }
            if (isDeleted == 1)
            {
                MessageBox.Show("Cannot update this record. Please set 'IsDeleted' to 'No' before updating.",
                "Update Not Allowed",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
                return;
            }

            SqlParameter[] sqlparams = new SqlParameter[]
            {
                new SqlParameter("@ID", currentId),
                new SqlParameter("@Name", name),
                new SqlParameter("@IsActive", isActive),
                new SqlParameter("@IsDeleted", isDeleted)
            };
            try
            {
                int rowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, "UpdateScreenMaster", sqlparams);
                bool success = rowAffected > 0;
                if (success)
                {
                    MessageBox.Show("Page Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    IsUpdated = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update the page: {ex.Message}", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}