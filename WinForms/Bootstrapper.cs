namespace PW.WinForms;

/// <summary>
/// Performs application start-up and displays 'shell' main form.
/// Optionally override OnStartup to perform addition actions before displaying the shell.
/// Unhandled exception handlers are automatically hooked up for UI and non-UI threads.
/// For Unity / Prism enabled version use PW.WinForms.BootStrappers.PrismBootstrapper
/// </summary>
/// <typeparam name="Shell"></typeparam>
public class Bootstrapper<Shell> where Shell : Form, new()
{

  /// <summary>
  /// Arguments for <see cref="OnStartup(StartupEventArgs)"/>
  /// </summary>
  protected class StartupEventArgs : EventArgs
  {
    /// <summary>
    /// Abort application start-up.
    /// </summary>
    public bool Abort { get; set; } = false;
  }


  /// <summary>
  /// May be overridden by inherited classes to perform any start-up routine before 
  /// the main form is created. To abort creation of the shell set <see cref="StartupEventArgs.Abort"/> to true.
  /// There is no need to call base.OnStartup explicitly from overriding methods.
  /// </summary>    
  protected virtual void OnStartup(StartupEventArgs eventArgs) { }

  /// <summary>
  /// Configure handlers for otherwise unhandled exceptions
  /// </summary>
  private static void ConfigureUnhandledExceptionHandlers()
  {

    // Cause app to quit on unhandled exceptions - message will be displayed first
    WinFormsUnhandledExceptionHandler.ExitOnUnhandledException = true;

    // UI thread exceptions
    Application.ThreadException += WinFormsUnhandledExceptionHandler.ApplicationThreadException;

    // Non-UI thread exceptions
    AppDomain.CurrentDomain.UnhandledException += WinFormsUnhandledExceptionHandler.CurrentDomainUnhandledException;

    // ? Read docs ;)
    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
  }

  /// <summary>
  /// Starts the application and displays the <typeparamref name="Shell"/>"/>.
  /// </summary>
  public void Run()
  {
    ConfigureUnhandledExceptionHandlers();
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    // If OnStartup returns Fail then the shell will not be created.
    // Returns the result in any case.
    var eventArgs = new StartupEventArgs();
    OnStartup(eventArgs);
    if (!eventArgs.Abort) CreateShell();
  }

  private static void CreateShell() => Application.Run(new Shell());

  // See: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.application.setunhandledexceptionmode?view=netframework-4.7.1#System_Windows_Forms_Application_SetUnhandledExceptionMode_System_Windows_Forms_UnhandledExceptionMode_System_Boolean_
  // Last-resort top-level handlers for otherwise unhandled exceptions.
  // This class is here just to keep it self-contained with the bootstrapper
  private static class WinFormsUnhandledExceptionHandler
  {
    public static bool ExitOnUnhandledException { get; set; } = true;

    // All exceptions thrown by the main UI thread are handled by this method
    public static void ApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
      => DisplayUnhandledExceptionAndExit(e.Exception);

    // All exceptions thrown by additional non-UI threads are handled by this method
    public static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
      => DisplayUnhandledExceptionAndExit((Exception)e.ExceptionObject);

    private static void DisplayUnhandledExceptionAndExit(Exception ex)
    {
      ExceptionDialog.Display(ex);
      if (ExitOnUnhandledException) Environment.Exit(-1);
    }


    //private static void DisplayError(string message) => MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

  }

}
