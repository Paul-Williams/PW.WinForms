#nullable enable

using System;
using System.Windows.Forms;


namespace PW.WinForms
{
  /// <summary>
  /// Basic form to display exception information
  /// </summary>
  public partial class ExceptionDialog : Form
  {

    /// <summary>
    /// Modally displays a dialog for the specified exception
    /// </summary>
    /// <param name="ex"></param>
    public static void Display(Exception ex)
    {
      using var dlg = new ExceptionDialog { Exception = ex };
      dlg.ShowDialog();
    }


    /// <summary>
    /// Creates a new instance
    /// </summary>
    private ExceptionDialog()
    {
      InitializeComponent();
    }


    /// <summary>
    /// The exception to be displayed
    /// </summary>
    public Exception? Exception { get; set; }

    private void PopulateControlsWithExceptionDetails()
    {

      if (Exception is Exception ex)
      {
        MessageTextBox.Text = ex.Message;
        StackTraceTextBox.Text = ex.ToString();
      }
      else
      {
        MessageTextBox.Text = "Unable to display exception information.\nException is null.";
      }
    }

    private void ExceptionDialog_Load(object sender, EventArgs e)
    {
      // Catch-all: Don't expect exceptions here, but it is important not to throw exceptions in the exception handling !!
      try
      {
        PopulateControlsWithExceptionDetails();
      }
      catch { }

    }



  }
}
