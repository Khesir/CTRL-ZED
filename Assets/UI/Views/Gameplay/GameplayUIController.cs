using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    public CharacterListUI characterListUI;
    public CharacterListIconUI characterIcons;
    public OSHPUI baseOSHP;
    private PlayerService playerService;
    public AttackTimer timer;
    public WaveUIController waveUI;
    public AnnouncementUI announcementUI;
    public CompleteScreenUI completeScreenUI;
    public SquadLevelUI squadLevelUI;
    [Header("GameObject Controls")]
    public GameObject starWaveButton;
    public void Initialize(List<CharacterBattleState> characters)
    {
        playerService = GameManager.Instance.PlayerManager.playerService;
        CharacterListInitialize(characters);
        CharacterListIconInitialize(characters);
        InitializeOSHP();
        InitializeWaveUI();
        InitializeAttackTimer();
    }

    private void InitializeWaveUI()
    {
        waveUI.Setup(playerService);
    }
    private void InitializeAttackTimer()
    {
        timer.Setup(playerService);
    }
    private void InitializeOSHP()
    {
        baseOSHP.Setup(playerService);
    }
    private void CharacterListIconInitialize(List<CharacterBattleState> characters)
    {
        characterIcons.Initialize(characters);
    }
    private void CharacterListInitialize(List<CharacterBattleState> characters)
    {
        characterListUI.Initialize(characters);
    }
    public void PushMessage(string message)
    {
        announcementUI.PushMessage(message);
    }
    public void Complete(string type, bool complete, string team = "", List<LootDropData> loots = null)
    {
        completeScreenUI.Complete(type, complete, team, loots);
    }
}
