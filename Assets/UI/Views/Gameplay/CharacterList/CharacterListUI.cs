using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
public class CharacterListUI : MonoBehaviour
{
    [SerializeField] private bool animateOnce = true;
    [SerializeField] private bool isAnimated = false;
    [SerializeField] private GameObject CharacterIcons;
    public GameObject hotbar1;
    public GameObject hotbar2;
    public GameObject hotbar3;
    public GameObject hotbar4;

    private List<GameObject> hotbars;

    private void Awake()
    {
        hotbars = new List<GameObject>
        {
            hotbar1,
            hotbar2,
            hotbar3,
            hotbar4
        };

    }
    public void OnClose()
    {
        CharacterIcons.SetActive(true);
    }
    public void Initialize(List<CharacterBattleState> characters)
    {

        for (int i = 0; i < hotbars.Count; i++)
        {
            var hotbar = hotbars[i];

            if (i < characters.Count && characters[i] != null)
            {
                hotbar.SetActive(true);
                hotbar.GetComponent<CharacterDetails>().Initialize(characters[i]);
            }
            else
            {
                hotbar.SetActive(false);
            }
        }
        GameplayManager.Instance.followerManager.OnSwitch += UpdateHotbar1;
    }
    public async UniTask AnimateHotbarsInAndOut()
    {
        if (isAnimated) return;
        isAnimated = true;
        float speed = 1.2f;
        float delaySpeed = 1;
        Vector2 startOffset = new Vector2(-200, 0); // start off-screen left
        float stagger = 0.3f * speed;

        // Animate in
        for (int i = 0; i < hotbars.Count; i++)
        {
            var hotbar = hotbars[i];
            var rect = hotbar.GetComponent<RectTransform>();
            var canvasGroup = hotbar.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = hotbar.AddComponent<CanvasGroup>();

            rect.anchoredPosition += startOffset;
            canvasGroup.alpha = 0;
            hotbar.SetActive(true);

            rect.DOAnchorPosX(rect.anchoredPosition.x - startOffset.x, 0.4f * speed)
                .SetEase(Ease.OutBack)
                .SetDelay(i * stagger * delaySpeed);
            canvasGroup.DOFade(1, 0.4f * speed)
                .SetDelay(i * stagger * delaySpeed);
        }

        // Wait for entrance + display duration
        await UniTask.Delay((int)((hotbars.Count * stagger + 0.4f + 2f) * 1000));

        // Animate out
        List<UniTask> outTweens = new();
        for (int i = 0; i < hotbars.Count; i++)
        {
            if (i == 0) continue;
            var hotbar = hotbars[i];
            var rect = hotbar.GetComponent<RectTransform>();
            var canvasGroup = hotbar.GetComponent<CanvasGroup>();

            var moveTween = rect.DOAnchorPosX(rect.anchoredPosition.x - 200, 0.4f * speed)
                .SetEase(Ease.InOutBack)
                .SetDelay(i * stagger * delaySpeed)
                .AsyncWaitForCompletion()
                .AsUniTask();

            var fadeTween = canvasGroup.DOFade(0, 0.4f * speed)
                .SetDelay(i * stagger * delaySpeed)
                .OnComplete(() => hotbar.SetActive(false))
                .AsyncWaitForCompletion()
                .AsUniTask();

            outTweens.Add(UniTask.WhenAll(moveTween, fadeTween));
        }

        await UniTask.WhenAll(outTweens);

        // Then show CharacterIcons
        CharacterIcons.SetActive(true);
        await CharacterIcons.GetComponent<PanelAnimator>().Show();

        if (!animateOnce)
            isAnimated = false;
    }
    public void UpdateHotbar1()
    {
        var activePlayer = GameplayManager.Instance.followerManager.GetCurrentTargetBattleState();
        hotbar1.GetComponent<CharacterDetails>().Initialize(activePlayer);
    }
}
