using DesktopTools.ViewModels.Spells;

namespace DesktopTools.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public SpellsListViewModel SpellsListTabs { get; }

    public MainWindowViewModel()
    {
        SpellsListTabs = new SpellsListViewModel();
    }
}