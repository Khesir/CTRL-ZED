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
    public void Initialize()
    {
        playerService = GameManager.Instance.PlayerManager.playerService;
        CharacterListInitialize();
        CharacterListIconInitialize();
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
    private void CharacterListIconInitialize()
    {
        var team = GameManager.Instance.TeamManager.GetActiveTeam();
        var characters = team[0].GetMembers();
        var compactCharacters = new List<CharacterBattleState>();
        foreach (var c in characters)
        {
            if (c != null) compactCharacters.Add(new CharacterBattleState(c));
        }
        while (compactCharacters.Count < 4)
        {
            compactCharacters.Add(null);
        }
        characterIcons.Initialize(compactCharacters);
    }
    private void CharacterListInitialize()
    {
        var team = GameManager.Instance.TeamManager.GetActiveTeam();
        var characters = team[0].GetMembers();
        var compactCharacters = new List<CharacterBattleState>();
        foreach (var c in characters)
        {
            if (c != null) compactCharacters.Add(new CharacterBattleState(c));
        }
        while (compactCharacters.Count < 4)
        {
            compactCharacters.Add(null);
        }
        characterListUI.Initialize(compactCharacters);
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
