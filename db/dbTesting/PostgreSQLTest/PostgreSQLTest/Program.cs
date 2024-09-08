using System;
using Npgsql;

namespace PostgreSQLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=rafopostgre2024;Database=ims_test_db";

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                InsertUsers(conn, 1000); // Insert 1000 users as a test
            }

            Console.WriteLine("Data insertion completed.");
        }

        static void InsertUsers(NpgsqlConnection conn, int numberOfUsers)
        {
            using (var transaction = conn.BeginTransaction())
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;

                    for (int i = 0; i < numberOfUsers; i++)
                    {
                        cmd.CommandText = "INSERT INTO appuser (firstname, lastname, emailaddress, lastlogin, creation) VALUES (@firstname, @lastname, @EmailAddress, @LastLogin, @Creation)";
                        cmd.Parameters.AddWithValue("firstname", $"FirstName{i}");
                        cmd.Parameters.AddWithValue("lastname", $"LastName{i}");
                        cmd.Parameters.AddWithValue("EmailAddress", $"user{i}@example.com");
                        cmd.Parameters.AddWithValue("LastLogin", DateTime.Now);
                        cmd.Parameters.AddWithValue("Creation", DateTime.Now);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }
                transaction.Commit();
            }
        }
    }
}