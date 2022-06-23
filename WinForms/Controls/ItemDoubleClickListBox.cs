#nullable enable

using System;
using System.Windows.Forms;

namespace PW.WinForms.Controls
{
  /// <summary>
  /// Extended version of <see cref="ListBox"/> control with <see cref="ItemDoubleClick"/> event.
  /// </summary>
  public class ItemDoubleClickListBox : ListBox
  {
    /// <summary>
    /// ItemDoubleClickEventArgs
    /// </summary>
    public class ItemDoubleClickEventArgs : EventArgs
    {
      internal ItemDoubleClickEventArgs(object item, Keys modifierKeys)
      {
        Item = item;
        ModifierKeys = modifierKeys;
      }

      /// <summary>
      /// The item that was double-clicked.
      /// </summary>
      public object Item { get; }

      /// <summary>
      /// See: <see cref="Control.ModifierKeys"/>
      /// </summary>
      public Keys ModifierKeys { get; }
    }

    /// <summary>
    /// ItemDoubleClick event handler delegate
    /// </summary>
    public delegate void ItemDoubleClickHandler(object sender, ItemDoubleClickEventArgs e);

    /// <summary>
    /// Invoked when a list item is double-clicked.
    /// </summary>
    public event ItemDoubleClickHandler? ItemDoubleClick;

    /// <summary>
    /// Overridden to implement item double-click event.
    /// </summary>
    protected override void OnMouseDoubleClick(MouseEventArgs e)
    {
      base.OnMouseDoubleClick(e);

      // Only do hit-testing etc. when the double-click event is being handled.
      if (ItemDoubleClick is ItemDoubleClickHandler handler && IndexFromPoint(e.Location) is int index && index != NoMatches)
        handler.Invoke(this, new ItemDoubleClickEventArgs(Items[index], ModifierKeys));
    }

  }
}
