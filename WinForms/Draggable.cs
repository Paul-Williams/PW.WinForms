namespace PW.WinForms;

/// <summary>
/// Enables dragging of a control or window.
/// </summary>
public static class Draggable
{

  private const int WM_NCLBUTTONDOWN = 0xA1;
  private const int HT_CAPTION = 0x2;

  [System.Runtime.InteropServices.DllImport("user32.dll")]
  private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

  [System.Runtime.InteropServices.DllImport("user32.dll")]
  private static extern bool ReleaseCapture();

  /// <summary>
  /// Performs dragging a form by simulating a mouse-down message in the window caption area.
  /// </summary>
  public static void Drag(this Form form)
  {
    _ = ReleaseCapture();
    _ = SendMessage(form.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
  }

  /// <summary>
  /// Mouse down event handler to perform drag of control.
  /// </summary>
  private static void Drag(object? sender, MouseEventArgs e)
  {
    ArgumentNullException.ThrowIfNull(sender);
    if (e.Button == MouseButtons.Left)
    {
      _ = ReleaseCapture();
      _ = SendMessage(((Control)sender).Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
    }
  }

  /// <summary>
  /// Mouse down event handler to perform drag of control's parent.
  /// </summary>
  private static void DragParent(object? sender, MouseEventArgs e)
  {
    ArgumentNullException.ThrowIfNull(sender);
    if (e.Button == MouseButtons.Left)
    {
      _ = ReleaseCapture();
      _ = SendMessage(((Control)sender).Parent!.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
    }
  }

  /// <summary>
  /// Attaches a MouseDown event handler to enable moving the control by dragging it.
  /// </summary>
  /// <param name="control"></param>
  public static void Attach(Control control) => control.IfNotNull(x => x.MouseDown += Drag);

  /// <summary>
  /// Attaches a MouseDown event handler to enable moving the control by dragging it.
  /// </summary>
  /// <param name="control">Control for which the mouse down will be handled.</param>
  /// <param name="dragParent">Whether to drag the control, or its parent.</param>
  public static void Attach(Control control, bool dragParent)
  {
    ArgumentNullException.ThrowIfNull(control);
    if (dragParent && control.Parent is null) throw new Exception("Control does not have a parent.");

    control.MouseDown += GetDragHandler(dragParent);
  }


  /// <summary>
  /// Removes a drag handler previously attached using <see cref="Attach(Control)"/>.
  /// </summary>
  /// <param name="control"></param>
  public static void Detach(Control control) => control.IfNotNull(c => c.MouseDown -= Drag);

  /// <summary>
  /// Removes a drag handler previously attached using <see cref="Attach(Control, bool)"/>. 
  /// The value of <paramref name="draggingParent"/> must match that used when the control was attached.
  /// </summary>
  public static void Detach(Control control, bool draggingParent) => control.IfNotNull(c => c.MouseDown -= GetDragHandler(draggingParent));

  /// <summary>
  /// Gets the correct event handler to either drag the control or the control's parent.
  /// </summary>
  private static MouseEventHandler GetDragHandler(bool dragParent) => dragParent ? DragParent : Drag;

  #region Extension Methods

  /// <summary>
  /// Enables / disables the drag-ability of the control.
  /// </summary>
  public static void IsDraggable(this Control control, bool enabled)
  {
    if (enabled) Attach(control);
    else Detach(control);
  }

  /// <summary>
  /// Enables / disables the drag-ability of the control's parent.
  /// </summary>
  public static void ParentIsDraggable(this Control control, bool enabled)
  {
    if (enabled) Attach(control, true);
    else Detach(control, true);
  }


  #endregion


}

// Below is the original non-static version of the class. Works fine too, but no need to instances, really.

//internal class Draggable : IDisposable
//{

//  private Control _control;
//  public const int WM_NCLBUTTONDOWN = 0xA1;
//  public const int HT_CAPTION = 0x2;

//  [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
//  public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
//  [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
//  public static extern bool ReleaseCapture();



//  public Draggable(Control control, bool autoAttach = true)
//  {
//    Guard.NotNull(control, nameof(control));
//    _control = control;
//    if (autoAttach) Attach();

//  }


//  private void Drag(object sender, MouseEventArgs e)
//  {      
//    if (e.Button == MouseButtons.Left)
//    {
//      ReleaseCapture();
//      SendMessage(_control.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
//    }
//  }


//  public void Attach() => _control.SafeAssign(c => c.MouseDown += Drag);

//  public void Detach() => _control.SafeAssign(c => c.MouseDown-= Drag);

//  public void Dispose()
//  {
//    Detach();
//    _control = null;
//  }
//}
