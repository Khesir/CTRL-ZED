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
    public async UniTask Initialize()
    {
        playerService = GameManager.Instance.PlayerManager.GetPlayerService();
        CharacterListInitialize();
        CharacterListIconInitialize();
        InitializeOSHP();
        InitializeWaveUI();
        InitializeAttackTimer();
        await UniTask.CompletedTask;
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
        var characters = team.GetMembers();
        var compactCharacters = new List<CharacterService>();
        foreach (var c in characters)
        {
            if (c != null) compactCharacters.Add(c);
        }
        while (compactCharacters.Count < 4)
        {
            compactCharacters.Add(null);
        }

        var hotbars = new List<GameObject>
        {
            characterIcons.hotbar1,
            characterIcons.hotbar2,
            characterIcons.hotbar3,
            characterIcons.hotbar4
        };

        for (int i = 0; i < hotbars.Count; i++)
        {
            var hotbar = hotbars[i];

            if (compactCharacters[i] != null)
            {
                hotbar.GetComponent<CharacterDetailsIcon>().Initialize(compactCharacters[i], i);
            }
        }
    }
    private void CharacterListInitialize()
    {
        var team = GameManager.Instance.TeamManager.GetActiveTeam();
        var characters = team.GetMembers();
        var compactCharacters = new List<CharacterService>();
        foreach (var c in characters)
        {
            if (c != null) compactCharacters.Add(c);
        }
        while (compactCharacters.Count < 4)
        {
            compactCharacters.Add(null);
        }

        var hotbars = new List<GameObject>
        {
            characterListUI.hotbar1,
            characterListUI.hotbar2,
            characterListUI.hotbar3,
            characterListUI.hotbar4
        };

        for (int i = 0; i < hotbars.Count; i++)
        {
            var hotbar = hotbars[i];

            if (compactCharacters[i] != null)
            {
                hotbar.SetActive(true);
                hotbar.GetComponent<CharacterDetails>().Initialize(compactCharacters[i]);
            }
            else
            {
                hotbar.SetActive(false); // Hide or optionally show "empty" UI
            }
        }
    }
}
