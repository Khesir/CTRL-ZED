using Cysharp.Threading.Tasks;

public interface IPlayerManager
{
    PlayerService playerService { get; }

    UniTask Initialize(PlayerData data);
    void StartPlayerService(PlayerData data);
}
