using System.ComponentModel;

// See: https://www.codeproject.com/Articles/35068/Creating-a-Custom-DropDown-Control
// This class is a refactored copy of the class from the sample project from the above article.

namespace PW.WinForms.Controls;

/// <summary>
/// Provides a drop-down container control. 
/// Important: Call InitializeDropDown(container) immediately after InitializeComponent() in constructor of inherited controls.
/// Where 'container' is the name of the control to be 'dropped down'.
/// </summary>
public partial class DropDownControl : UserControl
{
  /// <summary>
  /// Fired after the drop down is closed.
  /// </summary>
  public event EventHandler? DropDownClosed;

  /// <summary>
  /// Fired after the drop down is opened (dropped-down).
  /// </summary>
  public event EventHandler? DropDownOpened;


  /// <summary>
  /// Side to which the drop down is aligned.
  /// </summary>
  public enum DockSideOption
  {
    /// <summary>
    /// Align left
    /// </summary>
    Left,
    /// <summary>
    /// Align right
    /// </summary>
    Right
  }

  /// <summary>
  /// Possible drop down states.
  /// </summary>
  public enum DropStateOption
  {
    /// <summary>
    /// Closed
    /// </summary>
    Closed,
    /// <summary>
    /// Closing
    /// </summary>
    Closing,
    /// <summary>
    /// Dropping
    /// </summary>
    Dropping,
    /// <summary>
    /// Dropped
    /// </summary>
    Dropped
  }

  private DropDownContainer dropContainer;
  private Control _dropDownItem;
  private bool closedWhileInControl;
  private Size storedSize;

  /// <summary>
  /// Possible drop states
  /// </summary>
  protected DropStateOption DropState { get; private set; }

  private string _Text;
  /// <summary>
  /// Text displayed in the drop down
  /// </summary>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  /// <summary>
  /// Text displayed in the drop down
  /// </summary>
  public new string Text
  {
    get => _Text;
    set { if (value.AssignIfNotEqual(ref _Text)) Invalidate(); }
  }

  /// <summary>
  /// Creates a new instance
  /// </summary>
  public DropDownControl()
  {
    InitializeComponent();
    storedSize = Size;
    BackColor = Color.White;
    Text = Name;

    //HACK: Added to appease CS8618 x must contain a not-null value when exiting constructor
    dropContainer = null!;
    PropertyChanged = null!;
    _dropDownItem = null!;
    _Text = Text;
  }

  /// <summary>
  /// IMPORTANT -- Call this after InitializeComponents()
  /// </summary>
  /// <param name="dropDownItem"></param>
  public void InitializeDropDown(Control dropDownItem)
  {
    if (_dropDownItem != null) throw new Exception("The drop down item has already been implemented!");
    _DesignView = false;
    DropState = DropStateOption.Closed;
    Size = _AnchorSize;
    AnchorClientBounds = new Rectangle(2, 2, _AnchorSize.Width - 21, _AnchorSize.Height - 4);
    //removes the dropDown item from the controls list so it 
    //won't be seen until the drop-down window is active
    if (Controls.Contains(dropDownItem)) Controls.Remove(dropDownItem);
    _dropDownItem = dropDownItem;
  }

  private Size _AnchorSize = new(121, 21);

  /// <summary>
  /// Anchor size
  /// </summary>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  /// <summary>
  /// Anchor size
  /// </summary>
  public Size AnchorSize
  {
    get => _AnchorSize;
    set { if (value.AssignIfNotEqual(ref _AnchorSize)) Invalidate(); }
  }

  /// <summary>
  /// Dock side
  /// </summary>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  /// <summary>
  /// Dock side
  /// </summary>
  public DockSideOption DockSide { get; set; }


  private bool _DesignView = true;

  /// <summary>
  /// Design view
  /// </summary>
  [DefaultValue(false)]
  protected bool DesignView
  {
    get => _DesignView;
    set
    {
      if (!value.AssignIfNotEqual(ref _DesignView)) return;

      if (_DesignView) Size = storedSize;
      else
      {
        storedSize = Size;
        Size = _AnchorSize;
      }

    }
  }

  /// <summary>
  /// Property changed event
  /// </summary>
  public event EventHandler PropertyChanged;

