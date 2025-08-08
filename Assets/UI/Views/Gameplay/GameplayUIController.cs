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
    public LootHolder lootHolder;
    [Header("GameObject Controls")]
    public GameObject starWaveButton;
    public void Initialize(List<CharacterBattleState> characters)
    {
        playerService = GameManager.Instance.PlayerManager.playerService;
        characterListUI.Initialize(characters);
        characterIcons.Initialize(characters);
        baseOSHP.Setup(playerService);
        waveUI.Setup();
        timer.Setup(playerService);
        lootHolder.Setup();
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
}
