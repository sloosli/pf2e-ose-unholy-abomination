using System;
using System.Windows.Input;
using DesktopTools.Services.Localization;
using ReactiveUI;
using Utils.MiniTools.Reincarnation;

namespace DesktopTools.ViewModels.MiniTools;

public class ReincarnationViewModel : ViewModelBase
{
    private readonly IReincarnationService _reincarnationService;

    // Свойство для хранения выпавшего результата
    public string CurrentResult
    {
        get => NotifyGetValue(() => CurrentResult) ?? string.Empty;
        set => NotifySetValue(() => CurrentResult, value);
    }

    public ICommand RollRaceCommand { get; }

    public ReincarnationViewModel(IReincarnationService reincarnationService)
    {
        _reincarnationService = reincarnationService;

        RollRaceCommand = ReactiveCommand.Create(ExecuteRollRace);
    }

    private void ExecuteRollRace()
    {
        CurrentResult = _reincarnationService.RollNewRace();
    }
}