using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : MonoBehaviour, ISkill
{
    [SerializeField] private HealSkillConfig config;
    [SerializeField] private GameObject user;
    [SerializeField] private float lastUsedTime;

    public void Initialize(SkillConfig config, GameObject user)
    {
        this.config = config as HealSkillConfig;
        this.user = user;
    }


    public bool CanActivate()
    {
        return Time.time >= lastUsedTime + config.cooldown;
    }
    public void Activate()
    {
        if (!CanActivate()) return;

        lastUsedTime = Time.time;
        var state = user.GetComponent<PlayerGameplayService>()?.GetCharacterState();
        state?.Heal(config.healAmount);

        SpriteRenderer renderer = user.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Debug.Log("Flash!");
            StartCoroutine(FadeFlashColor(renderer, Color.green, 2f));
        }
        // VFX from config
        if (config.vfxPrefab != null)
        {
            GameObject vfx = Instantiate(config.vfxPrefab, user.transform.position, Quaternion.identity);
            Destroy(vfx, config.skillLifetime);
        }

        Debug.Log($"{user.name} used {config.skillName}");
    }
    private IEnumerator FlashColor(SpriteRenderer renderer, Color flashColor, float duration)
    {
        Color originalColor = renderer.color;
        renderer.color = flashColor;

        yield return new WaitForSeconds(duration);

        renderer.color = originalColor;
    }
    private IEnumerator FadeFlashColor(SpriteRenderer renderer, Color flashColor, float duration)
    {
        Color originalColor = renderer.color;
        float halfDuration = duration / 2f;

        float timer = 0f;

        // Phase 1: Fade to flashColor
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = timer / halfDuration;
            renderer.color = Color.Lerp(originalColor, flashColor, t);
            yield return null;
        }

        timer = 0f;

        // Phase 2: Fade back to originalColor
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = timer / halfDuration;
            renderer.color = Color.Lerp(flashColor, originalColor, t);
            yield return null;
        }

        renderer.color = originalColor; // Ensure it finishes at the original
    }
}
