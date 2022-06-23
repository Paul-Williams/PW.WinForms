#nullable enable

using System;
using System.Windows.Forms;

namespace PW.WinForms.DataBinding
{
  public partial class Binder<TDataSource> where TDataSource : class
  {
    /// <summary>
    /// Binds the <see cref="RadioButton"/> to a boolean value on the controller.
    /// </summary>
    public void BindChecked(RadioButton control, string to, bool initialValue)
    {
      if (control is null) { throw new System.ArgumentNullException(nameof(control)); }
      if (string.IsNullOrWhiteSpace(to)) { throw new System.ArgumentException($"'{nameof(to)}' cannot be null or whitespace", nameof(to)); }

      var b = new Binding(nameof(control.Checked), DataSource, to, true, DataSourceUpdateMode)
      {

        // RadioButtons automatically have their value set, as a group, within a container.
        // If the binding is not set to never, then the selected radio button cannot be changed by clicking in the UI.
        // A side-effect of this is that the control does not get initialized to the binding.
        // Therefore we use the initialValue argument to set one to true, as required.
        ControlUpdateMode = ControlUpdateMode.Never
      };
      control.DataBindings.Add(b);
      if (initialValue) control.Checked = true;
    }

    /// <summary>
    /// Performs an action on the controller when the <see cref="RadioButton"/> is clicked.
    /// </summary>
    public void BindClick(RadioButton control, Action<TDataSource> action)
    {
      if (control is null) { throw new ArgumentNullException(nameof(control)); }
      if (action is null) { throw new ArgumentNullException(nameof(action)); }
      
      control.Click += (s,e) => action.Invoke(DataSource);

    }

  }
}
