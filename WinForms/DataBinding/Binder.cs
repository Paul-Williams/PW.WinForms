using PW.FailFast;
using PW.WinForms.Controls;

namespace PW.WinForms.DataBinding;

/// <summary>
/// Used to bind a single data source to one or more controls.
/// Is just a simple wrapper around standard binding, but with simpler syntax.
/// This class will be extended as requirements arise for more controls and properties to be bound.
/// </summary>
/// <typeparam name="TDataSource"></typeparam>
public partial class Binder<TDataSource> where TDataSource : class
{
  /// <summary>
  /// Creates a new instance
  /// </summary>
  /// <param name="dataSource">Data source used when binding controls.</param>
  /// <param name="dataSourceUpdateMode">Specifies when a data source is updated when changes occur in the bound control.</param>
  public Binder(TDataSource dataSource, DataSourceUpdateMode dataSourceUpdateMode)
  {
    Guard.NotNull(dataSource, nameof(dataSource));
    DataSource = dataSource;
    DataSourceUpdateMode = dataSourceUpdateMode;
  }

  /// <summary>
  /// Creates a new instance with <see cref="DataSourceUpdateMode.OnPropertyChanged"/>
  /// </summary>
  /// <param name="dataSource">Data source used when binding controls.</param>
  public Binder(TDataSource dataSource) : this(dataSource, DataSourceUpdateMode.OnPropertyChanged) { }


  /// <summary>
  /// Data source used by this binder instance.
  /// </summary>
  public TDataSource DataSource { get; }

  /// <summary>
  /// 
  /// </summary>
  public void BindChecked(CheckBox control, string to)
  {
    ArgumentNullException.ThrowIfNull(control);
    ArgumentNullException.ThrowIfNull(to);


    var b = new Binding(nameof(control.Checked), DataSource, to, true, DataSourceUpdateMode);
    control.DataBindings.Add(b);
  }

  ///// <summary>
  ///// 
  ///// </summary>
  ///// <param name="control"></param>
  ///// <param name="to"></param>
  ///// <param name="initialValue"></param>
  //public void BindChecked(RadioButton control, string to, bool initialValue)
  //{
  //  Guard.NotNull(control, nameof(control));
  //  Guard.NotNull(to, nameof(to));

  //  var b = new Binding(nameof(control.Checked), DataSource, to, true, DataSourceUpdateMode)
  //  {

  //    // RadioButtons automatically have their value set, as a group, within a container.
  //    // If the binding is not set to never, then the selected radio button cannot be changed by clicking in the UI.
  //    // A side-effect of this is that the control does not get initialized to the binding.
  //    // Therefore we use the initialValue argument to set one to true, as required.
  //    ControlUpdateMode = ControlUpdateMode.Never
  //  };
  //  control.DataBindings.Add(b);
  //  if (initialValue) control.Checked = true;
  //}


  ///// <summary>
  ///// To support binding of custom controls which implement <see cref="Controls.IValueControl{TValue, TControl}"/>.
  ///// </summary>
  //[Obsolete]
  //public void BindValue<TValue, TControl>(Controls.IValueControl<TValue, TControl> control, string to) where TControl : Control
  //{
  //  Guard.NotNull(control, nameof(control));
  //  Guard.NotNull(to, nameof(to));

  //  var b = new Binding(nameof(control.Value), DataSource, to, true, DataSourceUpdateMode);
  //  ((Control)control).DataBindings.Add(b);
  //}


