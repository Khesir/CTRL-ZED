using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LevelInformationModal : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private Image levelInfoImage;

    [Header("Controls")]
    [SerializeField] private Button startButton;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [HideInInspector] public LevelData data;

    public void Trigger()
    {
        gameObject.SetActive(true);
        if (animator != null)
            animator.SetTrigger("Close");
    }

    public void CloseTrigger()
    {
        if (animator != null)
            animator.SetTrigger("Close");

        StartCoroutine(WaitUntilReadyThenDisable());
    }

    private IEnumerator WaitUntilReadyThenDisable()
    {
        if (animator != null)
        {
            while (animator.IsInTransition(0))
                yield return null;

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (!stateInfo.IsName("Ready"))
            {
                yield return null;
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            }
        }

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Set level information image
        if (levelInfoImage != null && data.levelInformation != null)
            levelInfoImage.sprite = data.levelInformation;

        // Setup start button
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(async () =>
            {
                try
                {
                    await StartGame();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"StartGame error: {ex}");
                }
            });
        }
    }

    private void OnDisable()
    {
        if (startButton != null)
            startButton.onClick.RemoveAllListeners();
    }
    async UniTask StartGame()
    {
        var activeTeam = ServiceLocator.Get<ITeamManager>().GetActiveTeam();

        PlayerService playerService = ServiceLocator.Get<IPlayerManager>().playerService;
        // IBioChipService bioChipService = playerService;
        IResourceService resourceService = playerService;
        if (activeTeam.Count < 1)
        {
            Debug.LogWarning("No set active team");
            return;
        }
        var members = activeTeam[0].GetMembers();
        var totalDeploymentCost = new Dictionary<string, float>();

        foreach (CharacterData character in members)
        {
            float multiplier = Mathf.Pow(1.2f, character.currentLevel - 1);

            var cost = new Dictionary<string, float>{
                    {"Food", character.baseData.food * multiplier },
                    {"Technology", character.baseData.technology * multiplier},
                    {"Energy", character.baseData.energy * multiplier},
                    {"Intelligence", character.baseData.intelligence* multiplier}
                };

            foreach (var kvp in cost)
            {
                if (totalDeploymentCost.ContainsKey(kvp.Key))
                {
                    totalDeploymentCost[kvp.Key] += kvp.Value;
                }
                else
                {
                    totalDeploymentCost[kvp.Key] = kvp.Value;
                }
            }
        }
        resourceService.SpendFood((int)totalDeploymentCost["Food"]);
        resourceService.SpendTechnology((int)totalDeploymentCost["Technology"]);
        resourceService.SpendEnergy((int)totalDeploymentCost["Energy"]);
        resourceService.SpendIntelligence((int)totalDeploymentCost["Intelligence"]);

        var levelManager = ServiceLocator.Get<ILevelManager>();
        levelManager.activeLevel = data;
        ServiceLocator.Get<ISoundService>().Play(SoundCategory.UI, SoundType.UI_Activate);
        await levelManager.LoadScene(GameState.Gameplay);
    }
}
