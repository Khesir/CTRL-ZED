using Cysharp.Threading.Tasks;

public interface ISoundService
{
    // Playback
    void Play(SoundCategory category, SoundType type, float volume = 1f);
    void PlayLoop(SoundCategory category, SoundType type, float volume = 1f);
    void Stop(SoundCategory category);
    void StopAll();

    // Volume control
    void SetCategoryVolume(SoundCategory category, float volume);
    float GetCategoryVolume(SoundCategory category);

    // Async operations
    UniTask FadeOut(SoundCategory category, float duration = 1f);
    UniTask PlayForDuration(SoundCategory category, SoundType type, float duration, float volume = 1f);

    // State
    bool IsPlaying(SoundCategory category);
}
