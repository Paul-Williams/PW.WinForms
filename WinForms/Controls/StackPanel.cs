#nullable enable

using System.Windows.Forms;

namespace PW.WinForms.Controls
{
  /// <summary>
  /// A control which automatically stacks child controls vertically and to full width.
  /// </summary>
  public class StackPanel : UserControl
  {
    /// <summary>
    /// <see cref="Control.OnControlAdded(ControlEventArgs)"/>
    /// </summary>
    protected override void OnControlAdded(ControlEventArgs e)
    {
      LayoutNewControl();
      base.OnControlAdded(e);
    }

    /// <summary>
    /// <see cref="Control.OnControlRemoved(ControlEventArgs)"/>
    /// </summary>
    protected override void OnControlRemoved(ControlEventArgs e)
    {
      MoveRemainingControls();
      base.OnControlRemoved(e);
    }

    /// <summary>
    /// Vertical spacing between child-controls. 
    /// TODO: Replace with <see cref="Padding"/>, to enable spacing on all sides of child-controls.
    /// </summary>
    public int Spacing { get; } = 3;

    /// <summary>
    /// Move controls after removing control.
    /// </summary>
    private void MoveRemainingControls()
    {
      // This method is non-optimal. 
      // If the removed control was the last control, then there is no need to move anything.

      var count = Controls.Count;

      // If there are no controls remaining, then there is nothing to do.
      if (count == 0) return;
      SuspendLayout();

      // Position the first control
      Controls[0].Top = NextTop(null);

      // Position subsequent controls, if any.
      if (count == 1) return;
      for (int i = 1; i < count; i++) Controls[i].Top = NextTop(Controls[i - 1]);
      ResumeLayout();
    }

    /// <summary>
    /// Position last control below previous control, set width and anchor.
    /// </summary>
    void LayoutNewControl()
    {
      var count = Controls.Count;
      if (count == 0) return;
      var ctl = Controls[count - 1];
      SuspendLayout();
      ctl.Top = count == 1 ? NextTop(null) : NextTop(Controls[count - 2]);
      ctl.Width = Width;
      ctl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      ResumeLayout();
    }

    /// <summary>
    /// Determines <see cref="Control.Top"/> for the next control. Pass null to position the first control.
    /// </summary>
    private int NextTop(Control? previous) => previous is not null ? previous.Top + previous.Height + Spacing : Spacing;

  }
}
