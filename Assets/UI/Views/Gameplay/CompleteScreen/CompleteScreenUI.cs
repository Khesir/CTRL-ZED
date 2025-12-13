using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreenUI : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text message;
    public TMP_Text reward1;
    public TMP_Text initial1;

    public TMP_Text reward2;
    public TMP_Text initial2;

    public TMP_Text reward3;
    public TMP_Text initial3;

    public TMP_Text reward4;
    public TMP_Text initial4;

    public TMP_Text reward5;
    public TMP_Text initial5;

    public TMP_Text reward6;
    public TMP_Text initial6;

    public Button backToMenuBtn;
    public bool isTriggered = false;
    private PanelAnimator animator;
    public GameObject parent;
    // Include resources plundered during battle
    public async UniTask CompleteAsync(string type, bool complete, string team = "")
    {
        parent.gameObject.SetActive(true);
        // Pause the world
        animator = GetComponent<PanelAnimator>();
        backToMenuBtn.onClick.RemoveAllListeners();
        backToMenuBtn.onClick.AddListener(() => BackToMainMenu());
        int rewardDrives = UnityEngine.Random.Range(1, 11);

        switch (type)
        {
            case "os":
                title.text = "";
                message.text = $"OS HP runned out!";
                break;
            case "character":
                if (complete)
                {
                    title.text = "Wave Cleared!";
                    message.text = $"{team} has succesfully! Stopped the wave";
                    // GameManager.Instance.LevelManager.currentLevelIndex++;

                    // Mark level as complete
                    var activeLevel = LevelManager.Instance.GetActiveLevel();
                    if (activeLevel != null)
                    {
                        LevelManager.Instance.MarkLevelComplete(activeLevel.levelID);
                        // Auto-save the completion
                        ServiceLocator.Get<IPlayerDataManager>().AutoSaveTrigger();
                    }
                }
                else
                {
                    title.text = "Game Over";
                    message.text = $"{team} has been wiped! Go back to base to repair or use other team";
                }
                break;
        }
        PlayerService manager = ServiceLocator.Get<IPlayerManager>().playerService;
        var lootHolder = ServiceLocator.Get<GameplayUIController>().lootHolder;

        // energy
        reward1.text = $"+{lootHolder.GetLoot(ItemType.Energy)}";
        initial1.text = manager.GetEnergy().ToString();
        manager.AddEnergy(lootHolder.GetLoot(ItemType.Energy));
        // food
        reward2.text = $"+{lootHolder.GetLoot(ItemType.Food)}";
        initial2.text = manager.GetFood().ToString();
        manager.AddFood(lootHolder.GetLoot(ItemType.Food));
        // tech
        reward3.text = $"+{lootHolder.GetLoot(ItemType.Technology)}";
        initial3.text = manager.GetTechnology().ToString();
        manager.AddTechnology(lootHolder.GetLoot(ItemType.Technology));
        // intel
        reward4.text = $"+{lootHolder.GetLoot(ItemType.Intelligence)}";
        initial4.text = manager.GetIntelligence().ToString();
        manager.AddTechnology(lootHolder.GetLoot(ItemType.Intelligence));

        // funds
        reward5.text = $"{lootHolder.GetLoot(ItemType.Coins)}+";
        initial5.text = manager.GetCoins().ToString();
        manager.AddCoins(lootHolder.GetLoot(ItemType.Coins));
        // drives
        reward6.text = $"{rewardDrives}+  ";
        initial6.text = manager.GetDrives().ToString();
        ServiceLocator.Get<IPlayerManager>().playerService.AddDrives(rewardDrives);

        gameObject.SetActive(true);
        await animator.Show();
        isTriggered = true;
    }
    public async void BackToMainMenu()
    {
        if (!isTriggered) return;
        Debug.Log("Going Back to mainmenu");
        gameObject.SetActive(false);
        await animator.Hide();
        GameInitiator.Instance.SwitchStates(GameState.MainMenu);
    }
}
