namespace PW.WinForms
{
  partial class ExceptionDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.MessageTextBox = new System.Windows.Forms.TextBox();
      this.StackTraceTextBox = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // MessageTextBox
      // 
      this.MessageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MessageTextBox.Location = new System.Drawing.Point(0, 0);
      this.MessageTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.MessageTextBox.Multiline = true;
      this.MessageTextBox.Name = "MessageTextBox";
      this.MessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.MessageTextBox.Size = new System.Drawing.Size(720, 69);
      this.MessageTextBox.TabIndex = 0;
      // 
      // StackTraceTextBox
      // 
      this.StackTraceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.StackTraceTextBox.Location = new System.Drawing.Point(0, 77);
      this.StackTraceTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.StackTraceTextBox.Multiline = true;
      this.StackTraceTextBox.Name = "StackTraceTextBox";
      this.StackTraceTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.StackTraceTextBox.Size = new System.Drawing.Size(720, 389);
      this.StackTraceTextBox.TabIndex = 1;
      // 
      // ExceptionDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(720, 455);
      this.Controls.Add(this.StackTraceTextBox);
      this.Controls.Add(this.MessageTextBox);
      this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.Name = "ExceptionDialog";
      this.Text = "ExceptionDialog";
      this.Load += new System.EventHandler(this.ExceptionDialog_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox MessageTextBox;
    private System.Windows.Forms.TextBox StackTraceTextBox;
  }
}