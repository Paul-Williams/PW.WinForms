
using PW.Events;
using PW.FailFast;

namespace PW.WinForms;

/// <summary>
/// Helper class to handle setting and restoring a form to full-screen border-less and back.
/// </summary>
public class FormFullScreen
{
  /// <summary>
  /// Raised after the form's full screen state changes. 
  /// The payload will be true if the form is now full screen, otherwise false.
  /// </summary>
  public PayloadEventHandler<bool>? FullScreenChanged;

  private Form Form { get; }

  private FormBorderStyle BorderStyleBeforeFullScreen { get; set; }
  private Size SizeBeforeFullScreen { get; set; }

  /// <summary>
  /// Creates a new instance.
  /// </summary>
  /// <param name="form"></param>
  public FormFullScreen(Form form)
  {
    Guard.NotNull(form, nameof(form));
    Form = form;
  }

  /// <summary>
  /// Get/Set the Form's full-screen state.
  /// </summary>
  public bool IsFullScreen
  {
    get;

    set
    {
      if (!BackingField.AssignIfNotEqual(value, ref field)) return;

      Form.SuspendLayout();
      if (field)
      {
        SizeBeforeFullScreen = Form.Size;
        BorderStyleBeforeFullScreen = Form.FormBorderStyle;

        Form.FormBorderStyle = FormBorderStyle.None;
        Form.WindowState = FormWindowState.Maximized;
        FullScreenChanged?.Invoke(true);
      }
      else
      {
        Form.WindowState = FormWindowState.Normal;
        Form.FormBorderStyle = BorderStyleBeforeFullScreen;
        Form.Size = SizeBeforeFullScreen;
        FullScreenChanged?.Invoke(false);
      }
      Form.ResumeLayout(true);
    }
  }


  /// <summary>
  /// Toggles the Form's full-screen state
  /// </summary>
  public void Toggle() => IsFullScreen = !IsFullScreen;


  //private bool IsAutoFullScreen = false;

  //private void TrackFullScreen() => IsFullScreen = Form.WindowState == FormWindowState.Maximized;

  ///// <summary>
  ///// 
  ///// </summary>
  //public bool AutoFullScreen
  //{
  //  get => IsAutoFullScreen;
  //  set
  //  {
  //    if (value == true && !IsAutoFullScreen)
  //    {
  //      IsAutoFullScreen = true;
  //      Form.ClientSizeChanged += (s, e) => TrackFullScreen();
  //    }
  //    else
  //    {
  //      Form.ClientSizeChanged-= (s, e) => TrackFullScreen();
  //    }
  //  }
  //}


}
