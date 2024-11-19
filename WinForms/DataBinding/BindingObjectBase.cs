
using System.ComponentModel;

namespace PW.WinForms.DataBinding;

/// <summary>
/// Base class for objects supporting <see cref="INotifyPropertyChanged"/>
/// </summary>
public abstract class BindingObjectBase : INotifyPropertyChanged
{
  /// <summary>
  /// Raised when a property changes
  /// </summary>
  public event PropertyChangedEventHandler? PropertyChanged;

  /// <summary>
  /// Raises the <see cref="PropertyChangedEventHandler"/>
  /// </summary>
  /// <param name="propertyName"></param>
  protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}
