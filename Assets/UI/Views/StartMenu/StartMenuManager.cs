using Cysharp.Threading.Tasks;
using UnityEngine;

public class StartMenuManager : MonoBehaviour
{
    public static StartMenuManager Instance { get; private set; }

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
        soundService.Play(SoundCategory.BGM, SoundType.BGM_Start, 0.5f);

        isInitialized = true;
        Debug.Log("[StartMenuManager] Initialized");
    }
}
