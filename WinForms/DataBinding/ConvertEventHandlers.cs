#nullable enable

using PW.Extensions;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PW.WinForms.DataBinding
{
  /// <summary>
  /// This class contains common function for handling conversion of bound data.
  /// The class will need to be extended to handle additional conversions, as required.
  /// </summary>
  public static class ConvertEventHandlers
  {
    //static readonly Type StringType = typeof(string);

    //private static bool ValueIsNonNullString(this ConvertEventArgs e) => e.DesiredType == StringType && e.Value != null;


    /// <summary>
    /// Creates a <see cref="ConvertEventHandler"/> to convert a boolean value to a color.
    /// </summary>
    public static ConvertEventHandler BoolToColor(System.Drawing.Color trueColor, System.Drawing.Color falseColor)
      => (o, e) => { if (e.Value is bool value) e.Value = value == true ? trueColor : falseColor; };

    /// <summary>
    /// A binding event handler to switch bool value: true->false and false->true.
    /// </summary>
    public static void NotBool(object? sender, ConvertEventArgs e)
    {
      if (e.Value is bool value) e.Value = !value;
    }


    /// <summary>
    /// A binding event handler to convert empty and white-space string values back to null.
    /// </summary>
    public static void EmptyStringToNull(object? sender, ConvertEventArgs e)
    {
      //if (e.Value is null || e.DesiredType != typeof(string)) return;
      //if (string.IsNullOrWhiteSpace((string)e.Value)) e.Value = null;

      if (e.Value is string value && value.IsNullOrWhiteSpace()) e.Value = null;

    }

    /// <summary>
    /// A <see cref="ConvertEventHandler"/> which returns only the numbers from within a string.
    /// </summary>
    public static void JustNumbers(object? sender, ConvertEventArgs e)
    {
      if (e.Value is string value)
        e.Value = value.Where(c => char.IsDigit(c)).AsString();
    }

    /// <summary>
    /// A binding event handler to display 11-digit phone number as 00000-000000.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void NationalTelephoneNumber(object? sender, ConvertEventArgs e)
    {
      if (e.Value is string value && value.Length == 11)
        e.Value = value.Substring(0, 5) + "-" + value.Substring(5);
    }

    /// <summary>
    /// A binding event handler which ensures the string is all lower-case
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void ToLower(object? sender, ConvertEventArgs e)
    {
      if (e.Value is string value) e.Value = value.ToLower();
    }

    /// <summary>
    /// A binding event handler which converts <see cref="decimal"/> to <see cref="int"/>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void DecimalToInt(object? sender, ConvertEventArgs e)
    {
      if (e.Value is decimal value) e.Value = Convert.ToInt32(value);
      //if (e.Value is null || e.DesiredType != typeof(decimal)) return;
      //e.Value = Convert.ToInt32(e.Value);
    }

  }

}
