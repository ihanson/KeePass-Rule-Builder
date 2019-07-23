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
			System.Windows.Forms.Button cmdRefresh;
			System.Windows.Forms.Label lblSample;
			System.Windows.Forms.Label label1;
			this.btnDeleteRow = new System.Windows.Forms.Button();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.customToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuComponents = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.udPasswordLength = new System.Windows.Forms.NumericUpDown();
			this.dgvComponents = new System.Windows.Forms.DataGridView();
			this.pnlRule = new System.Windows.Forms.Panel();
			this.txtExclude = new System.Windows.Forms.TextBox();
			this.rdoRule = new System.Windows.Forms.RadioButton();
			this.txtExample = new System.Windows.Forms.TextBox();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlProfile = new System.Windows.Forms.Panel();
			this.lbProfiles = new System.Windows.Forms.ListBox();
			this.rdoProfile = new System.Windows.Forms.RadioButton();
			this.btnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.Characters = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Required = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			lblLength = new System.Windows.Forms.Label();
			cmdRefresh = new System.Windows.Forms.Button();
			lblSample = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			this.mnuComponents.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.udPasswordLength)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvComponents)).BeginInit();
			this.pnlRule.SuspendLayout();
			this.pnlProfile.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblLength
			// 
			resources.ApplyResources(lblLength, "lblLength");
			lblLength.Name = "lblLength";
			// 
			// cmdRefresh
			// 
			resources.ApplyResources(cmdRefresh, "cmdRefresh");
			cmdRefresh.Image = global::RuleBuilder.Properties.Resources.Refresh;
			cmdRefresh.Name = "cmdRefresh";
			cmdRefresh.UseVisualStyleBackColor = true;
			cmdRefresh.Click += new System.EventHandler(this.OnRefreshClick);
			// 
			// lblSample
			// 
			resources.ApplyResources(lblSample, "lblSample");
			lblSample.Name = "lblSample";
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			// 
			// btnDeleteRow
			// 
			resources.ApplyResources(this.btnDeleteRow, "btnDeleteRow");
			this.btnDeleteRow.Name = "btnDeleteRow";
			this.btnDeleteRow.UseVisualStyleBackColor = true;
			this.btnDeleteRow.Click += new System.EventHandler(this.OnDeleteRowClick);
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
			// mnuComponents
			// 
			this.mnuComponents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.customToolStripMenuItem});
			this.mnuComponents.Name = "mnuComponents";
			this.mnuComponents.ShowImageMargin = false;
			resources.ApplyResources(this.mnuComponents, "mnuComponents");
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
            this.Characters,
            this.Required});
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
			// pnlRule
			// 
			resources.ApplyResources(this.pnlRule, "pnlRule");
			this.pnlRule.Controls.Add(this.txtExclude);
			this.pnlRule.Controls.Add(label1);
			this.pnlRule.Controls.Add(this.dgvComponents);
			this.pnlRule.Controls.Add(this.btnDeleteRow);
			this.pnlRule.Controls.Add(lblLength);
			this.pnlRule.Controls.Add(this.udPasswordLength);
			this.pnlRule.Name = "pnlRule";
			// 
			// txtExclude
			// 
			resources.ApplyResources(this.txtExclude, "txtExclude");
			this.txtExclude.Name = "txtExclude";
			this.txtExclude.TextChanged += new System.EventHandler(this.OnExcludeUpdate);
			// 
			// rdoRule
			// 
			resources.ApplyResources(this.rdoRule, "rdoRule");
			this.rdoRule.Checked = true;
			this.rdoRule.Name = "rdoRule";
			this.rdoRule.TabStop = true;
			this.rdoRule.UseVisualStyleBackColor = true;
			this.rdoRule.CheckedChanged += new System.EventHandler(this.RuleTypeSelected);
			// 
			// txtExample
			// 
			resources.ApplyResources(this.txtExample, "txtExample");
			this.txtExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtExample.Name = "txtExample";
			this.txtExample.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// pnlProfile
			// 
			resources.ApplyResources(this.pnlProfile, "pnlProfile");
			this.pnlProfile.Controls.Add(this.lbProfiles);
			this.pnlProfile.Name = "pnlProfile";
			// 
			// lbProfiles
			// 
			resources.ApplyResources(this.lbProfiles, "lbProfiles");
			this.lbProfiles.FormattingEnabled = true;
			this.lbProfiles.Name = "lbProfiles";
			this.lbProfiles.SelectedIndexChanged += new System.EventHandler(this.OnRefreshClick);
			// 
			// rdoProfile
			// 
			resources.ApplyResources(this.rdoProfile, "rdoProfile");
			this.rdoProfile.Name = "rdoProfile";
			this.rdoProfile.UseVisualStyleBackColor = true;
			this.rdoProfile.CheckedChanged += new System.EventHandler(this.RuleTypeSelected);
			// 
			// btnAccept
			// 
			resources.ApplyResources(this.btnAccept, "btnAccept");
			this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.Save);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// Characters
			// 
			this.Characters.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.Characters, "Characters");
			this.Characters.Name = "Characters";
			this.Characters.ReadOnly = true;
			this.Characters.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Required
			// 
			resources.ApplyResources(this.Required, "Required");
			this.Required.Name = "Required";
			this.Required.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// EditRule
			// 
			this.AcceptButton = this.btnAccept;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(cmdRefresh);
			this.Controls.Add(this.txtExample);
			this.Controls.Add(lblSample);
			this.Controls.Add(this.rdoProfile);
			this.Controls.Add(this.rdoRule);
			this.Controls.Add(this.pnlRule);
			this.Controls.Add(this.pnlProfile);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditRule";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.mnuComponents.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.udPasswordLength)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvComponents)).EndInit();
			this.pnlRule.ResumeLayout(false);
			this.pnlRule.PerformLayout();
			this.pnlProfile.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnDeleteRow;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem customToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip mnuComponents;
		private System.Windows.Forms.NumericUpDown udPasswordLength;
		private System.Windows.Forms.DataGridView dgvComponents;
		private System.Windows.Forms.Panel pnlRule;
		private System.Windows.Forms.RadioButton rdoRule;
		private System.Windows.Forms.TextBox txtExample;
		private System.Windows.Forms.Panel pnlProfile;
		private System.Windows.Forms.ListBox lbProfiles;
		private System.Windows.Forms.RadioButton rdoProfile;
		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtExclude;
		private System.Windows.Forms.DataGridViewTextBoxColumn Characters;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Required;
	}
}