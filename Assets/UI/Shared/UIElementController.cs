using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIElementController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private SoundType activate = SoundType.UI_OnOpen;
    [SerializeField] private SoundType deactivate = SoundType.UI_OnClose;

    [Header("Animation")]
    [SerializeField] private bool isAnimated;
    [SerializeField] private string triggerName;

    private ISoundService soundService;

    private async void Start()
    {
        // Temp solution 
        await UniTask.WaitUntil(() => GameInitiator.Instance != null && GameInitiator.Instance.isFinished);
        soundService = ServiceLocator.Get<ISoundService>();
    }

    public void Activate()
    {
        var obj = target ? target : gameObject;
        obj.SetActive(true);

        if (isAnimated && !string.IsNullOrEmpty(triggerName))
        {
            obj.GetComponent<Animator>()?.SetTrigger(triggerName);
        }

        soundService?.Play(SoundCategory.UI, activate);
    }

    public void Deactivate()
    {
        var obj = target ? target : gameObject;
        obj.SetActive(false);

        soundService?.Play(SoundCategory.UI, deactivate);
    }
}
