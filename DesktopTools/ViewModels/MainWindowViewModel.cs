using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DesktopTools.DesignServices;
using DesktopTools.ViewModels.LootTables;
using DesktopTools.ViewModels.Spells;
using DynamicData;
using ReactiveUI;
using Utils.LootTables.Interfaces;

namespace DesktopTools.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ITreasureGeneratorService _treasureService;

    public SpellsListViewModel SpellsListTabs { get; }
    public TreasureGeneratorViewModel TreasureTabs { get; }

    public ObservableCollection<string> SelectedLootFiles { get; } = [];

    public ICommand SelectLootFilesCommand { get; }
    
    public MainWindowViewModel() 
        : this(new DesignTreasureGeneratorService())
    {
    }

    public MainWindowViewModel(ITreasureGeneratorService treasureService)
    {
        SpellsListTabs = new SpellsListViewModel();

        _treasureService = treasureService;

        TreasureTabs = new TreasureGeneratorViewModel(_treasureService);

        SelectLootFilesCommand = ReactiveCommand.CreateFromTask<Window>(SelectLootFilesAsync);
    }

    private async Task SelectLootFilesAsync(Window parentWindow)
    {
        var topLevel = TopLevel.GetTopLevel(parentWindow);
        if (topLevel == null)
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выберите JSON файлы таблиц сокровищ",
            AllowMultiple = true,
            FileTypeFilter = [new FilePickerFileType("JSON файлы") { Patterns = ["*.json"] }]
        });

        if (files.Count == 0)
            return;

        var paths = files.Select(x => x.Path.LocalPath);
        SelectedLootFiles.Clear();
        SelectedLootFiles.AddRange(paths);

        _treasureService.Initialize(SelectedLootFiles);

        TreasureTabs.RefreshTables();
    }
}