using ArkaPRP83.Core.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public partial class UpdateFormParameterMasterPage : Window
    {
        private static readonly string connectionString = "Data Source=DESKTOP-QI77AJ8\\SQLEXPRESS;Initial Catalog=ArkaPRP83_UserAudit;User ID=sa;Password=Test#123";
        private int currentId;
        public bool IsUpdated { get; private set; } = false;
        public UpdateFormParameterMasterPage(int id, string parametername, bool isActive, bool isDeleted)
        {
            InitializeComponent();
            currentId = id;
            NameTextBox.Text = parametername;
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
                MessageBox.Show("Name Required");
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
            SqlParameter[] sqlParameter = new SqlParameter[]
            {
                new SqlParameter("@ID",currentId),
                new SqlParameter("@ParameterName",name),
                new SqlParameter("@IsActive",isActive),
                new SqlParameter("@IsDeleted",isDeleted)
            };
            try
            {
                int rowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, "UpdateScreenParameterMaster", sqlParameter);
                bool success = rowAffected > 0;
                if (success)
                {
                    MessageBox.Show("Parameter Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