  /// <summary>
  /// To support binding of custom controls which implement <see cref="IValueControl"/>.
  /// </summary>    
  public void BindValue(IValueControl control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));

    var b = new Binding(nameof(control.Value), DataSource, to, true, DataSourceUpdateMode);
    ((Control)control).DataBindings.Add(b);
  }

  /// <summary>
  /// To support binding of custom controls which implement <see cref="ILongValueControl"/>.
  /// </summary>    
  public void BindValue(ILongValueControl control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));

    var b = new Binding(nameof(control.Value), DataSource, to, true, DataSourceUpdateMode);
    ((Control)control).DataBindings.Add(b);
  }


  private DataSourceUpdateMode DataSourceUpdateMode { get; }


  /// <summary>
  /// Binds the <see cref="DataGridView"/> to a sub-object of <typeparamref name="TDataSource"/>.
  /// </summary>
  public void BindGrid(DataGridView control, Func<TDataSource, object> toList)
  {
    control.DataSource = null;
    control.DataSource = toList.Invoke(DataSource);
  }

  /// <summary>
  /// Binds the <see cref="DataGridView"/> to <typeparamref name="TDataSource"/>.
  /// </summary>
  public void BindGrid(DataGridView control)
  {
    control.DataSource = null;
    control.DataSource = DataSource;
  }

  /// <summary>
  /// Binds the control's click event to a method on the data source which takes a single parameter of type <typeparamref name="TArg"/>.
  /// </summary>
  public void BindClick<TArg>(Control control, Func<TDataSource, Action<TArg>> to) => control.Click += (o, e) => to.Invoke(DataSource);


  /// <summary>
  /// Binds the control's click event to an Action which takes the <typeparamref name="TDataSource"/> as a parameter.
  /// </summary>
  public void BindClick(Control control, Action<TDataSource> to) => control.Click += (o, e) => to.Invoke(DataSource);

  /// <summary>
  /// Binds the control's click event to a parameterless method on the data source.
  /// </summary>
  /// <param name="control">Control for which the click event will be bound.</param>
  /// <param name="to">Function returning the method on the data source to which to bind.</param>
  public void BindClick(Control control, Func<TDataSource, Action> to) => control.Click += (o, e) => to.Invoke(DataSource)();


  /// <summary>
  /// Binds the control's click-event to a parameterless method and it's enabled-state to a property on the data source.
  /// </summary>
  /// <param name="control">Control for which the click event will be bound.</param>
  /// <param name="toAction">Function returning the method on the data source to which to bind.</param>
  /// <param name="toEnabled">Property on the data source used to set the enabled state of the control.</param>
  public void BindClick(Control control, Func<TDataSource, Action> toAction, string toEnabled)
  {
    control.Click += (o, e) => toAction.Invoke(DataSource)();
    BindEnabled(control, toEnabled);
  }


  /// <summary>
  /// Binds the list control to a sub-object on the data-source
  /// </summary>
  /// <param name="control">Control to be bound.</param>
  /// <param name="toList">Function returning the property of <typeparamref name="TDataSource"/> to which to bind.</param>
  /// <param name="toDisplayMember">Property of the data-source to display in the list.</param>
  public void BindList(ListControl control, Func<TDataSource, object> toList, string toDisplayMember)
  {
    control.DataSource = null;
    control.DataSource = toList.Invoke(DataSource);
    control.DisplayMember = toDisplayMember;
  }

  /// <summary>
  /// Binds the list control to a sub-object on the data-source
  /// </summary>
  /// <param name="control">Control to be bound.</param>
  /// <param name="toList">Function returning the property of <typeparamref name="TDataSource"/> to which to bind.</param>
  public void BindList(ListControl control, Func<TDataSource, object> toList)
  {
    control.DataSource = null;
    control.DataSource = toList.Invoke(DataSource);
  }



  /// <summary>
  /// Binds the <see cref="TextBoxBase.Lines"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindLines(TextBox control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));

    var b = new Binding(nameof(control.Lines), DataSource, to, true, DataSourceUpdateMode);
    control.DataBindings.Add(b);
  }

  /// <summary>
  /// Binds the <see cref="Control.ForeColor"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  /// <param name="converter"></param>
  public void BindForecolor(Control control, string to, ConvertEventHandler converter)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));
    Guard.NotNull(converter, nameof(converter));
    var b = new Binding(nameof(control.ForeColor), DataSource, to, true, DataSourceUpdateMode);
    b.Format += converter;
    control.DataBindings.Add(b);
  }

  //public void BindProperty(Action<TDataSource> property, string to)
  //{
  //  this.DataSource.PropertyChanged += (s, e) => { if (e.PropertyName == to) property.Invoke(DataSource); };
  //}

  //public void BindProperty<T>(Action<T> property, Func <TDataSource,T> func, string to)
  //{
  //  this.DataSource.PropertyChanged += (s, e) => { if (e.PropertyName == to) property.Invoke(func(DataSource)); };
  //}

  /// <summary>
  /// Binds the <see cref="Control.Enabled"/> property to the opposite of the value of <paramref name="to"/>.
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindNotEnabled(Control control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));

    var b = new Binding(nameof(control.Enabled), DataSource, to, true, DataSourceUpdateMode);
    b.Format += ConvertEventHandlers.NotBool;
    control.DataBindings.Add(b);
  }

  /// <summary>
  /// Binds the <see cref="Control.Enabled"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindEnabled(Control control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));

    var b = new Binding(nameof(control.Enabled), DataSource, to, true, DataSourceUpdateMode);
    control.DataBindings.Add(b);
  }

  /// <summary>
  /// Binds the <see cref="Control.Enabled"/> property
  /// </summary>
  /// <param name="controls"></param>
  /// <param name="to"></param>
  public void BindEnabled(IEnumerable<Control> controls, string to) => controls.ForEach(c => BindEnabled(c, to));

  /// <summary>
  /// Binds the <see cref="Control.Text"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindText(Control control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));

    var b = new Binding(nameof(control.Text), DataSource, to, true, DataSourceUpdateMode);
    control.DataBindings.Add(b);

  }

  /// <summary>
  /// Binds the <see cref="DateTimePicker.Value"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindValue(DateTimePicker control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));

    var b = new Binding(nameof(control.Value), DataSource, to, true, DataSourceUpdateMode);
    control.DataBindings.Add(b);
  }

  /// <summary>
  /// Binds the <see cref="NumericUpDown.Value"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindValue(NumericUpDown control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));
    var b = new Binding(nameof(control.Value), DataSource, to, true, DataSourceUpdateMode);
    control.DataBindings.Add(b);
  }

  /// <summary>
  /// Binds the <see cref="CheckBox.Checked"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindValue(CheckBox control, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));
    var b = new Binding(nameof(control.Checked), DataSource, to, true, DataSourceUpdateMode);
    control.DataBindings.Add(b);
  }

  /// <summary>
  /// Binds the <see cref="Control.Text"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  /// <param name="formatter"></param>
  /// <param name="parser"></param>
  public void BindText(Control control, string to, ConvertEventHandler formatter, ConvertEventHandler parser) =>
    BindText(control, to,
      formatter is null ? null : [formatter],
      parser is null ? null : [parser]);

  /// <summary>
  /// Binds the <see cref="Control.Text"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  /// <param name="formatters"></param>
  /// <param name="parsers"></param>
  public void BindText(Control control, string to, ConvertEventHandler[]? formatters, ConvertEventHandler[]? parsers)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));

    var b = new Binding(nameof(Control.Text), DataSource, to, true, DataSourceUpdateMode);
    if (formatters != null) foreach (var f in formatters) b.Format += f;
    if (parsers != null) foreach (var p in parsers) b.Parse += p;
    control.DataBindings.Add(b);

  }

  /// <summary>
  /// Binds the <see cref="ComboBox.SelectedItem"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindSelectedItem(ComboBox control, string to) => BindSelectionInternal(control, SelectedOptions.SelectedItem, to);

  /// <summary>
  /// Binds the <see cref="ListBox.SelectedItem"/> property
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindSelectedItem(ListBox control, string to) => BindSelectionInternal(control, SelectedOptions.SelectedItem, to);

  /// <summary>
  /// Binds the <see cref="ListControl.SelectedValue"/> property. 
  /// Also works for both <see cref="ComboBox"/> and <see cref="ListBox"/>.
  /// </summary>
  /// <param name="control"></param>
  /// <param name="to"></param>
  public void BindSelectedValue(ListControl control, string to) => BindSelectionInternal(control, SelectedOptions.SelectedValue, to);

  /// <summary>
  /// Enum for the supported 'Selected...' values supported by <see cref="BindSelectionInternal(Control, SelectedOptions, string)"/>
  /// NB: The enum value names must match property names
  /// </summary>
  private enum SelectedOptions
  {
    SelectedItem,
    SelectedValue
  }

  /// <summary>
  /// Handles binding ListBox and ComboBox SelectedItem and SelectedValue properties.
  /// </summary>
  private void BindSelectionInternal(Control control, SelectedOptions selectedOptions, string to)
  {
    Guard.NotNull(control, nameof(control));
    Guard.NotNull(to, nameof(to));
    var b = new Binding(selectedOptions.ToString(), DataSource, to, true, DataSourceUpdateMode);
    control.DataBindings.Add(b);
  }

}
