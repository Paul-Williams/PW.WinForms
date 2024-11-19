namespace PW.WinForms;

/// <summary>
/// Static methods for displaying dialogs.
/// </summary>
public static class Dialogs
{
  /// <summary>
  /// Displays dialog for user to select a folder.
  /// </summary>
  /// <param name="description">Descriptive text displayed above the tree view control in the dialog box.</param>
  /// <param name="showNewFolderButton">Whether the New Folder button appears in the folder browser dialog box.</param>
  /// <param name="initialPath">Path selected <see cref="FolderBrowserDialog"/> when initially displayed.</param>
  /// <param name="owner">Any object that implements System.Windows.Forms.IWin32Window that represents the top-level window that will own the modal dialog box.</param>
  /// <returns>If the user selects a folder: (true,directory-path). Otherwise: (false,empty-string)</returns>

  public static (bool Ok, string SelectedPath) BrowseForFolder(string description = "Select folder", bool showNewFolderButton = false, string? initialPath = null, IWin32Window? owner = null)
  {
    using var dlg = CreateDialog(description, showNewFolderButton, initialPath);

    return dlg.ShowDialog(owner) == DialogResult.OK ? (true, dlg.SelectedPath ?? string.Empty) : (false, string.Empty);
  }

  private static FolderBrowserDialog CreateDialog(string description, bool showNewFolderButton, string? initialPath)
  {
    return new FolderBrowserDialog()
    {
      Description = description,
      ShowNewFolderButton = showNewFolderButton,
      SelectedPath = initialPath!
    };
  }
  /// <summary>
  /// Displays dialog for user to select a folder.
  /// </summary>
  /// <param name="selectedFolder">Out parameter. Returns the folder selected by the user, or <see cref="string.Empty"/> if the dialog was canceled.</param>
  /// <param name="description">Descriptive text displayed above the tree view control in the dialog box.</param>
  /// <param name="showNewFolderButton">Whether the New Folder button appears in the folder browser dialog box.</param>
  /// <param name="initialPath">Path selected <see cref="FolderBrowserDialog"/> when initially displayed.</param>
  /// <param name="owner">Any object that implements System.Windows.Forms.IWin32Window that represents the top-level window that will own the modal dialog box.</param>
  /// <returns>True if the user clicks Ok, otherwise false. Use <paramref name="selectedFolder"/> to determine the selected folder.</returns>
  public static bool BrowseForFolder(out string selectedFolder, string description = "Select folder", bool showNewFolderButton = false, string? initialPath = null, IWin32Window? owner = null)
  {
    using var dlg = CreateDialog(description, showNewFolderButton, initialPath);

    if (dlg.ShowDialog(owner) == DialogResult.OK)
    {
      selectedFolder = dlg.SelectedPath ?? string.Empty;
      return true;
    }
    else
    {
      selectedFolder = string.Empty;
      return false;
    }
  }


}
