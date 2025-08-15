using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<CategorizedSound> soundEntries = new List<CategorizedSound>();
    private Dictionary<SoundCategory, Dictionary<SoundType, AudioClip>> categoryMap;
    private Dictionary<SoundCategory, AudioSource> categorySources;
    private Dictionary<SoundType, AudioClip> typeMap;
    private HashSet<SoundCategory> lockedCategories = new HashSet<SoundCategory>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public async UniTask Initialize()
    {
        categorySources = new Dictionary<SoundCategory, AudioSource>();
        foreach (SoundCategory category in System.Enum.GetValues(typeof(SoundCategory)))
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            if (category == SoundCategory.BGM)
                src.loop = true;
            categorySources[category] = src;
        }

        BuildSoundMaps();
        await UniTask.CompletedTask;
    }
    private void BuildSoundMaps()
    {
        categoryMap = new Dictionary<SoundCategory, Dictionary<SoundType, AudioClip>>();
        typeMap = new Dictionary<SoundType, AudioClip>();

        foreach (var entry in soundEntries)
        {
            if (!categoryMap.ContainsKey(entry.category))
                categoryMap[entry.category] = new Dictionary<SoundType, AudioClip>();

            categoryMap[entry.category][entry.soundType] = entry.clip;
            typeMap[entry.soundType] = entry.clip;
        }
    }

    public static void PlaySound(SoundCategory category, SoundType type, float volume = 1f)
    {
        if (Instance.categoryMap.TryGetValue(category, out var typeDict) &&
            typeDict.TryGetValue(type, out var clip) && clip != null)
        {
            AudioSource src = Instance.categorySources[category];

            if (category == SoundCategory.BGM)
            {
                src.clip = clip;
                src.volume = volume;
                src.Play();
            }
            else
            {
                src.PlayOneShot(clip, volume);
            }
        }
    }

    public static async UniTask FadeOutCategory(SoundCategory category, float duration = 1f)
    {
        if (Instance.categorySources.TryGetValue(category, out var src))
        {
            float startVolume = src.volume;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                src.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                await UniTask.Yield();
            }

            src.Stop();
            src.volume = startVolume; // Reset for next use
        }
    }
    public static async UniTask PlayLoopUntil(SoundCategory category, SoundType type, float duration)
    {
        // Lock category so nothing overrides it
        Instance.lockedCategories.Add(category);

        // Play music
        PlaySound(category, type);

        // Wait for the duration
        await UniTask.Delay((int)(duration * 1000));

        // Unlock category
        Instance.lockedCategories.Remove(category);

        // Fade out now that lock is released
        await FadeOutCategory(category, 1f);
    }
}
