namespace Utils.MiniTools.Reincarnation;

public interface IReincarnationService
{
    /// <summary>
    /// Совершает случайный выбор новой расы по правилам реинкарнации.
    /// 1d20: 1 - 14 обычные, 15-20 необычные/редкие
    /// Потом выбор из списка
    /// </summary>
    string RollNewRace();
}