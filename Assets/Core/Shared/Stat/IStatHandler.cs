
public interface IStatHandler
{
    float GetStat(string statId);
    void AddStatProvider(IStatProvider provider);
    void RemoveStatProvider(IStatProvider provider);
}
