#nullable enable 

using System.Windows.Forms;

namespace WatchTracker.Controls
{
  /// <summary>
  /// A version of the TextBox class which captures window message 'paste' and allows processing of text being pasted.
  /// </summary>
  public partial class InterceptPasteTextBox : TextBox
  {
    const int WM_PASTE = 0x302;

    /// <summary>
    /// Raised when paste message is intercepted, before the text box text is updated.
    /// </summary>
    public delegate void BeforePasteEventHandler(object sender, BeforePasteEventArgs e);

    /// <summary>
    /// Raised when paste message is intercepted, before the text box text is updated.
    /// </summary>
    public event BeforePasteEventHandler? BeforePast;

    /// <inheritdoc/>
    [System.Diagnostics.DebuggerStepThrough]
    protected override void WndProc(ref Message m)
    {

      // Only intercept 'Paste' messages when an event handler is attached.
      if (m.Msg == WM_PASTE && BeforePast is BeforePasteEventHandler handler)
      {
        var e = new BeforePasteEventArgs(Clipboard.GetText());
        handler.Invoke(this, e);
        // If the event was not handled then call the base message handler.
        if (!e.Handled) base.WndProc(ref m);
      }
      else base.WndProc(ref m);

    }

  }
}
