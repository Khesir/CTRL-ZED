using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private TeamManager teamManager;
    [SerializeField] private AntiVirusManager antiVirusManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private StatusEffectManager statusEffectManager;

    [Header("Tutorial")]
    [SerializeField] private LevelData tutorialLevel;
    [SerializeField] private bool skipTutorial = false;

    // State
    public bool isGameActive;
    public bool isInTutorial;
    private bool isInitialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async UniTask Initialize()
    {
        if (isInitialized) return;

        isGameActive = false;
        isInitialized = true;

        Debug.Log("[GameManager] Initialized");
        await UniTask.CompletedTask;

        if (!skipTutorial)
        {
            TutorialTrigger().Forget();
        }
    }

    private async UniTaskVoid TutorialTrigger()
    {
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);
        await UniTask.WaitUntil(() => GameInitiator.Instance.GameStateManager.Currentstate == GameState.MainMenu);

        bool tutorialNotCompleted = !ServiceLocator.Get<IPlayerManager>().playerService.GetPlayerData().completedTutorial;

        if (tutorialNotCompleted)
        {
            ServiceLocator.Get<ILevelManager>().activeLevel = tutorialLevel;
            GameInitiator.Instance.SwitchStates(GameState.Gameplay);
            isInTutorial = true;
        }
    }

    public bool HandleTutorial()
    {
        if (!isInTutorial) return false;

        skipTutorial = true;
        isInTutorial = false;
        return true;
    }

    #region Internal Accessors for CoreCompositionRoot Only
    // These are only used during DI registration in CoreCompositionRoot
    // Do NOT use these elsewhere - use ServiceLocator.Get<T>() instead
    internal PlayerDataManager GetPlayerDataManager() => playerDataManager;
    internal PlayerManager GetPlayerManager() => playerManager;
    internal CharacterManager GetCharacterManager() => characterManager;
    internal TeamManager GetTeamManager() => teamManager;
    internal AntiVirusManager GetAntiVirusManager() => antiVirusManager;
    internal LevelManager GetLevelManager() => levelManager;
    internal StatusEffectManager GetStatusEffectManager() => statusEffectManager;
    #endregion
}
