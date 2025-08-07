using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderDamageFlash : MonoBehaviour
{
    [SerializeField] private float _flashTime = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Material material;
    private Coroutine flashCoroutine;
    private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

    public void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    public void Flash(float duration)
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(DamageFlasher(duration));
    }

    private IEnumerator DamageFlasher(float duration)
    {
        float elapsed = 0f;
        bool toggle = false;

        while (elapsed < duration)
        {
            toggle = !toggle;
            material.SetFloat(FlashAmount, toggle ? 1 : 0);

            yield return new WaitForSeconds(_flashTime);
            elapsed += _flashTime;
        }

        material.SetFloat(FlashAmount, 0); // Ensure flash is cleared
        flashCoroutine = null;
    }
}
