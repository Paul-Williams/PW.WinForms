# nullable enable

using System.Drawing;
using System.Windows.Forms;

namespace PW.WinForms.Controls
{
  /// <summary>
  /// TextBox is a Cue Banner.
  /// </summary>
  public class CueBannerTextBox : TextBox
  {
    //const int WM_PASTE = 0x302;
    private const int WM_PAINT = 0xF;


    /// <summary>
    /// Text to display as Cue Banner.
    /// </summary>
    public string? CueBannerText { get; set; }

    /// <summary>
    /// WndProc
    /// </summary>
    /// <param name="m"></param>
    [System.Diagnostics.DebuggerStepThrough]
    protected override void WndProc(ref Message m)
    {

      // Allow normal draw first
      base.WndProc(ref m);

      // Override 'paint' messages: Display 'Cue Banner' text when appropriate.
      if (m.Msg == WM_PAINT)
      {

        // Then add Cue Banner text
        if (/*!Focused &&*/ string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(CueBannerText))
        {
          using var graphics = CreateGraphics();
          TextRenderer.DrawText(
              dc: graphics,
              text: CueBannerText,
              font: Font,
              bounds: ClientRectangle,
              foreColor: SystemColors.GrayText,
              backColor: Enabled ? BackColor : SystemColors.Control,
              flags: Multiline ? TextFormatFlags.Top | TextFormatFlags.Left : TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }
        return;
      }

    }


  }
}
