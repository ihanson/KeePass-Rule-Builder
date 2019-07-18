namespace RuleBuilder.Forms {
	partial class ChangePassword {
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
			System.Windows.Forms.Button cmdRefresh;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePassword));
			System.Windows.Forms.Label lblAutoTypeOld;
			System.Windows.Forms.Label lblAutoTypeNew;
			this.btnDiscard = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.lblOldPassword = new System.Windows.Forms.Label();
			this.txtOldPassword = new System.Windows.Forms.TextBox();
			this.txtNewPassword = new System.Windows.Forms.TextBox();
			this.lblNewPassword = new System.Windows.Forms.Label();
			this.btnEditRule = new System.Windows.Forms.Button();
			cmdRefresh = new System.Windows.Forms.Button();
			lblAutoTypeOld = new System.Windows.Forms.Label();
			lblAutoTypeNew = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cmdRefresh
			// 
			resources.ApplyResources(cmdRefresh, "cmdRefresh");
			cmdRefresh.Name = "cmdRefresh";
			cmdRefresh.UseVisualStyleBackColor = true;
			cmdRefresh.Click += new System.EventHandler(this.Refresh);
			// 
			// lblAutoTypeOld
			// 
			resources.ApplyResources(lblAutoTypeOld, "lblAutoTypeOld");
			lblAutoTypeOld.Name = "lblAutoTypeOld";
			// 
			// lblAutoTypeNew
			// 
			resources.ApplyResources(lblAutoTypeNew, "lblAutoTypeNew");
			lblAutoTypeNew.Name = "lblAutoTypeNew";
			// 
			// btnDiscard
			// 
			resources.ApplyResources(this.btnDiscard, "btnDiscard");
			this.btnDiscard.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnDiscard.Name = "btnDiscard";
			this.btnDiscard.UseVisualStyleBackColor = true;
			// 
			// btnSave
			// 
			resources.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSave.Name = "btnSave";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.SaveNewPassword);
			// 
			// lblOldPassword
			// 
			resources.ApplyResources(this.lblOldPassword, "lblOldPassword");
			this.lblOldPassword.Name = "lblOldPassword";
			// 
			// txtOldPassword
			// 
			resources.ApplyResources(this.txtOldPassword, "txtOldPassword");
			this.txtOldPassword.Name = "txtOldPassword";
			this.txtOldPassword.ReadOnly = true;
			// 
			// txtNewPassword
			// 
			resources.ApplyResources(this.txtNewPassword, "txtNewPassword");
			this.txtNewPassword.Name = "txtNewPassword";
			// 
			// lblNewPassword
			// 
			resources.ApplyResources(this.lblNewPassword, "lblNewPassword");
			this.lblNewPassword.Name = "lblNewPassword";
			// 
			// btnEditRule
			// 
			resources.ApplyResources(this.btnEditRule, "btnEditRule");
			this.btnEditRule.Name = "btnEditRule";
			this.btnEditRule.UseVisualStyleBackColor = true;
			this.btnEditRule.Click += new System.EventHandler(this.EditRule);
			// 
			// ChangePassword
			// 
			this.AcceptButton = this.btnSave;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnDiscard;
			this.Controls.Add(lblAutoTypeNew);
			this.Controls.Add(lblAutoTypeOld);
			this.Controls.Add(cmdRefresh);
			this.Controls.Add(this.btnEditRule);
			this.Controls.Add(this.txtNewPassword);
			this.Controls.Add(this.lblNewPassword);
			this.Controls.Add(this.txtOldPassword);
			this.Controls.Add(this.lblOldPassword);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnDiscard);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChangePassword";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnDiscard;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label lblOldPassword;
		private System.Windows.Forms.TextBox txtOldPassword;
		private System.Windows.Forms.TextBox txtNewPassword;
		private System.Windows.Forms.Label lblNewPassword;
		private System.Windows.Forms.Button btnEditRule;
	}
}