using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using DesktopTools.Services.DesignMocks;
using DesktopTools.Services.Localization;
using DesktopTools.ViewModels.LootTables;
using DesktopTools.ViewModels.MiniTools;
using DesktopTools.ViewModels.Spells;
using DynamicData;
using ReactiveUI;
using Utils.LootTables.Interfaces;
using Utils.MiniTools.Reincarnation;
using Utils.Spells.Interfaces;

namespace DesktopTools.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ITreasureGeneratorService _treasureService;

    public SpellsListViewModel SpellsListTabs { get; }
    public TreasureGeneratorViewModel TreasureTabs { get; }
    public ReincarnationViewModel ReincarnationVm { get; }

    public ObservableCollection<string> SelectedLootFiles { get; } = [];

    public ICommand SelectLootFilesCommand { get; }
    public ICommand SwitchLanguageCommand { get; }

    public MainWindowViewModel() :
        this(new DesignTreasureGeneratorService(),
            new DesignSpellFilterContext(),
            new DesignReincarnationService(),
            new DesignSpellSelectorService())
    {
    }

    public MainWindowViewModel(ITreasureGeneratorService treasureService,
        ISpellFilterContext spellFilterContext,
        IReincarnationService reincarnationService,
        ISpellSelectorService spellSelectorService)
    {
        SpellsListTabs = new SpellsListViewModel(spellFilterContext, spellSelectorService);

        _treasureService = treasureService;

        TreasureTabs = new TreasureGeneratorViewModel(_treasureService);
        ReincarnationVm = new ReincarnationViewModel(reincarnationService);

        SelectLootFilesCommand = ReactiveCommand.CreateFromTask<Window>(SelectLootFilesAsync);
        SwitchLanguageCommand = ReactiveCommand.Create(ChangeLocalization);
    }

    private static void ChangeLocalization()
    {
        var current = LocalizationManager.Instance.CurrentCulture.Name;
        LocalizationManager.Instance.CurrentCulture = current == "ru-RU"
            ? new System.Globalization.CultureInfo("en-US")
            : new System.Globalization.CultureInfo("ru-RU");
    }

    private async Task SelectLootFilesAsync(Window parentWindow)
    {
        var topLevel = TopLevel.GetTopLevel(parentWindow);
        if (topLevel == null)
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = LocalizationManager.Instance["FileManagerWindow"],
            AllowMultiple = true,
            FileTypeFilter =
                [new FilePickerFileType(LocalizationManager.Instance["JsonFiles"]) { Patterns = ["*.json"] }]
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