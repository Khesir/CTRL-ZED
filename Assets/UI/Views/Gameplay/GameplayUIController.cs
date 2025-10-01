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
    [Header("Gameplay Services")]
    private PlayerService playerService;
    public AttackTimer timer;
    public UISkillSlots skillSlots;

    [Header("Rewards & Progressions")]
    public LootHolder lootHolder;

    [Header("Controls")]
    public GameObject starWaveButton;
    public void Initialize(PlayerService playerService)
    {
        this.playerService = playerService;
    }
    public void SetupCharacterUI(List<CharacterBattleState> characters)
    {
        characterListUI.Initialize(characters);
        characterIcons.Initialize(characters);
    }
    public void StartStateSetup()
    {
        baseOSHP.Setup(playerService);
        waveUI.Setup();
        timer.Setup(playerService);
        lootHolder.Setup();
        activeStatusEffect.Setup();
        skillSlots.Initialize();
    }
    public async UniTask StartStateUIAnimation()
    {
        var hotbarTask = characterListUI.AnimateHotbarsInAndOut();
        var startButtonTask = startButton.AnimateIn();

        // Wait until both are finished
        await UniTask.WhenAll(hotbarTask, startButtonTask);
    }
    public async UniTask PlayingStateUIAnimation()
    {
        var startButtonTask = startButton.AnimateOut();

        // To handle animation Concurrently
        await UniTask.WhenAll(startButtonTask);
    }
    public void PushMessage(string message)
    {
        announcementUI.PushMessage(message);
    }
    public void Complete(string type, bool complete, string team = "")
    {
        completeScreenUI.Complete(type, complete, team);
    }
    public void LootHolderAddAmount(LootDropData data)
    {
        lootHolder.AddAmount(data);
    }
    public async void HandleGameOver()
    {
        // Places a UI that allow the user to pick deployed team or choose game over.
        // Maybe use certain amount of drive to revive
        await GameplayManager.Instance.SetState(GameplayManager.GameplayState.End);
    }
}
