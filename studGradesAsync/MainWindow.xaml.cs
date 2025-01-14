using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;


//using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace studGradesAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static DbConnection connection = null;
        DbProviderFactory? fact = null;
        string providerName = "";
        string connString = "";


        private string connectionString = "";
        public MainWindow()
        {
            InitializeComponent();

            connection = new SqlConnection();
            connectionString = ConfigurationManager.ConnectionStrings["StudGrades"].ConnectionString;
            connection.ConnectionString = connectionString;

            comboBox.Items.Add("Отображение всей информации из таблицы со студентами и оценками;");
            comboBox.Items.Add("Отображение ФИО всех студентов;");
            comboBox.Items.Add("Отображение всех средних оценок;");
            comboBox.Items.Add("Показать ФИО всех студентов с минимальной оценкой, больше, чем 3,0;");
            comboBox.Items.Add("Показать название всех предметов с минимальными средними оценками.");
            comboBox.Items.Add("Показать минимальную среднюю оценку;");
            comboBox.Items.Add("Показать максимальную среднюю оценку;");
            comboBox.Items.Add("Показать количество студентов, у которых минимальная средняя оценка по математике;");
            comboBox.Items.Add("Показать количество студентов, у которых максимальная средняя оценка по математике;");
            comboBox.Items.Add("Показать количество студентов в каждой группе;");
            comboBox.Items.Add("Показать среднюю оценку по группе.");

        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.OpenAsync();
                MessageBox.Show("Успешно подключено к базе данных \"Оценки Студентов!\"");
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Непредвиденная ошибка: {ex.Message}");
            }
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                connection.Close();
                MessageBox.Show("Успешно отключено от базы данных \"Оценки Студентов\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отключения: {ex.Message}");
            }
        }

        private async void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlQuery = GetSqlQuery();
            try
            {

                using SqlCommand cmd = new SqlCommand(sqlQuery, (SqlConnection)connection);
                //{

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                //    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                //    {
                DataTable table = new DataTable();
                //        adapter.Fill(dataTable);  

                //    }
                //}
                using DbDataReader reader = await cmd.ExecuteReaderAsync();

                int line = 0;
                do
                {
                    while (await reader.ReadAsync())
                    {
                        if (line == 0)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                table.Columns.Add(reader.GetName(i));
                            }
                            line++;
                        }
                        DataRow row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = await reader.GetFieldValueAsync<Object>(i);
                        }
                        table.Rows.Add(row);
                    }
                } while (reader.NextResult());
                stopwatch.Stop();
                timeQuery.Content = $"Query execution time: {stopwatch.ElapsedMilliseconds} milliseconds";
                dataGrid.ItemsSource = table.AsDataView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private string GetSqlQuery()
        {
            string Query = "";


            switch (comboBox.SelectedIndex)
            {
                case 0:
                    Query = "select * from StudentGrades";
                    break;
                case 1:
                    Query = "select StudentName from StudentGrades";
                    break;
                case 2:
                    Query = "select AverageScore from StudentGrades";
                    break;
                case 3:
                    Query = "SELECT StudentName FROM StudentGrades WHERE AverageScore > 3.0;";
                    break;
                case 4:
                    Query = "SELECT DISTINCT MinSubjectName FROM StudentGrades  WHERE AverageScore IN (SELECT MIN(AverageScore)   FROM StudentGrades  GROUP BY MinSubjectName);";
                    break;
                case 5:
                    Query = "SELECT MIN(AverageScore) AS MinAverageScore FROM StudentGrades";
                    break;
                case 6:
                    Query = "SELECT MAX(AverageScore) AS MaxAverageScore FROM StudentGrades";
                    break;
                case 7:
                    Query = "SELECT COUNT(ID) FROM StudentGrades WHERE AverageScore = (SELECT MIN(AverageScore)FROM StudentGrades WHERE MinSubjectName = 'Математика');";
                    break;
                case 8:
                    Query = "SELECT COUNT(ID) FROM StudentGrades WHERE AverageScore = (SELECT MAX(AverageScore)FROM StudentGrades WHERE MaxSubjectName = 'Математика');";
                    break;
                case 9:
                    Query = "SELECT GroupName, COUNT(StudentName) AS NumberOfStudents FROM StudentGrades GROUP BY GroupName;";
                    break;
                case 10:
                    Query = "SELECT GroupName, AVG(AverageScore) AS AverageGroupScore FROM StudentGrades GROUP BY GroupName;";
                    break;


                default:
                    break;
            }
            return Query;
        }

        private void GetAllProviders_Click(object sender, RoutedEventArgs e)
        {
            DataTable table = DbProviderFactories.GetFactoryClasses();//Показывает все классы к которым можно подключить нпример MsSql MySql Acces 
            dataGrid.ItemsSource = table.AsDataView();
            //comboBox1.Items.Clear();

            Console.WriteLine(table.Columns.Count);
            Console.WriteLine(table.Rows.Count);

            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine(row.ToString());
                comboBox1.Items.Add(row["InvariantName"]);

            }
        }
        static string GetConnectionStringByProvider(string providerName)
        {
            string returnValue = "";
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                    {
                        returnValue = cs.ConnectionString;
                        break;
                    }
                }
            }
            return returnValue;

        }
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fact = DbProviderFactories.GetFactory(comboBox1.SelectedItem.ToString());
            connection = fact.CreateConnection();
            providerName = GetConnectionStringByProvider(comboBox1.SelectedItem.ToString());
            connString = providerName;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new DeleteWindow();
            window.ShowDialog();
        }
    }
}
