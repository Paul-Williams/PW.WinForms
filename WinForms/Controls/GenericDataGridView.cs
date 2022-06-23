#nullable enable

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PW.WinForms.Controls
{

  /// <summary>
  /// Generic (typed) version of the standard <see cref="DataGridView"/> control.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class GenericDataGridView<T> : DataGridView
  {

    /// <summary>
    /// Creates a new instance of the control
    /// </summary>
    public GenericDataGridView()
    {
      SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      RowHeadersVisible = false;
      BackgroundColor = Color.White;
      Dock = DockStyle.Fill;
      AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      AllowUserToResizeRows = false;
      AllowUserToResizeColumns = true;
    }

    /// <summary>
    /// Returns the first select data bound item.
    /// </summary>
    public T? SelectedItem()
    {

      return SelectedItems() is IReadOnlyList<T> rows && rows.Count != 0 ? rows[0] : default;

    }

    /// <summary>
    /// Returns all selected data bound items.
    /// </summary>
    public IReadOnlyList<T> SelectedItems()
    {
      return SelectedRows is DataGridViewSelectedRowCollection rows && rows.Count != 0
        ? rows.OfType<DataGridViewRow>().Select(x => (T)x.DataBoundItem).ToArray()
        : Array.Empty<T>();

    }

  }
}
