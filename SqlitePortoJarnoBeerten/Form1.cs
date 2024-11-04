using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DBManager
{
    public partial class Form1 : Form
    {
        private string currentDbPath;
        private SQLiteConnection connection;
        private string currentTable;

        public Form1()
        {
            InitializeComponent();
            InitializeListView();
        }

        private void InitializeListView()
        {
            listViewData.View = View.Details;
            listViewData.FullRowSelect = true;
            listViewData.GridLines = true;
            listViewData.MultiSelect = false;
        }

        private void btnCreateDb_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "SQLite Database|*.db",
                Title = "Create a new SQLite Database"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenDatabase(saveFileDialog.FileName, true);
            }
        }

        private void btnOpenDb_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "SQLite Database|*.db",
                Title = "Open an existing SQLite Database"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenDatabase(openFileDialog.FileName, false);
            }
        }

        private void OpenDatabase(string path, bool isNew)
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                if (isNew)
                {
                    SQLiteConnection.CreateFile(path);
                }

                currentDbPath = path;
                connection = new SQLiteConnection($"Data Source={currentDbPath};Version=3;");
                connection.Open();

                if (isNew)
                {
                    CreateNewTable("DefaultTable");
                }

                RefreshTableList();

                if (cboTables.Items.Count > 0)
                {
                    cboTables.SelectedIndex = 0;
                    currentTable = cboTables.SelectedItem.ToString();
                    RefreshData();
                }

                MessageBox.Show(isNew ? "Database created successfully!" : "Database opened successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening database: {ex.Message}");
            }
        }

        private void RefreshTableList()
        {
            cboTables.Items.Clear();
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", connection))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cboTables.Items.Add(reader["name"].ToString());
                }
            }

            if (cboTables.Items.Count > 0)
            {
                cboTables.SelectedIndex = 0;
            }
        }

        private void cboTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTables.SelectedItem != null)
            {
                currentTable = cboTables.SelectedItem.ToString();
                RefreshData();
            }
        }

        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            if (connection == null)
            {
                MessageBox.Show("Please create or open a database first.");
                return;
            }

            string tableName = PromptForInput("Enter new table name:", "Create Table");
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                CreateNewTable(tableName);
                RefreshTableList();
            }
        }

        private void CreateNewTable(string tableName)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT)";
                cmd.ExecuteNonQuery();
            }
        }

        private void btnAddColumn_Click(object sender, EventArgs e)
        {
            if (connection == null || string.IsNullOrEmpty(currentTable))
            {
                MessageBox.Show("Please create or open a database and select a table first.");
                return;
            }

            string columnName = PromptForInput("Enter column name:", "Add Column");
            if (!string.IsNullOrWhiteSpace(columnName))
            {
                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = $"ALTER TABLE {currentTable} ADD COLUMN {columnName} TEXT";
                        cmd.ExecuteNonQuery();
                    }
                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding column: {ex.Message}");
                }
            }
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            if (connection == null || string.IsNullOrEmpty(currentTable) || listViewData.Columns.Count == 0)
            {
                MessageBox.Show("Please create or open a database, select a table, and add columns first.");
                return;
            }

            List<string> selectedColumns = SelectColumns();
            if (selectedColumns.Count == 0)
            {
                MessageBox.Show("No columns selected. Data not added.");
                return;
            }

            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (string column in selectedColumns)
            {
                string value = PromptForInput($"Enter value for {column}:", "Add Data");
                values[column] = value;
            }

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                string columns = string.Join(", ", values.Keys);
                string parameters = string.Join(", ", values.Keys.Select(c => $"@{c}"));
                cmd.CommandText = $"INSERT INTO {currentTable} ({columns}) VALUES ({parameters})";

                foreach (var kvp in values)
                {
                    cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                }

                cmd.ExecuteNonQuery();
            }

            RefreshData();
        }

        private List<string> SelectColumns()
        {
            List<string> selectedColumns = new List<string>();
            Form selectForm = new Form()
            {
                Width = 300,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Select Columns",
                StartPosition = FormStartPosition.CenterScreen
            };

            int top = 20;
            foreach (ColumnHeader column in listViewData.Columns)
            {
                if (column.Text != "Id")
                {
                    CheckBox checkBox = new CheckBox()
                    {
                        Text = column.Text,
                        Left = 20,
                        Top = top,
                        Width = 240
                    };
                    selectForm.Controls.Add(checkBox);
                    top += 30;
                }
            }

            Button confirmation = new Button() { Text = "Ok", Left = 100, Width = 80, Top = top + 10, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { selectForm.Close(); };
            selectForm.Controls.Add(confirmation);

            if (selectForm.ShowDialog() == DialogResult.OK)
            {
                foreach (Control control in selectForm.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Checked)
                    {
                        selectedColumns.Add(checkBox.Text);
                    }
                }
            }

            return selectedColumns;
        }

        private void RefreshData()
        {
            listViewData.Items.Clear();
            listViewData.Columns.Clear();

            if (connection == null || string.IsNullOrEmpty(currentTable))
            {
                return;
            }

            using (SQLiteCommand cmd = new SQLiteCommand($"PRAGMA table_info({currentTable})", connection))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string columnName = reader["name"].ToString();
                    listViewData.Columns.Add(columnName);
                }
            }

            using (SQLiteCommand cmd = new SQLiteCommand($"SELECT * FROM {currentTable}", connection))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader[0].ToString());
                    for (int i = 1; i < reader.FieldCount; i++)
                    {
                        item.SubItems.Add(reader[i].ToString());
                    }
                    listViewData.Items.Add(item);
                }
            }
        }

        private string PromptForInput(string prompt, string title)
        {
            Form promptForm = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = prompt };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240 };
            Button confirmation = new Button() { Text = "Ok", Left = 180, Width = 80, Top = 80, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { promptForm.Close(); };
            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(confirmation);
            promptForm.Controls.Add(textLabel);
            promptForm.AcceptButton = confirmation;

            return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (listViewData.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            ListViewItem selectedItem = listViewData.SelectedItems[0];
            string whereClause = BuildWhereClauseForRow(selectedItem);

            DialogResult result = MessageBox.Show($"Are you sure you want to delete this row?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = $"DELETE FROM {currentTable} WHERE {whereClause}";
                    for (int i = 0; i < listViewData.Columns.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@param{i}", selectedItem.SubItems[i].Text);
                    }
                    cmd.ExecuteNonQuery();
                }
                RefreshData();
            }
        }

        private string BuildWhereClauseForRow(ListViewItem item)
        {
            List<string> conditions = new List<string>();
            for (int i = 0; i < listViewData.Columns.Count; i++)
            {
                string columnName = listViewData.Columns[i].Text;
                conditions.Add($"{columnName} = @param{i}");
            }
            return string.Join(" AND ", conditions);
        }

        private void btnDeleteColumn_Click(object sender, EventArgs e)
        {
            if (listViewData.Columns.Count <= 1)
            {
                MessageBox.Show("There are no columns to delete.");
                return;
            }

            List<string> columns = new List<string>();
            foreach (ColumnHeader column in listViewData.Columns)
            {
                columns.Add(column.Text);
            }

            string selectedColumn = PromptForSelection("Select column to delete:", "Delete Column", columns);

            if (!string.IsNullOrEmpty(selectedColumn))
            {
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the column '{selectedColumn}'?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        // Get all column names except the one to be deleted
                        List<string> remainingColumns = columns.Where(c => c != selectedColumn).ToList();
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

                    RefreshData();
                }
            }
        }

        private string PromptForSelection(string prompt, string title, List<string> options)
        {
            Form promptForm = new Form()
            {
                Width = 300,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = prompt };
            ComboBox comboBox = new ComboBox() { Left = 20, Top = 50, Width = 240 };
            comboBox.Items.AddRange(options.ToArray());
            Button confirmation = new Button() { Text = "Ok", Left = 180, Width = 80, Top = 80, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { promptForm.Close(); };
            promptForm.Controls.Add(comboBox);
            promptForm.Controls.Add(confirmation);
            promptForm.Controls.Add(textLabel);
            promptForm.AcceptButton = confirmation;

            return promptForm.ShowDialog() == DialogResult.OK ? comboBox.SelectedItem?.ToString() : "";
        }
    }
}