using Cysharp.Threading.Tasks;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Menu Components")]
    [SerializeField] private OsExpTop osExpTop;
    [SerializeField] private FundsMenuComponent fundsMenuComponent;
    [SerializeField] private ResourceUI resourceUI;
    [SerializeField] private RepairComponent repairComponent;
    [SerializeField] private DrivesMenuComponent drivesMenuComponent;
    [SerializeField] private DeployTeamController deployTeamController;
    [SerializeField] private InstructionsPanel instructionsPanel;

    private ISoundService soundService;
    private bool isInitialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private async void Start()
    {
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);
        Initialize();
    }

    private void Initialize()
    {
        if (isInitialized) return;

        soundService = ServiceLocator.Get<ISoundService>();

        SetupComponents();
        soundService.Play(SoundCategory.BGM, SoundType.BGM_MainMenu, 0.5f);

        isInitialized = true;
        Debug.Log("[MenuManager] Initialized");
    }

    private void SetupComponents()
    {
        osExpTop.Setup();
        fundsMenuComponent.Setup();
        resourceUI.Setup();
        repairComponent.Setup();
        drivesMenuComponent.Setup();
        deployTeamController.Setup();

        if (ServiceLocator.Get<GameManager>().isInTutorial)
        {
            instructionsPanel.gameObject.SetActive(true);
        }
    }
}
