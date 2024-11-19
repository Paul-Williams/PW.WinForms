using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

// See: https://www.doogal.co.uk/transparent.php

namespace PW.WinForms.Controls;

/// <summary>
/// A label that can be transparent.
/// </summary>
public partial class TransparentLabel : Control

{
  /// <summary>
  /// Creates a new <see cref="TransparentLabel"/> instance.
  /// </summary>
  public TransparentLabel() => TabStop = false;

  /// <summary>
  /// Gets the creation parameters.
  /// </summary>
  protected override CreateParams CreateParams
  {
    get
    {
      var cp = base.CreateParams;
      cp.ExStyle |= 0x20;
      return cp;
    }
  }

  ///// <summary>
  ///// Paints the background.
  ///// </summary>
  //protected override void OnPaintBackground(PaintEventArgs e)
  //{
  //  // do nothing
  //}

  /// <summary>
  /// Paints the control.
  /// </summary>
  protected override void OnPaint(PaintEventArgs e) => DrawText();

  /// <summary>
  /// Custom message handler.
  /// </summary>    
  protected override void WndProc(ref Message m)
  {
    const int WM_PAINT = 0x000F;

    base.WndProc(ref m);
    if (m.Msg == WM_PAINT) DrawText();
  }

  private void DrawText()
  {
    if (Text is null || Text.Length == 0) return;

    using var graphics = CreateGraphics();
    using SolidBrush brush = new(ForeColor);
    var size = graphics.MeasureString(Text, Font);

    // first figure out the top
    float top = 0;
    switch (TextAlign)
    {
      case ContentAlignment.MiddleLeft:
      case ContentAlignment.MiddleCenter:
      case ContentAlignment.MiddleRight:
        top = (Height - size.Height) / 2;
        break;
      case ContentAlignment.BottomLeft:
      case ContentAlignment.BottomCenter:
      case ContentAlignment.BottomRight:
        top = Height - size.Height;
        break;
      case ContentAlignment.TopLeft:
        break;
      case ContentAlignment.TopCenter:
        break;
      case ContentAlignment.TopRight:
        break;
      default:
        break;
    }

    float left = -1;
    switch (TextAlign)
    {
      case ContentAlignment.TopLeft:
      case ContentAlignment.MiddleLeft:
      case ContentAlignment.BottomLeft:
        left = RightToLeft == RightToLeft.Yes ? Width - size.Width : -1;
        break;
      case ContentAlignment.TopCenter:
      case ContentAlignment.MiddleCenter:
      case ContentAlignment.BottomCenter:
        left = (Width - size.Width) / 2;
        break;
      case ContentAlignment.TopRight:
      case ContentAlignment.MiddleRight:
      case ContentAlignment.BottomRight:
        left = RightToLeft == RightToLeft.Yes ? -1 : Width - size.Width;
        break;
      default:
        break;
    }
    graphics.DrawString(Text, Font, brush, left, top);
  }

  /// <summary>
  /// Gets or sets the text associated with this control.
  /// </summary>
  /// <returns>
  /// The text associated with this control.
  /// </returns>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  /// <summary>
  /// Gets or sets the text associated with this control.
  /// </summary>
  /// <returns>
  /// The text associated with this control.
  /// </returns>
  [AllowNull]
  public override string Text
  {
    get => base.Text;
    set
    {
      base.Text = value ?? string.Empty;
      RecreateHandle();
    }
  }

  /// <summary>
  /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
  /// </summary>
  /// <value></value>
  /// <returns>
  /// One of the <see cref="T:System.Windows.Forms.RightToLeft"/> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit"/>.
  /// </returns>
  /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
  /// The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft"/> values.
  /// </exception>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  /// <summary>
  /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
  /// </summary>
  /// <value></value>
  /// <returns>
  /// One of the <see cref="T:System.Windows.Forms.RightToLeft"/> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit"/>.
  /// </returns>
  /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
  /// The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft"/> values.
  /// </exception>
  public override RightToLeft RightToLeft
  {
    get => base.RightToLeft;
    set
    {
      base.RightToLeft = value;
      RecreateHandle();
    }
  }

  /// <summary>
  /// Gets or sets the font of the text displayed by the control.
  /// </summary>
  /// <value></value>
  /// <returns>
  /// The <see cref="T:System.Drawing.Font"/> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"/> property.
  /// </returns>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  /// <summary>
  /// Gets or sets the font of the text displayed by the control.
  /// </summary>
  /// <value></value>
  /// <returns>
  /// The <see cref="T:System.Drawing.Font"/> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"/> property.
  /// </returns>
  [AllowNull]
  public override Font Font
  {
    get => base.Font;
    set
    {
      base.Font = value;
      RecreateHandle();
    }
  }

  /// <summary>
  /// Gets or sets the text alignment.
  /// </summary>
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  /// <summary>
  /// Gets or sets the text alignment.
  /// </summary>
  public ContentAlignment TextAlign
  {
    get;
    set
    {
      field = value;
      RecreateHandle();
    }
  } = ContentAlignment.TopLeft;
}