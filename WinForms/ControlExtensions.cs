#nullable enable

using System.Windows.Forms;

namespace PW.WinForms
{
  /// <summary>
  /// Extension methods for WinForm controls
  /// </summary>
  public static class ControlExtensions
  {

    /// <summary>
    /// Sets all items checked value to <paramref name="state"/>
    /// </summary>
    public static void SetItemsChecked(this CheckedListBox control, bool state)
    {
      if (control.Items.Count is int count && count > 0)
        for (var i = 0; i < count; ++i) control.SetItemChecked(i, state);
    }

    /// <summary>
    /// Checks all items.
    /// </summary>    
    public static void CheckAll(this CheckedListBox control) => SetItemsChecked(control, true);

    /// <summary>
    /// Un-checks all items.
    /// </summary>    
    public static void UncheckAll(this CheckedListBox control) => SetItemsChecked(control, false);



    /// <summary>
    /// Attempts to find a parent control of type <typeparamref name="T"/>. 
    /// Returns the parent control if found. Returns null when no parent of the specified type is found or the control '<paramref name="c"/>' is null.
    /// </summary>
    public static Control? FindParent<T>(this Control c) where T : Control
    {
      // See: https://referencesource.microsoft.com/#System.Windows.Forms/winforms/Managed/System/WinForms/Control.cs,855704ced06bee8f,references
      // See: https://www.infoq.com/news/2020/07/CSharp-And-Or-Not/
      while (c is not null and not T) c = c.Parent;
      return c;
    }

    /// <summary>
    /// Sets the control's current height as it's maximum height. Height can be reduced. Width can be freely changed. 
    /// </summary>
    public static void PinMaximumHeight(this Control c) => c.MaximumSize = new System.Drawing.Size(int.MaxValue, c.Height);

    /// <summary>
    /// Sets the controls current size as it's minimum size. Prevents control from being resized smaller in either dimension.
    /// </summary>
    public static void PinMinimumSize(this Control c) => c.MinimumSize = c.Size;



  }
}
