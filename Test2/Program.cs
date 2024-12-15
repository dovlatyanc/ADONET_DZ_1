using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;




namespace Test2
{
    //DbConnection - подключение к бд
    //DbCommand - запросы(команды) для бд
    //DbDataReader - запрос select
    //DbAdapter

    //DataTable
    //DataSet
    //Data Source =(localdb)\v11.0; Initial Catalog = Library;Integrated Securoty =SSPI
    //Data Source =(localdb)\v11.0; Initial Catalog = Library; User Id = admin; Password = 1234567890
    internal class Program
    {
        SqlConnection connection = null;
        SqlDataReader reader = null;

        public Program()
        {
            connection = new SqlConnection();
           // connection.ConnectionString = @"Data Source=LAPTOP-5BDQ3HL0;Initial Catalog=library2;Integrated Security=True;Connect Timeout=30;";
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
        }


        static void Main(string[] args)
        {


            //conn = new SqlConnection(@"Data Source=DESKTOP-MGC0MB3;Initial Catalog=Library2;Integrated Security=True;Connect Timeout=30;");
            //DbCommand

            Program program = new Program();
            program.Insert();
            program.SelectAuthors();
           Console.ReadKey();

        }

        public void Insert()
        {
            try
            {
                connection.Open();
                string insertStr = "INSERT INTO Authors(FirstName,LastName) VALUES('Roger','Ivanov')";
                SqlCommand cmd = new SqlCommand(insertStr, connection);
                cmd.ExecuteNonQuery();
                //ExecuteReader() Select
                //ExecuteScalar() AVG() SUM()
            }
            //FieldCount - узнать количество полей 
            //GetName(index)
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

        public void SelectAuthors()
        {
            string sqlSelect = "select * from Authors";
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
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetName(i).ToString() + " ");
                        }
                    }
                    Console.WriteLine();
                    line++;
                    //Console.WriteLine(reader[1] + " " + reader[2]);
                    Console.WriteLine(reader["FirstName"] + " " + reader["LastName"]);
                }
                Console.WriteLine("Обработано строк: " + line.ToString());
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
