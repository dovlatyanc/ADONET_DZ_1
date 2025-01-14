using Microsoft.Data.SqlClient;
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

namespace studGradesAsync
{
    /// <summary>
    /// Логика взаимодействия для DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        public DeleteWindow()
        {
            InitializeComponent();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlQuery = $"delete from StudentGrades where Id = @id";
                SqlCommand cmd = new SqlCommand(sqlQuery, (SqlConnection)MainWindow.connection);


                Int32.TryParse(delTB.Text, out int id);

                SqlParameter idParameter = new SqlParameter("@id", id);
                cmd.Parameters.Add(idParameter);

                int number = await cmd.ExecuteNonQueryAsync();
                MessageBox.Show($"Delete {number} rows!");
            }
            finally
            {
                Close();
            }


        }

    }
}
