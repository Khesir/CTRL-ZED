using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface ILevelManager
{
    LevelData activeLevel { get; set; }
    List<LevelData> allLevels { get; }

    UniTask Initialize();
    LevelData GetLevelByID(string id);
    List<LevelData> GetAllLevels();
    LevelData GetActiveLevel();
    UniTask LoadScene(GameState gameState);
}
