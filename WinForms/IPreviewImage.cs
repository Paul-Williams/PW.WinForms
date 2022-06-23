#nullable enable

using System.Drawing;

namespace PW.WinForms
{
  /// <summary>
  /// Implemented by objects which can provide a preview image
  /// </summary>
  public interface IPreviewImage
  {
    /// <summary>
    /// Returns a preview image
    /// </summary>
    Image Image { get; } 
  }


}
