using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundService
{
    [SerializeField] private List<CategorizedSound> soundEntries = new List<CategorizedSound>();

    private Dictionary<SoundCategory, Dictionary<SoundType, AudioClip>> categoryMap;
    private Dictionary<SoundCategory, AudioSource> categorySources;
    private Dictionary<SoundCategory, float> categoryVolumes;

    public void Initialize()
    {
        categorySources = new Dictionary<SoundCategory, AudioSource>();
        categoryVolumes = new Dictionary<SoundCategory, float>();

        foreach (SoundCategory category in System.Enum.GetValues(typeof(SoundCategory)))
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;

            if (category == SoundCategory.BGM)
                source.loop = true;

            categorySources[category] = source;
            categoryVolumes[category] = 1f;
        }

        BuildSoundMaps();
    }

    private void BuildSoundMaps()
    {
        categoryMap = new Dictionary<SoundCategory, Dictionary<SoundType, AudioClip>>();

        foreach (var entry in soundEntries)
        {
            if (!categoryMap.ContainsKey(entry.category))
                categoryMap[entry.category] = new Dictionary<SoundType, AudioClip>();

            categoryMap[entry.category][entry.soundType] = entry.clip;
        }
    }

    private bool TryGetClip(SoundCategory category, SoundType type, out AudioClip clip)
    {
        clip = null;

        if (categoryMap == null)
        {
            Debug.LogWarning("[SoundManager] Not initialized. Call Initialize() first.");
            return false;
        }

        if (categoryMap.TryGetValue(category, out var typeDict) &&
            typeDict.TryGetValue(type, out clip) && clip != null)
        {
            return true;
        }

        Debug.LogWarning($"[SoundManager] Clip not found: {category}/{type}");
        return false;
    }

    private AudioSource GetSource(SoundCategory category)
    {
        if (categorySources.TryGetValue(category, out var source))
            return source;

        Debug.LogWarning($"[SoundManager] No AudioSource for category: {category}");
        return null;
    }

    #region ISoundService Implementation

    public void Play(SoundCategory category, SoundType type, float volume = 1f)
    {
        if (!TryGetClip(category, type, out var clip)) return;

        var source = GetSource(category);
        if (source == null) return;

        // One-shot for SFX, full control for BGM/Status
        if (category == SoundCategory.BGM || category == SoundCategory.Status)
        {
            source.clip = clip;
            source.loop = false;
            source.volume = volume * categoryVolumes[category];
            source.Play();
        }
        else
        {
            source.PlayOneShot(clip, volume * categoryVolumes[category]);
        }
    }

    public void PlayLoop(SoundCategory category, SoundType type, float volume = 1f)
    {
        if (!TryGetClip(category, type, out var clip)) return;

        var source = GetSource(category);
        if (source == null) return;

        source.clip = clip;
        source.loop = true;
        source.volume = volume * categoryVolumes[category];
        source.Play();
    }

    public void Stop(SoundCategory category)
    {
        var source = GetSource(category);
        if (source == null) return;

        source.Stop();
        source.clip = null;
    }

    public void StopAll()
    {
        foreach (var category in categorySources.Keys)
        {
            Stop(category);
        }
    }

    public void SetCategoryVolume(SoundCategory category, float volume)
    {
        categoryVolumes[category] = Mathf.Clamp01(volume);

        // Update currently playing source
        if (categorySources.TryGetValue(category, out var source) && source.isPlaying)
        {
            source.volume = volume;
        }
    }

    public float GetCategoryVolume(SoundCategory category)
    {
        return categoryVolumes.TryGetValue(category, out var volume) ? volume : 1f;
    }

    public async UniTask FadeOut(SoundCategory category, float duration = 1f)
    {
        var source = GetSource(category);
        if (source == null || !source.isPlaying) return;

        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            await UniTask.Yield();
        }

        source.Stop();
        source.volume = startVolume; // Reset for next play
    }

    public async UniTask PlayForDuration(SoundCategory category, SoundType type, float duration, float volume = 1f)
    {
        PlayLoop(category, type, volume);
        await UniTask.Delay((int)(duration * 1000));
        await FadeOut(category, 0.5f);
    }

    public bool IsPlaying(SoundCategory category)
    {
        var source = GetSource(category);
        return source != null && source.isPlaying;
    }

    #endregion
}
