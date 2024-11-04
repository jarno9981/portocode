using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleSQLiteManager
{
    class Program
    {
        //jarno beerten 
        private static SQLiteConnection connection;
        private static string currentTable;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nSQLite Database Manager");
                Console.WriteLine("1. Create new database");
                Console.WriteLine("2. Open existing database");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateDatabase();
                        break;
                    case "2":
                        OpenDatabase();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void CreateDatabase()
        {
            Console.Write("Enter the path for the new database: ");
            string path = Console.ReadLine();
            SQLiteConnection.CreateFile(path);
            OpenDatabaseConnection(path);
            Console.WriteLine("Database created successfully!");
            ManageDatabase();
        }

        static void OpenDatabase()
        {
            Console.Write("Enter the path of the existing database: ");
            string path = Console.ReadLine();
            OpenDatabaseConnection(path);
            Console.WriteLine("Database opened successfully!");
            ManageDatabase();
        }

        static void OpenDatabaseConnection(string path)
        {
            connection = new SQLiteConnection($"Data Source={path};Version=3;");
            connection.Open();
        }

        static void ManageDatabase()
        {
            while (true)
            {
                Console.WriteLine("\nDatabase Management");
                Console.WriteLine("1. Create new table");
                Console.WriteLine("2. Select table");
                Console.WriteLine("3. Add column");
                Console.WriteLine("4. Add data");
                Console.WriteLine("5. View data");
                Console.WriteLine("6. Delete row");
                Console.WriteLine("7. Delete column");
                Console.WriteLine("8. Close database");
                Console.Write("Select an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateTable();
                        break;
                    case "2":
                        SelectTable();
                        break;
                    case "3":
                        AddColumn();
                        break;
                    case "4":
                        AddData();
                        break;
                    case "5":
                        ViewData();
                        break;
                    case "6":
                        DeleteRow();
                        break;
                    case "7":
                        DeleteColumn();
                        break;
                    case "8":
                        connection.Close();
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void CreateTable()
        {
            Console.Write("Enter the name for the new table: ");
            string tableName = Console.ReadLine();

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT)";
                cmd.ExecuteNonQuery();
            }

            currentTable = tableName;
            Console.WriteLine($"Table '{tableName}' created successfully!");
        }

        static void SelectTable()
        {
            List<string> tables = GetTables();

            if (tables.Count == 0)
            {
                Console.WriteLine("No tables found in the database.");
                return;
            }

            Console.WriteLine("Available tables:");
            for (int i = 0; i < tables.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tables[i]}");
            }

            Console.Write("Select a table number: ");
            if (int.TryParse(Console.ReadLine(), out int tableIndex) && tableIndex > 0 && tableIndex <= tables.Count)
            {
                currentTable = tables[tableIndex - 1];
                Console.WriteLine($"Selected table: {currentTable}");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        static List<string> GetTables()
        {
            List<string> tables = new List<string>();
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", connection))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add(reader["name"].ToString());
                }
            }
            return tables;
        }

        static void AddColumn()
        {
            if (string.IsNullOrEmpty(currentTable))
            {
                Console.WriteLine("Please select a table first.");
                return;
            }

            Console.Write("Enter the name for the new column: ");
            string columnName = Console.ReadLine();

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = $"ALTER TABLE {currentTable} ADD COLUMN {columnName} TEXT";
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine($"Column '{columnName}' added successfully to table '{currentTable}'!");
        }

        static void AddData()
        {
            if (string.IsNullOrEmpty(currentTable))
            {
                Console.WriteLine("Please select a table first.");
                return;
            }

            List<string> columns = GetColumns();
            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (string column in columns)
            {
                if (column != "Id")
                {
                    Console.Write($"Enter value for {column}: ");
                    string value = Console.ReadLine();
                    values[column] = value;
                }
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                string columnList = string.Join(", ", values.Keys);
                string paramList = string.Join(", ", values.Keys.Select(k => $"@{k}"));
                cmd.CommandText = $"INSERT INTO {currentTable} ({columnList}) VALUES ({paramList})";

                foreach (var kvp in values)
                {
                    cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                }

                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Data added successfully!");
        }

        static List<string> GetColumns()
        {
            List<string> columns = new List<string>();
            using (SQLiteCommand cmd = new SQLiteCommand($"PRAGMA table_info({currentTable})", connection))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    columns.Add(reader["name"].ToString());
                }
            }
            return columns;
        }

        static void ViewData()
        {
            if (string.IsNullOrEmpty(currentTable))
            {
                Console.WriteLine("Please select a table first.");
                return;
            }

            List<string> columns = GetColumns();
            Console.WriteLine(string.Join(" | ", columns));
            Console.WriteLine(new string('-', columns.Count * 15));

            using (SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM {currentTable}", connection))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    List<string> rowData = new List<string>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        rowData.Add(reader[i].ToString());
                    }
                    Console.WriteLine(string.Join(" | ", rowData));
                }
            }
        }

        static void DeleteRow()
        {
            if (string.IsNullOrEmpty(currentTable))
            {
                Console.WriteLine("Please select a table first.");
                return;
            }

            Console.Write("Enter the Id of the row to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = $"DELETE FROM {currentTable} WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Row with Id {id} deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"No row found with Id {id}.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Id. Please enter a number.");
            }
        }

        static void DeleteColumn()
        {
            if (string.IsNullOrEmpty(currentTable))
            {
                Console.WriteLine("Please select a table first.");
                return;
            }

            List<string> columns = GetColumns();
            Console.WriteLine("Available columns:");
            for (int i = 0; i < columns.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {columns[i]}");
            }

            Console.Write("Enter the number of the column to delete: ");
            if (int.TryParse(Console.ReadLine(), out int columnIndex) && columnIndex > 0 && columnIndex <= columns.Count)
            {
                string columnToDelete = columns[columnIndex - 1];

                if (columnToDelete == "Id")
                {
                    Console.WriteLine("Cannot delete the 'Id' column.");
                    return;
                }

                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    // Get all column names except the one to be deleted
                    List<string> remainingColumns = columns.Where(c => c != columnToDelete).ToList();
                    string columnList = string.Join(", ", remainingColumns);

                    // Create a new table without the selected column
                    cmd.CommandText = $"CREATE TABLE {currentTable}_new AS SELECT {columnList} FROM {currentTable}";
                    cmd.ExecuteNonQuery();

                    // Drop the old table
                    cmd.CommandText = $"DROP TABLE {currentTable}";
                    cmd.ExecuteNonQuery();

                    // Rename the new table to the original name
                    cmd.CommandText = $"ALTER TABLE {currentTable}_new RENAME TO {currentTable}";
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine($"Column '{columnToDelete}' deleted successfully from table '{currentTable}'!");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
    }
}
