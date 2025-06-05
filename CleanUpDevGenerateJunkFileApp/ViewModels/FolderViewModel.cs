using CommunityToolkit.Mvvm.ComponentModel;

namespace CleanUpDevGenerateJunkFileApp.ViewModels;

public partial class FolderViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsChecked { get; set; } = true;
    [ObservableProperty]
    public partial string FolderName { get; set; }
}
