using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    public CharacterListUI characterListUI;
    public CharacterListIconUI characterIcons;
    public TMP_Text timerText;
    public float timeLimit = 5f;
    private float timer;
    public async UniTask Initialize()
    {
        CharacterListInitialize();
        CharacterListIconInitialize();
        SetupAttackTimer();
        await UniTask.CompletedTask;
    }
    private void SetupAttackTimer()
    {
        timer = timeLimit;
    }
    public void TriggerTimer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Debug.Log("Attack Player OS");
            timer = timeLimit;
        }

        UpdateAttackTimer(timer);
    }
    private void UpdateAttackTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f / 10f);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
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