  /// <summary>
  /// Fires the <see cref="OnPropertyChanged"/> event.
  /// </summary>
  protected void OnPropertyChanged() => PropertyChanged?.Invoke(this, new EventArgs());

  /// <summary>
  /// Not sure
  /// </summary>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  /// <summary>
  /// Not sure
  /// </summary>
  public Rectangle AnchorClientBounds { get; private set; }

  /// <summary>
  /// Overrides the <see cref="OnResize(EventArgs)"/> method.
  /// </summary>
  /// <param name="e"></param>
  protected override void OnResize(EventArgs e)
  {
    base.OnResize(e);
    _AnchorSize.Width = Width;
    if (_DesignView) storedSize = Size;
    else
    {
      _AnchorSize.Height = Height;
      AnchorClientBounds = new Rectangle(2, 2, _AnchorSize.Width - 21, _AnchorSize.Height - 4);
    }
    //[14/02/19] Added to fix 'streaking' redraw issue when resizing - e.g. when anchored on resizable form.
    Invalidate();
    //
  }

  /// <summary>
  /// Not sure
  /// </summary>
  protected bool mousePressed;

  /// <summary>
  /// Overrides <see cref="OnMouseDown(MouseEventArgs)"/>
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseDown(MouseEventArgs e)
  {
    base.OnMouseDown(e);
    mousePressed = true;
    OpenDropDown();
  }

  /// <summary>
  /// Overrides <see cref="OnMouseUp(MouseEventArgs)"/>
  /// </summary>
  /// <param name="e"></param>
  protected override void OnMouseUp(MouseEventArgs e)
  {
    base.OnMouseUp(e);
    mousePressed = false;
    Invalidate();
  }

  /// <summary>
  /// Determines if the drop down can drop
  /// </summary>
  protected virtual bool CanDrop
  {
    get
    {
      if (dropContainer != null) return false;

      if (dropContainer == null && closedWhileInControl)
      {
        closedWhileInControl = false;
        return false;
      }

      return !closedWhileInControl;
    }
  }

  /// <summary>
  /// Opens the drop down
  /// </summary>
  protected void OpenDropDown()
  {
    if (_dropDownItem == null)
      throw new NotImplementedException("The drop down item has not been initialized!  Use the InitializeDropDown() method to do so.");

    if (!CanDrop) return;

    dropContainer = new DropDownContainer(_dropDownItem) { Bounds = GetDropDownBounds() };
    dropContainer.DropStateChange += new DropDownContainer.DropWindowArgs(DropContainer_DropStateChange);
    dropContainer.FormClosed += new FormClosedEventHandler(DropContainer_Closed);
    ParentForm!.Move += new EventHandler(ParentForm_Move);
    DropState = DropStateOption.Dropping;
    dropContainer.Show(this);
    DropState = DropStateOption.Dropped;
    Invalidate();
    DropDownOpened?.Invoke(this, EventArgs.Empty);
  }

  private void ParentForm_Move(object? sender, EventArgs e) => dropContainer.Bounds = GetDropDownBounds();

  /// <summary>
  /// Closes the drop down
  /// </summary>
  public void CloseDropDown()
  {

    if (dropContainer != null)
    {
      DropState = DropStateOption.Closing;
      dropContainer.Freeze = false;
      dropContainer.Close();
    }
  }

  private void DropContainer_DropStateChange(DropStateOption state) => DropState = state;

  private void DropContainer_Closed(object? sender, FormClosedEventArgs e)
  {
    if (!dropContainer.IsDisposed)
    {
      dropContainer.DropStateChange -= DropContainer_DropStateChange;
      dropContainer.FormClosed -= DropContainer_Closed;
      ParentForm!.Move -= ParentForm_Move;
      dropContainer.Dispose();
    }
    dropContainer = null!;
    closedWhileInControl = RectangleToScreen(ClientRectangle).Contains(Cursor.Position);
    DropState = DropStateOption.Closed;
    Invalidate();
    DropDownClosed?.Invoke(this, new EventArgs());
  }

