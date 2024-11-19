namespace WatchTracker.Controls;

public partial class InterceptPasteTextBox
{
  /// <summary>
  /// Creates a new instance.
  /// </summary>
  public class BeforePasteEventArgs(string s) : EventArgs
  {

    /// <summary>
    /// Set to 'true' if the text box text was updated by the event handler or to cancel updating the text box. 
    /// Leave as 'false' for the control to update it's own text.
    /// </summary>
    public bool Handled { get; set; }

    /// <summary>
    /// The text to be pasted from the clipboard. Modify this as required and the modified text will be 'pasted'.
    /// </summary>
    public string Text { get; set; } = s;
  }

}
