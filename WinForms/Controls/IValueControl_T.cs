namespace PW.WinForms.Controls;


///// <summary>
///// Represents a custom WinForms Control with a Value property of type <typeparamref name="TValue"/>
///// Useful with <see cref="PW.WinForms.DataBinding.Binder{TDataSource}"/>. Saves overloads for each and every custom control.
///// </summary>
///// <typeparam name="TValue"></typeparam>
///// <typeparam name="TControl"></typeparam>
//[Obsolete("Use either IIntValueControl or  ILongValueControl.")]  
//public interface IValueControl<TValue, TControl> where TControl : Control
//{
//  /// <summary>
//  /// The value of the control.
//  /// </summary>
//  TValue Value { get; set; }
//}

/// <summary>
/// Represents a custom WinForms Control with a Value property of type <see cref="int"/>
/// Useful with <see cref="PW.WinForms.DataBinding.Binder{TDataSource}"/>. Saves overloads for each and every custom control.
/// </summary>
public interface IValueControl
{

  /// <summary>
  /// The value of the control.
  /// </summary>
  int Value { get; set; }
}

/// <summary>
/// Represents a custom WinForms Control with a Value property of type <see cref="long"/>
/// Useful with <see cref="PW.WinForms.DataBinding.Binder{TDataSource}"/>. Saves overloads for each and every custom control.
/// </summary>
public interface ILongValueControl
{

  /// <summary>
  /// The value of the control.
  /// </summary>
  long Value { get; set; }
}
