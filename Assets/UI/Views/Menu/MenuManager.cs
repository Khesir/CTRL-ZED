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

        FindComponents();
        SetupComponents();
        soundService.Play(SoundCategory.BGM, SoundType.BGM_MainMenu, 0.5f);

        isInitialized = true;
        Debug.Log("[MenuManager] Initialized");
    }

    private void FindComponents()
    {
        // Find scene components if not assigned (include inactive objects)
        if (osExpTop == null) osExpTop = FindAnyObjectByType<OsExpTop>(FindObjectsInactive.Include);
        if (fundsMenuComponent == null) fundsMenuComponent = FindAnyObjectByType<FundsMenuComponent>(FindObjectsInactive.Include);
        if (resourceUI == null) resourceUI = FindAnyObjectByType<ResourceUI>(FindObjectsInactive.Include);
        if (repairComponent == null) repairComponent = FindAnyObjectByType<RepairComponent>(FindObjectsInactive.Include);
        if (drivesMenuComponent == null) drivesMenuComponent = FindAnyObjectByType<DrivesMenuComponent>(FindObjectsInactive.Include);
        if (deployTeamController == null) deployTeamController = FindAnyObjectByType<DeployTeamController>(FindObjectsInactive.Include);
        if (instructionsPanel == null) instructionsPanel = FindAnyObjectByType<InstructionsPanel>(FindObjectsInactive.Include);

        // Debug: Log which components were found
        Debug.Log($"[MenuManager] FindComponents - osExpTop: {(osExpTop != null ? "Found" : "NULL")}");
        Debug.Log($"[MenuManager] FindComponents - fundsMenuComponent: {(fundsMenuComponent != null ? "Found" : "NULL")}");
        Debug.Log($"[MenuManager] FindComponents - resourceUI: {(resourceUI != null ? "Found" : "NULL")}");
        Debug.Log($"[MenuManager] FindComponents - repairComponent: {(repairComponent != null ? "Found" : "NULL")}");
        Debug.Log($"[MenuManager] FindComponents - drivesMenuComponent: {(drivesMenuComponent != null ? "Found" : "NULL")}");
        Debug.Log($"[MenuManager] FindComponents - deployTeamController: {(deployTeamController != null ? "Found" : "NULL")}");
    }

    private void SetupComponents()
    {
        osExpTop?.Setup();
        fundsMenuComponent?.Setup();
        resourceUI?.Setup();
        repairComponent?.Setup();
        drivesMenuComponent?.Setup();
        deployTeamController?.Setup();

        if (ServiceLocator.Get<GameInitiator>().isInTutorial && instructionsPanel != null)
        {
            instructionsPanel.gameObject.SetActive(true);
            ServiceLocator.Get<GameInitiator>().CompleteTutorial();
        }
    }
}
