namespace RuleBuilder.Forms {
	partial class EditRule {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Label lblLength;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditRule));
			System.Windows.Forms.Label lblSample;
			System.Windows.Forms.Button cmdRefresh;
			this.udPasswordLength = new System.Windows.Forms.NumericUpDown();
			this.dgvComponents = new System.Windows.Forms.DataGridView();
			this.Characters = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.txtExample = new System.Windows.Forms.TextBox();
			this.mnuComponents = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.customToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDeleteRow = new System.Windows.Forms.Button();
			lblLength = new System.Windows.Forms.Label();
			lblSample = new System.Windows.Forms.Label();
			cmdRefresh = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.udPasswordLength)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvComponents)).BeginInit();
			this.mnuComponents.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblLength
			// 
			resources.ApplyResources(lblLength, "lblLength");
			lblLength.Name = "lblLength";
			// 
			// lblSample
			// 
			resources.ApplyResources(lblSample, "lblSample");
			lblSample.Name = "lblSample";
			// 
			// cmdRefresh
			// 
			resources.ApplyResources(cmdRefresh, "cmdRefresh");
			cmdRefresh.Name = "cmdRefresh";
			cmdRefresh.UseVisualStyleBackColor = true;
			cmdRefresh.Click += new System.EventHandler(this.OnRefreshClick);
			// 
			// udPasswordLength
			// 
			resources.ApplyResources(this.udPasswordLength, "udPasswordLength");
			this.udPasswordLength.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.udPasswordLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udPasswordLength.Name = "udPasswordLength";
			this.udPasswordLength.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
			this.udPasswordLength.ValueChanged += new System.EventHandler(this.OnLengthUpdate);
			// 
			// dgvComponents
			// 
			this.dgvComponents.AllowUserToAddRows = false;
			this.dgvComponents.AllowUserToResizeRows = false;
			resources.ApplyResources(this.dgvComponents, "dgvComponents");
			this.dgvComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Characters});
			this.dgvComponents.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.dgvComponents.MultiSelect = false;
			this.dgvComponents.Name = "dgvComponents";
			this.dgvComponents.RowHeadersVisible = false;
			this.dgvComponents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvComponents.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellClick);
			this.dgvComponents.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellValueChange);
			this.dgvComponents.CurrentCellDirtyStateChanged += new System.EventHandler(this.OnDirtyStateChange);
			this.dgvComponents.SelectionChanged += new System.EventHandler(this.OnSelectionChange);
			this.dgvComponents.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.OnDeletingRow);
			// 
			// Characters
			// 
			this.Characters.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.Characters, "Characters");
			this.Characters.Name = "Characters";
			this.Characters.ReadOnly = true;
			this.Characters.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// txtExample
			// 
			resources.ApplyResources(this.txtExample, "txtExample");
			this.txtExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtExample.Name = "txtExample";
			this.txtExample.ReadOnly = true;
			// 
			// mnuComponents
			// 
			this.mnuComponents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.customToolStripMenuItem});
			this.mnuComponents.Name = "mnuComponents";
			this.mnuComponents.ShowImageMargin = false;
			resources.ApplyResources(this.mnuComponents, "mnuComponents");
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// customToolStripMenuItem
			// 
			this.customToolStripMenuItem.Name = "customToolStripMenuItem";
			resources.ApplyResources(this.customToolStripMenuItem, "customToolStripMenuItem");
			this.customToolStripMenuItem.Click += new System.EventHandler(this.AddCustom);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// btnDeleteRow
			// 
			resources.ApplyResources(this.btnDeleteRow, "btnDeleteRow");
			this.btnDeleteRow.Name = "btnDeleteRow";
			this.btnDeleteRow.UseVisualStyleBackColor = true;
			this.btnDeleteRow.Click += new System.EventHandler(this.OnDeleteRowClick);
			// 
			// EditRule
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnDeleteRow);
			this.Controls.Add(cmdRefresh);
			this.Controls.Add(this.txtExample);
			this.Controls.Add(lblSample);
			this.Controls.Add(this.dgvComponents);
			this.Controls.Add(this.udPasswordLength);
			this.Controls.Add(lblLength);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditRule";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.udPasswordLength)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvComponents)).EndInit();
			this.mnuComponents.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.DataGridView dgvComponents;
		private System.Windows.Forms.TextBox txtExample;
		private System.Windows.Forms.ContextMenuStrip mnuComponents;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem customToolStripMenuItem;
		private System.Windows.Forms.NumericUpDown udPasswordLength;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Characters;
		private System.Windows.Forms.Button btnDeleteRow;
	}
}