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

    // Level unlock and completion
    bool IsLevelComplete(string levelID);
    bool IsLevelUnlocked(LevelData level);
    void MarkLevelComplete(string levelID);
    void LoadClearedLevels(List<string> clearedLevelIDs);
}
