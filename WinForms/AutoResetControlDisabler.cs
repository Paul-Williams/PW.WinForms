using PW.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PW.WinForms;

/// <summary>
/// Disables controls and restores their previous enabled state when disposed. Useful within a 'using' block when app is processing.
/// </summary>
public sealed class AutoResetControlDisabler : IDisposable
{
  private Dictionary<Control, bool> OriginalControlEnabledStates { get; } = new Dictionary<Control, bool>();
  private System.Threading.Timer? DisableControlsTimer { get; set; }
  private bool ReenableRequired;
  private SynchronizationContext? SC { get; }


  private AutoResetControlDisabler(IEnumerable<Control> controls, TimeSpan wait = default)
  {
    SC = SynchronizationContext.Current;
    OriginalControlEnabledStates = controls.ToDictionary(x => x, x => x.Enabled);

    if (wait == default)
    {
      ControlsWithOriginalEnabledState(true).ForEach(c => c.Enabled = false);
      ReenableRequired = true;
    }

    else
      DisableControlsTimer = new System.Threading.Timer(WaitTimerCallback, null, wait, TimeSpan.FromMilliseconds(-1));
  }

  private void WaitTimerCallback(object? state)
  {
    DisableControlsTimer?.Dispose();
    DisableControlsTimer = null;
    ReenableRequired = true;

    if (SC is not null && SC != SynchronizationContext.Current)
      SC.Post(new SendOrPostCallback((o) => ControlsWithOriginalEnabledState(true).ForEach(c => c.Enabled = false)), null);
    else
      ControlsWithOriginalEnabledState(true).ForEach(c => c.Enabled = false);


  }

  private IEnumerable<Control> ControlsWithOriginalEnabledState(bool value) =>
    OriginalControlEnabledStates
      .Where(p => p.Value == value)
      .Select(p => p.Key);


  private static IEnumerable<Control> GetControls(Form form) => form.Controls.OfType<Control>();

  /// <summary>
  /// Disable all controls
  /// </summary>
  public static AutoResetControlDisabler Disable(IEnumerable<Control> controls) => new(controls);

  /// <summary>
  /// Disable all controls in the collection. Useful for container controls.
  /// </summary>
  public static AutoResetControlDisabler Disable(Control.ControlCollection controls) => new(controls.OfType<Control>());

  /// <summary>
  /// Disable all controls on the <see cref="Form"/>
  /// </summary>
  public static AutoResetControlDisabler Disable(Form form) => new(GetControls(form));

  /// <summary>
  /// Disable all controls on the <see cref="Form"/>
  /// </summary>
  /// <param name="form"></param>
  /// <param name="wait">Period of time to wait before disabling controls.</param>
  /// <returns></returns>
  public static AutoResetControlDisabler Disable(Form form, TimeSpan wait) => new(GetControls(form), wait);


  /// <summary>
  /// Disable all controls except the specified control.
  /// </summary>
  public static AutoResetControlDisabler DisableExcept(Form form, Control exception) => new(GetControls(form).Where(x => x != exception));



  /// <summary>
  /// Restores the original enabled state of each control.
  /// </summary>
  public void Dispose()
  {
    if (DisableControlsTimer != null)
    {
      DisableControlsTimer.Change(Timeout.Infinite, Timeout.Infinite);
      DisableControlsTimer.Dispose();
      DisableControlsTimer = null;
    }
    if (ReenableRequired) ControlsWithOriginalEnabledState(true).ForEach(x => x.Enabled = true);
  }
}

