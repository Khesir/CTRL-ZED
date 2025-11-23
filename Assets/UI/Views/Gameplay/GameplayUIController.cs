using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    [Header("Core UI")]
    public CharacterListUI characterListUI;
    public CharacterListIconUI characterIcons;
    public OSHPUI baseOSHP;
    public WaveUIController waveUI;
    public AnnouncementUI announcementUI;
    public CompleteScreenUI completeScreenUI;
    public GameplayActiveStatusEffect activeStatusEffect;
    public StartButton startButton;
    public RevivePanel revivePanel;
    public DialogSystem dialogUI;
    [Header("Gameplay Services")]
    private PlayerService playerService;
    public AttackTimer timer;
    public UISkillSlots skillSlots;


    [Header("Rewards & Progressions")]
    public LootHolder lootHolder;

    [Header("Controls")]
    public GameObject starWaveButton;

    private void OnEnable()
    {
        SceneEventBus.Subscribe<WaveMessageEvent>(OnWaveMessage);
        SceneEventBus.Subscribe<WaveStartedEvent>(OnWaveStarted);
        SceneEventBus.Subscribe<WaveCompletedEvent>(OnWaveCompleted);
        SceneEventBus.Subscribe<LootCollectedEvent>(OnLootCollected);
    }

    private void OnDisable()
    {
        SceneEventBus.Unsubscribe<WaveMessageEvent>(OnWaveMessage);
        SceneEventBus.Unsubscribe<WaveStartedEvent>(OnWaveStarted);
        SceneEventBus.Unsubscribe<WaveCompletedEvent>(OnWaveCompleted);
        SceneEventBus.Unsubscribe<LootCollectedEvent>(OnLootCollected);
    }

    private void OnWaveMessage(WaveMessageEvent evt)
    {
        PushMessageAsync(evt.Message).Forget();
    }

    private void OnWaveStarted(WaveStartedEvent evt)
    {
        starWaveButton.SetActive(false);
    }

    private void OnWaveCompleted(WaveCompletedEvent evt)
    {
        // Add wave rewards to loot holder
        if (evt.Rewards != null)
        {
            foreach (var loot in evt.Rewards)
            {
                lootHolder.AddAmount(loot);
            }
        }
    }

    private void OnLootCollected(LootCollectedEvent evt)
    {
        lootHolder.AddAmount(evt.Data);
    }

    public void Initialize(PlayerService playerService)
    {
        this.playerService = playerService;
    }
    public void SetupCharacterUI(List<CharacterBattleState> characters)
    {
        characterListUI.Initialize(characters);
        characterIcons.Initialize(characters);
    }
    #region  Gameplay State Animation

    private bool _isInitializeSetup = false;
    public void StartStateSetup()
    {
        // Only happen on start state once, since we look start to next
        if (!_isInitializeSetup)
        {
            baseOSHP.Setup(playerService);
            waveUI.Setup();
            timer.Setup(playerService);
            lootHolder.Setup();
            activeStatusEffect.Setup();
            skillSlots.Initialize();
            dialogUI.Initialize();
            _isInitializeSetup = true;
        }

        startButton.gameObject.SetActive(true);
    }
    private bool _isInitializeSetupAnimation = false;
    public async UniTask StartStateUIAnimation()
    {
        if (!_isInitializeSetupAnimation)
        {
            characterListUI.AnimateHotbarsInAndOut().Forget();
            _isInitializeSetupAnimation = true;
        }

        startButton.AnimateIn().Forget();
    }
    public async UniTask PlayingStateUIAnimation()
    {
        startButton.AnimateOut().Forget();
    }
    #endregion
    public async UniTask PushMessageAsync(string message)
    {
        await announcementUI.PushMessage(message);
    }
    public async UniTask CompleteAsync(string type, bool complete, string team = "")
    {
        await completeScreenUI.CompleteAsync(type, complete, team);
    }
    public void LootHolderAddAmount(LootDropData data)
    {
        lootHolder.AddAmount(data);
    }
    public async void HandleEndGamePanel(GameplayEndGameState endGameState)
    {
        if (endGameState == GameplayEndGameState.DeathOnTimer)
        {
            await CompleteAsync("os", false);
        }
        else if (endGameState == GameplayEndGameState.LevelComplete)
        {
            var team = ServiceLocator.Get<ITeamManager>().GetActiveTeam();
            await CompleteAsync(
                "character",
                true,
                team[0].GetTeamName()
            );
        }
    }
}
