using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace CleanUpDevGenerateJunkFileApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly OpenFolderDialog _openFolderDialog;
    private readonly Dispatcher _dispatcher;
    public MainWindowViewModel(OpenFolderDialog openFolderDialog, Dispatcher dispatcher)
    {
        _openFolderDialog = openFolderDialog;
        _dispatcher = dispatcher;
        Folders.CollectionChanged += (_, _) =>
        {
            ItemsCount = Folders.Count;
        };
    }
    [ObservableProperty]
    public partial string Title { get; set; } = "Prism Application";
    [ObservableProperty]
    public partial bool ObjChecked { get; set; } = true;

    [ObservableProperty]
    public partial bool BinChecked { get; set; } = true;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(FindFolderCommand))]
    public partial string FolderDir { get; set; }

    [ObservableProperty]
    public partial Visibility MaskVisibility { get; set; } = Visibility.Collapsed;

    [ObservableProperty]
    public partial ObservableCollection<FolderViewModel> Folders { get; set; } = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteFileCommand))]
    public partial int ItemsCount { get; set; }

    #region
    [RelayCommand]
    public void BrowseFolder()
    {
        if (true == _openFolderDialog.ShowDialog())
        {
            FolderDir = _openFolderDialog.FolderName;
        }
    }

    private bool CanFindFolder => !string.IsNullOrWhiteSpace(FolderDir);

    [RelayCommand(CanExecute = nameof(CanFindFolder))]
    public async Task FindFolderAsync()
    {
        await Task.Run(() =>
        {
            _dispatcher.Invoke(() => MaskVisibility = Visibility.Visible);
            //var files = Directory.GetFiles(Folder, "*.*", SearchOption.AllDirectories); // 遍历所有文件
            IEnumerable<string> binDirs, objDirs;
            _dispatcher.Invoke(() => { Folders.Clear(); });
            if (BinChecked)
            {
                binDirs = Directory.GetDirectories(FolderDir, "bin", SearchOption.AllDirectories);
                _dispatcher.Invoke(() => { Folders.AddRange(binDirs.Select(x => new FolderViewModel() { FolderName = x })); });
            }
            if (ObjChecked)
            {
                objDirs = Directory.GetDirectories(FolderDir, "obj", SearchOption.AllDirectories); //遍历所有文件夹});
                _dispatcher.Invoke(() => { Folders.AddRange(objDirs.Select(x => new FolderViewModel() { FolderName = x })); });
            }

            _dispatcher.Invoke(() => MaskVisibility = Visibility.Collapsed);
        });

    }

    private bool CanDeleteFile => Folders is not null && Folders.Count > 0;

    [RelayCommand(CanExecute = nameof(CanDeleteFile))]
    public async Task DeleteFileAsync()
    {
        await Task.Run(() =>
        {
            foreach (var dir in Folders.Where(x => x.IsChecked).ToArray())
            {
                if (Directory.Exists(dir.FolderName))
                {
                    Directory.Delete(dir.FolderName, true);
                    _dispatcher.Invoke(() => Folders.Remove(dir));
                }
            }
        });
    }
    #endregion
}
