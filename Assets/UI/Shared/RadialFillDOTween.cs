using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RadialFillDOTween : MonoBehaviour
{
    public float fillDuration = 2f;
    public Ease fillEase = Ease.Linear;
    public bool startFilled = false;
    public bool playOnStart = false;
    public bool loop = false;

    Image _image;
    Tweener _tween;

    void Awake()
    {
        _image = GetComponent<Image>();
        if (_image.type != Image.Type.Filled)
            Debug.LogWarning("RadialFillDOTween: Image must be set to filled type (radial)");
    }

    void Start()
    {
        _image.fillAmount = startFilled ? 1f : 0f;
        if (playOnStart) PlayFill(loop ? 0f : fillDuration, loop);
    }
    /// <summary>
    /// Fills (0 -> 1) over duration. If loop is true, will keep looping.
    /// Use progress between 0...1 to set manually.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="shouldLoop"></param>
    public void PlayFill(float duration = -1f, bool shouldLoop = false)
    {
        _tween?.Kill();
        _tween = DOTween.To(() => _image.fillAmount, x => _image.fillAmount = x, 1f, duration)
            .SetEase(fillEase)
            .OnComplete(() => { if (!shouldLoop) _tween = null; }).SetId(this);

        if (shouldLoop) _tween.SetLoops(-1, LoopType.Restart);
    }
    public void FillTo(float target, float duration)
    {
        _tween?.Kill();
        _tween = DOTween.To(() => _image.fillAmount, x => _image.fillAmount = x, Mathf.Clamp01(target), duration).SetEase(fillEase).SetId(this);
    }
    public void SetProgress(float normalized) => _image.fillAmount = Mathf.Clamp01(normalized);
    public void Stop(bool reset = false)
    {
        _tween?.Kill();
        _tween = null;
        if (reset) _image.fillAmount = startFilled ? 1f : 0f;
    }
}
