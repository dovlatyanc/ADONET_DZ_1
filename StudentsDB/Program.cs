using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentsDB
{
    internal class Program
    {
        SqlConnection connection = null;
        SqlDataReader reader = null;


        public Program()
        {

            string connectionString = @"Data Source=LAPTOP-5BDQ3HL0;Initial Catalog=StudentGradesDataBase;Integrated Security=True;Connect Timeout=30;";
            Console.WriteLine("Введите '1' для подключения к базе данных и '0' для выхода.");
            string input = Console.ReadLine();
            if (input == "1")
            {
                try
                {
                    using (connection = new SqlConnection(connectionString))
                    {

                        // Подключаемся к базе данных
                        connection.Open();
                        Console.WriteLine("Подключение к базе данных успешно!");
                        Console.WriteLine();
                        Thread.Sleep(1000);
                        SelectTable();

                        while (true)
                        {
                            Console.WriteLine("Для продолжения введите любую клавишу...");
                            Console.ReadKey();
                            Console.Clear();
                            Console.WriteLine("Если вы хотите вывести ФИО - введите '1'");
                            Console.WriteLine("Если вы хотите вывести средние баллы - введите '2'");
                            Console.WriteLine("Если вы хотите показать ФИО всех студентов с минимальной оценкой, больше, чем указанная - введите '3'");
                            Console.WriteLine("Если вы хотите показать название всех предметов с минимальными средними оценками. Названия предметов должны быть уникальными. - введите '4'");
                            Console.WriteLine("Если вы хотите получить среднюю минимальную оценку - введите '5'");
                            Console.WriteLine("Если вы хотите получить среднюю максимальную оценку - введите '6'");
                            Console.WriteLine("Если вы хотите вывести количество оутсайдеров по математике  - введите '7'");
                            Console.WriteLine("Если вы хотите вывести количество лучших студентов по математике  - введите '8'");
                            Console.WriteLine("Если вы хотите вывести количество студентов по группам  - введите '9'");
                            Console.WriteLine("Если вы хотите вывести cреднюю оценку по группам  - введите '10'");
                            Console.WriteLine("Если вы хотите выйти  - введите '0'");
                            string inputString = Console.ReadLine();
                            if (inputString == "1")
                            {
                                Console.Clear();
                                SelectNameStudents();
                            }
                            else if (inputString == "2")
                            {
                                Console.Clear();
                                SelectScore();
                            }
                            else if (inputString == "3")
                            {
                                Console.Clear();
                                Console.WriteLine("Введите средний балл");
                                Decimal.TryParse(Console.ReadLine(), out decimal score);
                                SelectNameStudents(score);
                            }
                            else if (inputString == "4")
                            {
                                Console.Clear();
                                SelectSubject();

                            }
                            else if (inputString == "5")
                            {
                                Console.Clear();
                                GetMinAverageScore();
                                Console.WriteLine();
                            }
                            else if (inputString == "6")
                            {
                                Console.Clear();
                                GetMaxAverageScore();
                                Console.WriteLine();
                            }
                            else if (inputString == "7")
                            {
                                Console.Clear();
                                GetCountStudentsBadMath();
                                Console.WriteLine();
                            }
                            else if (inputString == "8")
                            {
                                Console.Clear();
                                GetCountStudentsBadMath();
                                Console.WriteLine();
                            }
                            else if (inputString == "9")
                            {
                                Console.Clear();
                                GetCountStudentsInGroup();
                                Console.WriteLine();
                            }
                            else if (inputString == "10")
                            {
                                Console.Clear();
                                GetAverageScoreInGroup();
                                Console.WriteLine();
                            }
                            else if (inputString == "0")
                            {
                                Console.WriteLine("Выход из приложения...");
                                Thread.Sleep(500);
                                Process.GetCurrentProcess().Kill();
                            }
                            else { Console.WriteLine("Неверный ввод. Пожалуйста, введите число от 0 до 10 "); }
                        }

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Ошибка при подключении к базе данных: {ex.Message}");
                }
            }
            else if (input == "0")
            {
                Console.WriteLine("Выход из приложения...");
            }
            else
            {
                Console.WriteLine("Неверный ввод. Пожалуйста, введите '1' или '0'.");
            }
        }


        static void Main(string[] args)
        {

            //Console.WriteLine("Введите имя студента:");
            //string FullName = Console.ReadLine();

            //Console.WriteLine("Введите имя группы:");
            //string GroupName = Console.ReadLine();

            //Console.WriteLine("Введите среднюю оценку ");
            //if (Double.TryParse(Console.ReadLine(), out double averageScore))
            //{
            //}
            //else
            //{
            //    averageScore = 0;
            //}
            //Console.WriteLine("Введите название предмета с минимальной оценкой ");
            //string MinSubjectName = Console.ReadLine();
            //Console.WriteLine("Введите название предмета с максимальной оценкой ");
            //string MaxSubjectName = Console.ReadLine();

            Program program = new Program();
            // program.Insert(FullName,GroupName,averageScore, MinSubjectName, MaxSubjectName);



            Console.ReadKey();

        }

        public void Insert(string StudentName, string GroupName, double AverageScore, string MinSubjectName, string MaxSubjectName)
        {
            try
            {
                connection.Open();
                string insertStr = $"INSERT INTO StudentGrades (StudentName, GroupName, AverageScore, MinSubjectName, MaxSubjectName) VALUES" +
                    $" ('{StudentName}', '{GroupName}', {AverageScore}, '{MinSubjectName}', '{MaxSubjectName}')";
                SqlCommand cmd = new SqlCommand(insertStr, connection);
                cmd.ExecuteNonQuery();

                //ExecuteReader() Select
                //ExecuteScalar() AVG() SUM()
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public void SelectTable()
        {
            string sqlSelect = "select * from StudentGrades";
            try
            {

                //connection.Open();
                SqlCommand cmd = new SqlCommand(sqlSelect, connection);
                reader = cmd.ExecuteReader();
                int line = 0;
                while (reader.Read())
                {
                    //Шапка
                    if (line == 0)
                    {
                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetName(i).ToString(),-30}");
                        }
                    }
                    Console.WriteLine();
                    line++;
                    //Console.WriteLine(reader[1] + " " + reader[2]);


                    string col1 = (string)reader["StudentName"];
                    string col2 = (string)reader["GroupName"];
                    decimal col3 = (decimal)reader["AverageScore"];
                    string col4 = (string)reader["MinSubjectName"];
                    string col5 = (string)reader["MaxSubjectName"];

                    Console.WriteLine($"{col1,-30}{col2,-30}{col3,-30}{col4,-30}{col5,-30}");
                    // Console.WriteLine(reader["StudentName"] + " " + reader["GroupName"] + " " + reader["AverageScore"] + " " + reader["MinSubjectName"] + " " + reader["MaxSubjectName"]);
                }
                Console.WriteLine("\nОбработано строк: " + line.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }

        }

        public void SelectNameStudents()
        {
            string sqlSelect = "select StudentName from StudentGrades";
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlSelect, connection);
                reader = cmd.ExecuteReader();
                int line = 0;
                while (reader.Read())
                {
                    //Шапка
                    if (line == 0)
                    {
                        Console.Write($"ФИО Студента");

                    }
                    Console.WriteLine();
                    line++;

                    string col1 = (string)reader["StudentName"];


                    Console.WriteLine($"{col1,-30}");

                }
                Console.WriteLine("\nОбработано строк: " + line.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }

        }
        public void SelectNameStudents(decimal score)
        {
            string sqlSelect = $"SELECT StudentName FROM StudentGrades WHERE AverageScore > {score};";
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlSelect, connection);
                reader = cmd.ExecuteReader();
                int line = 0;
                while (reader.Read())
                {
                    //Шапка
                    if (line == 0)
                    {
                        Console.Write($"ФИО Студента");

                    }
                    Console.WriteLine();
                    line++;

                    string col1 = (string)reader["StudentName"];


                    Console.WriteLine($"{col1,-30}");

                }
                Console.WriteLine("\nОбработано строк: " + line.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }

        }

        public void SelectScore()
        {
            string sqlSelect = "select AverageScore from StudentGrades";
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlSelect, connection);
                reader = cmd.ExecuteReader();
                int line = 0;
                while (reader.Read())
                {
                    //Шапка
                    if (line == 0)
                    {
                        Console.Write($"Средняя оценка:");

                    }
                    Console.WriteLine();
                    line++;

                    decimal col1 = (decimal)reader["AverageScore"];


                    Console.WriteLine($"{col1,-30}");

                }
                Console.WriteLine("\nОбработано строк: " + line.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }

        }
        public void SelectSubject()
        {
            string sqlSelect = "SELECT DISTINCT MinSubjectName FROM StudentGrades " +
                "WHERE AverageScore IN (SELECT MIN(AverageScore)   FROM StudentGrades  GROUP BY MinSubjectName);";
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlSelect, connection);
                reader = cmd.ExecuteReader();
                int line = 0;
                while (reader.Read())
                {
                    //Шапка
                    if (line == 0)
                    {
                        Console.Write($"Предмет:");

                    }
                    Console.WriteLine();
                    line++;

                    string col1 = (string)reader["MinSubjectName"];


                    Console.WriteLine($"{col1,-30}");

                }
                Console.WriteLine("\nОбработано строк: " + line.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        public void GetMinAverageScore()
        {
            try
            {
                connection.Open();
                string query = "SELECT MIN(AverageScore) AS MinAverageScore FROM StudentGrades";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Выполняем запрос и получаем минимальную среднюю оценку
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        double minAverageScore = Convert.ToDouble(result);
                        Console.WriteLine($"Минимальная средняя оценка: {minAverageScore}");
                    }
                    else
                    {
                        Console.WriteLine("Нет доступных оценок.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        public void GetMaxAverageScore()
        {
            try
            {
                connection.Open();
                string query = "SELECT MAX(AverageScore) AS MaxAverageScore FROM StudentGrades";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Выполняем запрос и получаем минимальную среднюю оценку
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        double maxAverageScore = Convert.ToDouble(result);
                        Console.WriteLine($"Максимальная средняя оценка: {maxAverageScore}");
                    }
                    else
                    {
                        Console.WriteLine("Нет доступных оценок.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        public void GetCountStudentsBadMath()
        {
            try
            {
                connection.Open();
                string query = "SELECT COUNT(ID)" +
                    "FROM StudentGrades" +
                    "WHERE AverageScore = (" +
                    "SELECT MIN(AverageScore) " +
                    "FROM StudentGrades" +
                    "WHERE MinSubjectName = 'Математика'" +
                    ");";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        int Count = Convert.ToInt32(result);
                        Console.WriteLine($"Количество студентов: {Count}");
                    }
                    else
                    {
                        Console.WriteLine("Нет данных");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }


        public void GetCountStudentsBestMath()
        {
            try
            {
                connection.Open();
                string query = "SELECT COUNT(ID) " +
                "FROM StudentGrades " +
                "WHERE AverageScore = (" +
                "SELECT MAX(AverageScore) " +
                "FROM StudentGrades " +
                "WHERE MaxSubjectName = 'Математика'" +
                ");";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        int Count = Convert.ToInt32(result);
                        Console.WriteLine($"Количество студентов: {Count}");
                    }
                    else
                    {
                        Console.WriteLine("Нет данных");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public void GetCountStudentsInGroup()
        {

            string query = "SELECT GroupName, COUNT(StudentName) AS NumberOfStudents FROM StudentGrades GROUP BY GroupName;";


            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                int line = 0;
                while (reader.Read())
                {
                    //Header
                    if (line == 0)
                    {
                        Console.WriteLine($"{"Group Name",-15} {"Number of Students",-15}");
                    }
                    Console.WriteLine();
                    line++;

                    string col1 = (string)reader["GroupName"];
                    int col2 = (int)reader["NumberOfStudents"];
                    Console.WriteLine($"{col1,-15} {col2,-15}");
                }
                Console.WriteLine("\nProcessed rows: " + line.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }

        }
        public void GetAverageScoreInGroup()
        {

            string query = "SELECT GroupName, AVG(AverageScore) AS AverageGroupScore FROM StudentGrades GROUP BY GroupName;";


            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                int line = 0;
                while (reader.Read())
                {
                    //Header
                    if (line == 0)
                    {
                        Console.WriteLine($"{"Group Name",-15} {"AverageGroupScore",-15}");
                    }
                    Console.WriteLine();
                    line++;

                    string col1 = (string)reader["GroupName"];
                    decimal col2 = (decimal)reader["AverageGroupScore"];
                    Console.WriteLine($"{col1,-15} {col2,-15}");
                }
                Console.WriteLine("\nProcessed rows: " + line.ToString());
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }

        }
    }
}




