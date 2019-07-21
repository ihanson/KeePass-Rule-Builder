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
			this.lblAutoTypeOld = new System.Windows.Forms.Label();
			this.lblAutoTypeNew = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnAccept = new System.Windows.Forms.Button();
			this.lblOldPassword = new System.Windows.Forms.Label();
			this.txtOldPassword = new System.Windows.Forms.TextBox();
			this.txtNewPassword = new System.Windows.Forms.TextBox();
			this.lblNewPassword = new System.Windows.Forms.Label();
			this.btnEditRule = new System.Windows.Forms.Button();
			cmdRefresh = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmdRefresh
			// 
			resources.ApplyResources(cmdRefresh, "cmdRefresh");
			cmdRefresh.Image = global::RuleBuilder.Properties.Resources.Refresh;
			cmdRefresh.Name = "cmdRefresh";
			cmdRefresh.UseVisualStyleBackColor = true;
			cmdRefresh.Click += new System.EventHandler(this.Refresh);
			// 
			// lblAutoTypeOld
			// 
			resources.ApplyResources(this.lblAutoTypeOld, "lblAutoTypeOld");
			this.lblAutoTypeOld.Name = "lblAutoTypeOld";
			// 
			// lblAutoTypeNew
			// 
			resources.ApplyResources(this.lblAutoTypeNew, "lblAutoTypeNew");
			this.lblAutoTypeNew.Name = "lblAutoTypeNew";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnAccept
			// 
			resources.ApplyResources(this.btnAccept, "btnAccept");
			this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.SaveNewPassword);
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
			this.AcceptButton = this.btnAccept;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.lblAutoTypeNew);
			this.Controls.Add(this.lblAutoTypeOld);
			this.Controls.Add(cmdRefresh);
			this.Controls.Add(this.btnEditRule);
			this.Controls.Add(this.txtNewPassword);
			this.Controls.Add(this.lblNewPassword);
			this.Controls.Add(this.txtOldPassword);
			this.Controls.Add(this.lblOldPassword);
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChangePassword";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.Label lblOldPassword;
		private System.Windows.Forms.TextBox txtOldPassword;
		private System.Windows.Forms.TextBox txtNewPassword;
		private System.Windows.Forms.Label lblNewPassword;
		private System.Windows.Forms.Button btnEditRule;
		private System.Windows.Forms.Label lblAutoTypeOld;
		private System.Windows.Forms.Label lblAutoTypeNew;
	}
}