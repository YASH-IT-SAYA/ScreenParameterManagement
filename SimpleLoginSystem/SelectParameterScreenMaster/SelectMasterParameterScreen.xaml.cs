using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using ArkaPRP83.Core.Helper;
using System.Configuration;

namespace SimpleLoginSystem
{
    public partial class SelectMasterParameterScreen : Window
    {
        //private static readonly string connectionString = "Data Source=DESKTOP-QI77AJ8\\SQLEXPRESS;Initial Catalog=ArkaPRP83_UserAudit;User ID=sa;Password=Test#123";
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
        int screenId;
        public SelectMasterParameterScreen()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadScreenNames();
        }
        private void LoadScreenNames()
        {
            DataTable dt = SqlHelper.ExecuteDataTable(connectionString, CommandType.StoredProcedure, "GetActiveScreenMasterList");
            if (dt != null)
            {
                ComboBox1.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    ComboBoxItem item = new ComboBoxItem
                    {
                        Content = row["Name"].ToString().Trim(),
                        Tag = row["Id"]
                    };
                    ComboBox1.Items.Add(item);
                    ComboBox1.SelectedIndex = 0;
                }
            }
        }
        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox1.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                screenId = Convert.ToInt32(selectedItem.Tag);
                LoadParameters(screenId);
            }
        }
        private void LoadParameters(int screenId)
        {
            SqlParameter[] parameters = { new SqlParameter("@ScreenMasterId", screenId) };
            DataTable dt = SqlHelper.ExecuteDataTable(connectionString, CommandType.StoredProcedure, "GetScreenParametersList", parameters);
            if (dt != null)
            {
                ParameterDataGrid.ItemsSource = dt.DefaultView;
            }
        }
        private void Add_parameter_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AddSelectMasterParameterPage addSelectMasterParameterPage = new AddSelectMasterParameterPage();
            addSelectMasterParameterPage.ShowDialog();
            this.Show();
            if (addSelectMasterParameterPage.IsUpdated)
            {
                LoadScreenNames();
            }
        }
        private void Update_parameter_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = ParameterDataGrid.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Please Select the row you want to update");
                return;
            }
            int id = Convert.ToInt32(row["ParameterID"]);
            string parametername = row["ParameterName"].ToString();
            bool isActive = Convert.ToBoolean(row["IsActive"]);
            bool isDeleted = Convert.ToBoolean(row["IsDeleted"]);
            this.Hide();
            UpdateFormParameterMasterPage updatePage = new UpdateFormParameterMasterPage(id, parametername, isActive, isDeleted);
            updatePage.ShowDialog();
            this.Show();
            if (updatePage.IsUpdated)
            {
                LoadParameters(screenId);
            }
        }
        private void SoftDeleteparameter_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = ParameterDataGrid.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Please select the row");
                return;
            }
            int id = Convert.ToInt32(row["ParameterID"]);
            string parametername = row["ParameterName"].ToString();
            bool isDeleted = Convert.ToBoolean(row["IsDeleted"]);
            int isActive = 0;
            if (isDeleted)
            {
                MessageBox.Show($"{parametername} is already disabled", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to disable {parametername}", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@ID",id),
                    new SqlParameter("@IsDeleted",1),
                    new SqlParameter("@ISActive",isActive)
                };
                try
                {
                    int rowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, "SoftDeleteScreenParameter", sqlParameters);
                    if (rowAffected > 0)
                    {
                        MessageBox.Show($"{parametername} has been successfull disabled","Success",MessageBoxButton.OK,MessageBoxImage.Information);
                        LoadParameters(screenId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to update {parametername}. Error: {ex.Message}","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }
    }
}
