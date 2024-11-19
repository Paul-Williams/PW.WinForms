namespace PW.WinForms;


/// <summary>
/// Displays a <see cref="MessageBox"/>
/// </summary>
public static class MsgBox
{

  /// <summary>
  /// Optional. The title to be used for all message dialogs.
  /// </summary>
  public static string? Title { get; set; } = null;

  /// <summary>
  /// Display exception error message box
  /// </summary>
  public static void ShowError(Exception ex, IWin32Window? owner = null) => MessageBox.Show(owner, ex.MessageSafe(), Title ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

  private static string MessageSafe(this Exception ex) => ex != null ? ex.Message : "!! EXCEPTION IS NULL !!";


  /// <summary>
  /// Display exception error message box, with additional information
  /// </summary>
  public static void ShowError(Exception ex, string message, IWin32Window? owner = null) => MessageBox.Show(owner, message + Environment.NewLine + Environment.NewLine + ex.MessageSafe(), Title ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

  /// <summary>
  /// Displays error message box
  /// </summary>
  public static void ShowError(string message, IWin32Window? owner = null) => MessageBox.Show(owner, message, Title ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

  /// <summary>
  /// Possible results for the <see cref="ShowQuestion(string, IWin32Window)"/> method
  /// </summary>
  public enum QuestionResult
  {
    /// <summary>
    /// Yes result
    /// </summary>
    Yes = DialogResult.Yes,

    /// <summary>
    /// No result
    /// </summary>
    No = DialogResult.No,

    ///// <summary>
    ///// None result
    ///// </summary>
    //None = DialogResult.None
  };

  /// <summary>
  /// Possible results for <see cref="ShowYesNoCancel(string, IWin32Window?)"/>
  /// </summary>
  public enum YesNoCancelResult
  {
    /// <summary>
    /// Yes result.
    /// </summary>
    Yes = DialogResult.Yes,
    /// <summary>
    /// No result.
    /// </summary>
    No = DialogResult.No,
    /// <summary>
    /// Cancel result.
    /// </summary>
    Cancel = DialogResult.Cancel
  }

  /// <summary>
  /// Displays a question with Yes/No/Cancel options.
  /// </summary>
  public static YesNoCancelResult ShowYesNoCancel(string question, IWin32Window? owner = null)
    => (YesNoCancelResult)MessageBox.Show(owner, question, Title ?? "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);



  /// <summary>
  /// Displays a question message box and returns the user response as <see cref="QuestionResult"/> .
  /// </summary>
  public static QuestionResult ShowQuestion(string question, IWin32Window? owner = null)
    => (QuestionResult)MessageBox.Show(owner, question, Title ?? "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


  /// <summary>
  /// Displays a warning message box
  /// </summary>
  public static void ShowWarning(string message, IWin32Window? owner = null) =>
    MessageBox.Show(owner, message, Title ?? "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

  /// <summary>
  /// Displays an info message box
  /// </summary>
  public static void ShowInfo(string message, IWin32Window? owner = null) =>
    MessageBox.Show(owner, message, Title ?? "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

}
