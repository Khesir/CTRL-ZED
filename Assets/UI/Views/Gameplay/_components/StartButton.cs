using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Flags")]
    private bool isVisible = false;
    [HideInInspector] public Button button;
    private RectTransform rect;
    private Vector2 originalPos;
    private Vector3 originalScale;
    private Tween wobbleTween;
    private void Awake()
    {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
        originalScale = rect.localScale;

        gameObject.SetActive(false);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(async () => await Activate());
    }
    public async UniTask Activate()
    {
        await GameplayManager.Instance.SetState(GameplayState.Playing);
    }
    public async UniTask AnimateIn(float slideDistance = 200f, float duration = 0.6f)
    {
        if (isVisible) return;
        isVisible = true;

        // Capture true layout position every time
        originalPos = rect.anchoredPosition;

        gameObject.SetActive(true);
        rect.anchoredPosition = originalPos - new Vector2(0, slideDistance);
        rect.localScale = originalScale;

        // Animate slide-in
        await rect.DOAnchorPosY(originalPos.y, duration)
                  .SetEase(Ease.OutBack)
                  .SetUpdate(true)
                  .AsyncWaitForCompletion();

        StartWobble();
    }
    private void StartWobble(float wobbleAmount = 10f, float wobbleDuration = 0.5f)
    {
        wobbleTween?.Kill();
        wobbleTween = rect.DOAnchorPosY(originalPos.y + wobbleAmount, wobbleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true)
            .SetEase(Ease.InOutSine);
    }
    private void StopWobble()
    {
        wobbleTween?.Kill();
        rect.anchoredPosition = originalPos;
    }

    public async UniTask AnimateOut(float slideDistance = 200f, float duration = 0.6f)
    {
        if (!isVisible) return;
        isVisible = false;

        StopWobble();

        // Animate out
        var posTween = rect.DOAnchorPosY(originalPos.y - slideDistance, duration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .AsyncWaitForCompletion()
            .AsUniTask();

        var scaleTween = rect.DOScale(Vector3.zero, duration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .AsyncWaitForCompletion()
            .AsUniTask();

        await UniTask.WhenAll(posTween, scaleTween);

        // Restore state for next reuse
        rect.localScale = originalScale;
        rect.anchoredPosition = originalPos;

        gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOScale(originalScale * 1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack);
    }
}
