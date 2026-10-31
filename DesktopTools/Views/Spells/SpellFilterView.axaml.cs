using Avalonia.ReactiveUI;
using DesktopTools.ViewModels.Spells;

namespace DesktopTools.Views.Spells;

public partial class SpellFilterView : ReactiveUserControl<SpellFilterViewModel>
{
    public SpellFilterView()
    {
        InitializeComponent();
    }
}