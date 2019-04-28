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
			System.Windows.Forms.Label lblAdd;
			System.Windows.Forms.Label lblSample;
			System.Windows.Forms.NumericUpDown udPasswordLength;
			System.Windows.Forms.Button cmdRefresh;
			System.Windows.Forms.Button cmdCustom;
			this.dgvComponents = new System.Windows.Forms.DataGridView();
			this.colCharacterSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.componentsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.cboAdd = new System.Windows.Forms.ComboBox();
			this.txtExample = new System.Windows.Forms.TextBox();
			this.MinRequired = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ruleSource = new System.Windows.Forms.BindingSource(this.components);
			lblLength = new System.Windows.Forms.Label();
			lblAdd = new System.Windows.Forms.Label();
			lblSample = new System.Windows.Forms.Label();
			udPasswordLength = new System.Windows.Forms.NumericUpDown();
			cmdRefresh = new System.Windows.Forms.Button();
			cmdCustom = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(udPasswordLength)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvComponents)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.componentsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ruleSource)).BeginInit();
			this.SuspendLayout();
			// 
			// lblLength
			// 
			resources.ApplyResources(lblLength, "lblLength");
			lblLength.Name = "lblLength";
			// 
			// lblAdd
			// 
			resources.ApplyResources(lblAdd, "lblAdd");
			lblAdd.Name = "lblAdd";
			// 
			// lblSample
			// 
			resources.ApplyResources(lblSample, "lblSample");
			lblSample.Name = "lblSample";
			// 
			// udPasswordLength
			// 
			udPasswordLength.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.ruleSource, "Length", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			resources.ApplyResources(udPasswordLength, "udPasswordLength");
			udPasswordLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			udPasswordLength.Name = "udPasswordLength";
			udPasswordLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			udPasswordLength.ValueChanged += new System.EventHandler(this.OnLengthUpdate);
			// 
			// cmdRefresh
			// 
			resources.ApplyResources(cmdRefresh, "cmdRefresh");
			cmdRefresh.Name = "cmdRefresh";
			cmdRefresh.UseVisualStyleBackColor = true;
			cmdRefresh.Click += new System.EventHandler(this.RefreshClick);
			// 
			// dgvComponents
			// 
			this.dgvComponents.AllowUserToAddRows = false;
			this.dgvComponents.AllowUserToDeleteRows = false;
			this.dgvComponents.AllowUserToResizeRows = false;
			resources.ApplyResources(this.dgvComponents, "dgvComponents");
			this.dgvComponents.AutoGenerateColumns = false;
			this.dgvComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCharacterSet,
            this.MinRequired});
			this.dgvComponents.DataSource = this.componentsBindingSource;
			this.dgvComponents.Name = "dgvComponents";
			this.dgvComponents.RowHeadersVisible = false;
			this.dgvComponents.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnGridUpdate);
			this.dgvComponents.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.HandleError);
			this.dgvComponents.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.AddCharacterSet);
			// 
			// colCharacterSet
			// 
			this.colCharacterSet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.colCharacterSet, "colCharacterSet");
			this.colCharacterSet.Name = "colCharacterSet";
			this.colCharacterSet.ReadOnly = true;
			// 
			// componentsBindingSource
			// 
			this.componentsBindingSource.DataMember = "Components";
			this.componentsBindingSource.DataSource = this.ruleSource;
			// 
			// cboAdd
			// 
			resources.ApplyResources(this.cboAdd, "cboAdd");
			this.cboAdd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAdd.FormattingEnabled = true;
			this.cboAdd.Name = "cboAdd";
			this.cboAdd.SelectedIndexChanged += new System.EventHandler(this.AddRow);
			// 
			// txtExample
			// 
			resources.ApplyResources(this.txtExample, "txtExample");
			this.txtExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtExample.Name = "txtExample";
			this.txtExample.ReadOnly = true;
			// 
			// MinRequired
			// 
			this.MinRequired.DataPropertyName = "MinCount";
			resources.ApplyResources(this.MinRequired, "MinRequired");
			this.MinRequired.Name = "MinRequired";
			// 
			// ruleSource
			// 
			this.ruleSource.DataSource = typeof(RuleBuilder.Rule.PasswordRule);
			// 
			// cmdCustom
			// 
			resources.ApplyResources(cmdCustom, "cmdCustom");
			cmdCustom.Name = "cmdCustom";
			cmdCustom.UseVisualStyleBackColor = true;
			cmdCustom.Click += new System.EventHandler(this.AddCustom);
			// 
			// EditRule
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(cmdCustom);
			this.Controls.Add(cmdRefresh);
			this.Controls.Add(this.txtExample);
			this.Controls.Add(lblSample);
			this.Controls.Add(lblAdd);
			this.Controls.Add(this.cboAdd);
			this.Controls.Add(this.dgvComponents);
			this.Controls.Add(udPasswordLength);
			this.Controls.Add(lblLength);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditRule";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(udPasswordLength)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvComponents)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.componentsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ruleSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.BindingSource ruleSource;
		private System.Windows.Forms.BindingSource componentsBindingSource;
		private System.Windows.Forms.DataGridView dgvComponents;
		private System.Windows.Forms.ComboBox cboAdd;
		private System.Windows.Forms.TextBox txtExample;
		private System.Windows.Forms.DataGridViewTextBoxColumn colCharacterSet;
		private System.Windows.Forms.DataGridViewTextBoxColumn MinRequired;
	}
}