  /// <summary>
  /// Determines the size for the drop down
  /// </summary>
  /// <returns></returns>
  protected virtual Rectangle GetDropDownBounds()
  {
    Size inflatedDropSize = new(_dropDownItem.Width + 2, _dropDownItem.Height + 2);
    var screenBounds = DockSide == DockSideOption.Left
      ? new Rectangle(this!.Parent!.PointToScreen(new Point(this.Bounds.X, this.Bounds.Bottom)), inflatedDropSize)
      : new Rectangle(this!.Parent!.PointToScreen(new Point(this.Bounds.Right - _dropDownItem.Width, this.Bounds.Bottom)), inflatedDropSize);

    var workingArea = Screen.GetWorkingArea(screenBounds);

    //make sure we're completely in the top-left working area
    if (screenBounds.X < workingArea.X) screenBounds.X = workingArea.X;
    if (screenBounds.Y < workingArea.Y) screenBounds.Y = workingArea.Y;

    //make sure we're not extended past the working area's right /bottom edge
    if (screenBounds.Right > workingArea.Right && workingArea.Width > screenBounds.Width) screenBounds.X = workingArea.Right - screenBounds.Width;
    if (screenBounds.Bottom > workingArea.Bottom && workingArea.Height > screenBounds.Height) screenBounds.Y = workingArea.Bottom - screenBounds.Height;

    return screenBounds;
  }


  /// <summary>
  /// Overrides <see cref="OnPaint(PaintEventArgs)"/> 
  /// </summary>
  /// <param name="e"></param>
  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);

    //Check if VisualStyles are supported...
    //Thanks to codeproject member: Mathiyazhagan for catching this. :)
    if (ComboBoxRenderer.IsSupported)
    {
      ComboBoxRenderer.DrawTextBox(e.Graphics, new Rectangle(new Point(0, 0), _AnchorSize), GetState());
      ComboBoxRenderer.DrawDropDownButton(e.Graphics, new Rectangle(_AnchorSize.Width - 19, 2, 18, _AnchorSize.Height - 4), GetState());
    }
    else
    {
      ControlPaint.DrawComboButton(e.Graphics, new Rectangle(_AnchorSize.Width - 19, 2, 18, _AnchorSize.Height - 4),
          Enabled ? ButtonState.Normal : ButtonState.Inactive);
    }

    using (Brush b = new SolidBrush(BackColor)) e.Graphics.FillRectangle(b, AnchorClientBounds);

    TextRenderer.DrawText(e.Graphics, _Text, Font, AnchorClientBounds, ForeColor, TextFormatFlags.WordEllipsis);
  }

  private System.Windows.Forms.VisualStyles.ComboBoxState GetState()
  {
    return mousePressed || dropContainer != null
      ? System.Windows.Forms.VisualStyles.ComboBoxState.Pressed
      : System.Windows.Forms.VisualStyles.ComboBoxState.Normal;
  }

  /// <summary>
  /// Freezes the drop down
  /// </summary>
  /// <param name="remainVisible"></param>
  public void FreezeDropDown(bool remainVisible)
  {
    if (dropContainer != null)
    {
      dropContainer.Freeze = true;
      if (!remainVisible) dropContainer.Visible = false;
    }
  }

  /// <summary>
  /// Unfreezes the drop down
  /// </summary>
  public void UnFreezeDropDown()
  {
    if (dropContainer != null)
    {
      dropContainer.Freeze = false;
      if (!dropContainer.Visible) dropContainer.Visible = true;
    }
  }

  internal sealed partial class DropDownContainer : Form, IMessageFilter
  {
    public bool Freeze;


    public DropDownContainer(Control dropDownItem)
    {
      FormBorderStyle = FormBorderStyle.None;
      dropDownItem.Location = new Point(1, 1);
      Controls.Add(dropDownItem);
      StartPosition = FormStartPosition.Manual;
      ShowInTaskbar = false;
      Application.AddMessageFilter(this);
    }

    public bool PreFilterMessage(ref Message m)
    {
      if (!Freeze && Visible && (ActiveForm == null || !ActiveForm.Equals(this)))
      {
        OnDropStateChange(DropStateOption.Closing);
        Close();
      }


      return false;
    }

    public delegate void DropWindowArgs(DropStateOption state);
    public event DropWindowArgs? DropStateChange;
    public void OnDropStateChange(DropStateOption state) => DropStateChange?.Invoke(state);

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1));
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Application.RemoveMessageFilter(this);
      Controls.RemoveAt(0); //prevent the control from being disposed
      base.OnClosing(e);
    }
  }
}
