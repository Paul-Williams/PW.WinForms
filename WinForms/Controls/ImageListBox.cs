

// See: https://stackoverflow.com/questions/472897/c-sharp-can-i-display-images-in-a-list-box

/* TODO:
 * Remove items from dictionary when list content changes.
 * Use BackColor brush when filling item background rectangle.
 */

namespace PW.WinForms.Controls;

/// <summary>
/// A custom ListBox which displays an image for each item in the list.
/// </summary>
public class ImageListBox : ListBox
{
  private const int PicHeight = 120;
  private const int PicPadding = 4;
  private const int PicBottomMargin = 4;

  private readonly Dictionary<IPreviewImage, Image> Thumbs = [];

  private const int SelectionRectanglePenWidth = 4;
  private Pen SelectedItemBorderPen { get; } = new(SystemColors.HotTrack, SelectionRectanglePenWidth);
  private Pen? NotSelectedItemBorderPen { get; set; }
  private Brush? BackgroundBrush { get; set; }
  private System.Windows.Forms.Timer CleanupTimer { get; }

  /// <summary>
  /// Creates a new instance
  /// </summary>
  public ImageListBox()
  {
    DrawMode = DrawMode.OwnerDrawFixed;
    ItemHeight = PicHeight + (PicPadding * 2) + PicBottomMargin;
    IntegralHeight = false;
    BorderStyle = BorderStyle.None;

    CleanupTimer = new System.Windows.Forms.Timer();
    CleanupTimer.Tick += (o, ea) => CleanupThumbs();
    CleanupTimer.Interval = 1000;
  }

  /// <summary>
  /// Disposes and frees resources
  /// </summary>
  /// <param name="disposing"></param>
  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);

    if (CleanupTimer is System.Windows.Forms.Timer local)
    {
      local.Enabled = false;
      local.Dispose();
    }
  }


  /// <summary>
  /// Overrides <see cref="OnDrawItem(DrawItemEventArgs)"/>
  /// </summary>
  /// <param name="e"></param>
  protected override void OnDrawItem(DrawItemEventArgs e)
  {
    if (e.Index < 0 || Items.Count == 0) return;
    ItemHeight = PicHeight + (PicPadding * 2) + PicBottomMargin;

    BackgroundBrush ??= new SolidBrush(BackColor);
    NotSelectedItemBorderPen ??= new Pen(Color.White, SelectionRectanglePenWidth);
    e.Graphics.FillRectangle(BackgroundBrush, e.Bounds);

    if (Items[e.Index] is IPreviewImage ip)
    {

      if (GetPreview(ip) is Image preview)
      {
        var imageTopLeft = GetImageDrawPoint(e.Bounds, preview.Width);
        e.Graphics.DrawImage(preview, imageTopLeft);
        var selectionRect = GetImageBoundsRectangle(imageTopLeft, preview.Size);
        e.Graphics.DrawRectangle(IsSelected(e.State) ? SelectedItemBorderPen : NotSelectedItemBorderPen, selectionRect);
      }
      else
      {
        e.Graphics.DrawString(" Image missing", new Font("Arial", 10), Brushes.Red, new Point(e.Bounds.X + 5, e.Bounds.Y));
        e.Graphics.DrawRectangle(IsSelected(e.State) ? SelectedItemBorderPen : NotSelectedItemBorderPen, e.Bounds);
      }
    }
    else
    {
      e.Graphics.DrawString("Item " + e.Index.ToString() + " does not implement " + nameof(IPreviewImage), new Font("Arial", 10), Brushes.DarkRed, new Point(e.Bounds.X + 5, e.Bounds.Y));
      e.Graphics.DrawRectangle(IsSelected(e.State) ? SelectedItemBorderPen : NotSelectedItemBorderPen, e.Bounds);
    }



  }

  private static PointF GetImageDrawPoint(Rectangle listItemBounds, int imageWidth)
  {
    var x = (listItemBounds.Width - imageWidth) / 2;
    var y = listItemBounds.Y + PicPadding;
    return new PointF(x, y);
  }

  private static Rectangle GetImageBoundsRectangle(PointF topLeft, Size imageSize) => new(
        (int)topLeft.X - (SelectionRectanglePenWidth / 2),
        (int)topLeft.Y - (SelectionRectanglePenWidth / 2),
        imageSize.Width + SelectionRectanglePenWidth,
        imageSize.Height + SelectionRectanglePenWidth);


  private static bool IsSelected(DrawItemState state) => (state & DrawItemState.Selected) == DrawItemState.Selected;

  private void CleanupThumbs() => Thumbs.Keys.ToArray().Where(key => !Items.Contains(key)).ForEach(x => Thumbs.Remove(x));//var keys = Thumbs.Keys.ToArray();//foreach (var key in keys)//{//  if (Items.Contains(key) == false)//  {//    Thumbs.Remove(key);//  }//}

  private Image GetPreview(IPreviewImage ip)
  {
    if (!Thumbs.TryGetValue(ip, out var thumb))
    {
      using var img = ip.Image;
      if (img != null)
      {
        var scale = (double)(PicHeight - (PicPadding * 2)) / (img.Height - (PicPadding * 2));
        thumb = img.Resize((int)(img.Width * scale), PicHeight - (PicPadding * 2));
        Thumbs.Add(ip, thumb);
        if (!CleanupTimer.Enabled) CleanupTimer.Enabled = true;
      }
      else
      {
        throw new Exception(nameof(IPreviewImage) + " image is null.");
      }
    }

    return thumb;
  }

  /// <summary>
  /// Overrides <see cref="OnResize(EventArgs)"/>
  /// </summary>
  /// <param name="e"></param>
  protected override void OnResize(EventArgs e)
  {
    Invalidate();
    base.OnResize(e);
  }

}
