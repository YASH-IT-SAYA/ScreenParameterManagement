using ArkaPRP83.Core.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace SimpleLoginSystem
{
    public partial class AddSelectMasterParameterPage : Window
    {
        public bool IsUpdated { get; private set; } = false;
        private static readonly string connectionString = "Data Source=DESKTOP-QI77AJ8\\SQLEXPRESS;Initial Catalog=ArkaPRP83_UserAudit;User ID=sa;Password=Test#123";
        public AddSelectMasterParameterPage()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenMasters();
        }
        private void LoadScreenMasters()
        {
            DataTable dt = SqlHelper.ExecuteDataTable(connectionString, CommandType.StoredProcedure, "GetActiveScreenMasterList");
            if (dt != null && dt.Rows.Count > 0)
            {
                ScreenMasterComboBox.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    ScreenMasterComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = row["Name"].ToString().Trim(),
                        Tag = row["Id"]
                    });
                }
                ScreenMasterComboBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No Screen Masters found or failed to load data.", "Load Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Save_parameter_Click(object sender, RoutedEventArgs e)
        {
            string name = ParameterNameTextBox.Text.Trim();
            int isActive = IsActiveYes.IsChecked == true ? 1 : 0;
            int isDeleted = 0;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter the Parameter Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }
            if (!(ScreenMasterComboBox.SelectedItem is ComboBoxItem selectedItem))
            {
                MessageBox.Show("Please select a Screen Master.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(selectedItem.Tag?.ToString(), out int screenMasterId))
            {
                MessageBox.Show("Invalid Screen Master selected.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SqlParameter[] sqlParams = new SqlParameter[]
            {
            new SqlParameter("@ScreenMasterId", screenMasterId),
            new SqlParameter("@Name", name),
            new SqlParameter("@IsDeleted", isDeleted),
            new SqlParameter("@IsActive", isActive)
            };
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, "AddScreenParameter", sqlParams);
                bool success = rowsAffected > 0;
                if (success)
                {
                    MessageBox.Show("Parameter added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                    IsUpdated = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to insert parameter. Please try again.", "Insert Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while adding parameter: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ClearForm()
        {
            ParameterNameTextBox.Clear();
            IsActiveYes.IsChecked = false;
            IsActiveNo.IsChecked = false;
            //IsDeletedYes.IsChecked = false;
            //IsDeletedNo.IsChecked = false;
            ScreenMasterComboBox.SelectedIndex = -1;
        }
    }
}
