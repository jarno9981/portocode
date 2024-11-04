namespace DBManager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listViewData = new System.Windows.Forms.ListView();
            this.btnCreateDb = new System.Windows.Forms.Button();
            this.btnOpenDb = new System.Windows.Forms.Button();
            this.btnAddColumn = new System.Windows.Forms.Button();
            this.btnAddData = new System.Windows.Forms.Button();
            this.btnDeleteRow = new System.Windows.Forms.Button();
            this.btnDeleteColumn = new System.Windows.Forms.Button();
            this.btnCreateTable = new System.Windows.Forms.Button();
            this.cboTables = new System.Windows.Forms.ComboBox();
            this.lblTables = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewData
            // 
            this.listViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewData.HideSelection = false;
            this.listViewData.Location = new System.Drawing.Point(15, 93);
            this.listViewData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listViewData.Name = "listViewData";
            this.listViewData.Size = new System.Drawing.Size(997, 489);
            this.listViewData.TabIndex = 0;
            this.listViewData.UseCompatibleStateImageBehavior = false;
            // 
            // btnCreateDb
            // 
            this.btnCreateDb.Location = new System.Drawing.Point(15, 16);
            this.btnCreateDb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreateDb.Name = "btnCreateDb";
            this.btnCreateDb.Size = new System.Drawing.Size(96, 31);
            this.btnCreateDb.TabIndex = 1;
            this.btnCreateDb.Text = "Create DB";
            this.btnCreateDb.UseVisualStyleBackColor = true;
            this.btnCreateDb.Click += new System.EventHandler(this.btnCreateDb_Click);
            // 
            // btnOpenDb
            // 
            this.btnOpenDb.Location = new System.Drawing.Point(120, 16);
            this.btnOpenDb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenDb.Name = "btnOpenDb";
            this.btnOpenDb.Size = new System.Drawing.Size(96, 31);
            this.btnOpenDb.TabIndex = 2;
            this.btnOpenDb.Text = "Open DB";
            this.btnOpenDb.UseVisualStyleBackColor = true;
            this.btnOpenDb.Click += new System.EventHandler(this.btnOpenDb_Click);
            // 
            // btnAddColumn
            // 
            this.btnAddColumn.Location = new System.Drawing.Point(224, 16);
            this.btnAddColumn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(124, 31);
            this.btnAddColumn.TabIndex = 3;
            this.btnAddColumn.Text = "Add Column";
            this.btnAddColumn.UseVisualStyleBackColor = true;
            this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
            // 
            // btnAddData
            // 
            this.btnAddData.Location = new System.Drawing.Point(356, 16);
            this.btnAddData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(129, 31);
            this.btnAddData.TabIndex = 4;
            this.btnAddData.Text = "Add Data";
            this.btnAddData.UseVisualStyleBackColor = true;
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // btnDeleteRow
            // 
            this.btnDeleteRow.Location = new System.Drawing.Point(493, 16);
            this.btnDeleteRow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteRow.Name = "btnDeleteRow";
            this.btnDeleteRow.Size = new System.Drawing.Size(171, 31);
            this.btnDeleteRow.TabIndex = 5;
            this.btnDeleteRow.Text = "Delete Row";
            this.btnDeleteRow.UseVisualStyleBackColor = true;
            this.btnDeleteRow.Click += new System.EventHandler(this.btnDeleteRow_Click);
            // 
            // btnDeleteColumn
            // 
            this.btnDeleteColumn.Location = new System.Drawing.Point(672, 16);
            this.btnDeleteColumn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteColumn.Name = "btnDeleteColumn";
            this.btnDeleteColumn.Size = new System.Drawing.Size(160, 31);
            this.btnDeleteColumn.TabIndex = 6;
            this.btnDeleteColumn.Text = "Delete Column";
            this.btnDeleteColumn.UseVisualStyleBackColor = true;
            this.btnDeleteColumn.Click += new System.EventHandler(this.btnDeleteColumn_Click);
            // 
            // btnCreateTable
            // 
            this.btnCreateTable.Location = new System.Drawing.Point(840, 16);
            this.btnCreateTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreateTable.Name = "btnCreateTable";
            this.btnCreateTable.Size = new System.Drawing.Size(172, 31);
            this.btnCreateTable.TabIndex = 7;
            this.btnCreateTable.Text = "Create Table";
            this.btnCreateTable.UseVisualStyleBackColor = true;
            this.btnCreateTable.Click += new System.EventHandler(this.btnCreateTable_Click);
            // 
            // cboTables
            // 
            this.cboTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTables.FormattingEnabled = true;
            this.cboTables.Location = new System.Drawing.Point(120, 55);
            this.cboTables.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboTables.Name = "cboTables";
            this.cboTables.Size = new System.Drawing.Size(199, 28);
            this.cboTables.TabIndex = 8;
            this.cboTables.SelectedIndexChanged += new System.EventHandler(this.cboTables_SelectedIndexChanged);
            // 
            // lblTables
            // 
            this.lblTables.AutoSize = true;
            this.lblTables.Location = new System.Drawing.Point(15, 59);
            this.lblTables.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTables.Name = "lblTables";
            this.lblTables.Size = new System.Drawing.Size(101, 20);
            this.lblTables.TabIndex = 9;
            this.lblTables.Text = "Select Table:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 600);
            this.Controls.Add(this.lblTables);
            this.Controls.Add(this.cboTables);
            this.Controls.Add(this.btnCreateTable);
            this.Controls.Add(this.btnDeleteColumn);
            this.Controls.Add(this.btnDeleteRow);
            this.Controls.Add(this.btnAddData);
            this.Controls.Add(this.btnAddColumn);
            this.Controls.Add(this.btnOpenDb);
            this.Controls.Add(this.btnCreateDb);
            this.Controls.Add(this.listViewData);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "DB Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewData;
        private System.Windows.Forms.Button btnCreateDb;
        private System.Windows.Forms.Button btnOpenDb;
        private System.Windows.Forms.Button btnAddColumn;
        private System.Windows.Forms.Button btnAddData;
        private System.Windows.Forms.Button btnDeleteRow;
        private System.Windows.Forms.Button btnDeleteColumn;
        private System.Windows.Forms.Button btnCreateTable;
        private System.Windows.Forms.ComboBox cboTables;
        private System.Windows.Forms.Label lblTables;
    }
}