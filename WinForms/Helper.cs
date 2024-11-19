namespace PW.WinForms;

/// <summary>
/// Static helpers
/// </summary>
public static class Helper
{
  /// <summary>
  /// Whether or not we are running in design mode.
  /// </summary>
  public static bool IsDesignMode =>
    System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime;
}
