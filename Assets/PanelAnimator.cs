using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class PanelAnimator : MonoBehaviour
{
    [Header("Animation settings")]
    public Vector2 slideDirection = new Vector2(0, -200); // From Button by default
    public float duration = 0.5f;
    public Ease ease = Ease.OutCubic;

    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;

    private Tween currentSequence;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPos = rect.anchoredPosition;
    }
    public async UniTask Show()
    {
        currentSequence?.Kill();

        rect.anchoredPosition = originalPos + slideDirection;
        canvasGroup.alpha = 0;

        currentSequence = DOTween.Sequence()
            .Join(rect.DOAnchorPos(originalPos, duration).SetEase(ease).SetUpdate(true))
            .Join(canvasGroup.DOFade(1, duration).SetUpdate(true));


        await currentSequence.AsyncWaitForCompletion().AsUniTask();
    }
    public async UniTask Hide(GameObject gameObject = null)
    {
        currentSequence?.Kill();
        currentSequence = DOTween.Sequence()
            .Join(rect.DOAnchorPos(originalPos + slideDirection, duration).SetEase(Ease.InCubic).SetUpdate(true))
            .Join(canvasGroup.DOFade(0, duration).SetUpdate(true))
            .OnComplete(() => { if (gameObject != null) gameObject.SetActive(false); });

        await currentSequence.AsyncWaitForCompletion().AsUniTask();
    }
}
