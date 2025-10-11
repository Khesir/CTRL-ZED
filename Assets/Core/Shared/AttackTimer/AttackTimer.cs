using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public float timeLimit = 5f;
    private float timer;
    public float minAttackTimerDamage = 1;
    public float maxAttackTimerDamage = 10;
    private PlayerService playerInstance;
    public float timeElapse;
    public void Setup(PlayerService playerInstance)
    {
        this.playerInstance = playerInstance;
        timer = timeLimit;
    }
    public async void TriggerTimer()
    {
        timer -= Time.deltaTime;
        timeElapse += Time.deltaTime;
        if (timer <= 0f)
        {
            var damage = Random.Range(minAttackTimerDamage, maxAttackTimerDamage);
            playerInstance.TakeDamage(damage);
            if (playerInstance.IsDead())
            {
                Debug.Log("Game Over");
                GameplayManager.Instance.followerManager.ResetTarget();
                GameplayManager.Instance.endGameState = GameplayManager.GameplayEndGameState.DeathOnTimer;
                await GameplayManager.Instance.SetState(GameplayManager.GameplayState.End);
            }
            timer = timeLimit;
        }

        UpdateAttackTimer(timer);
    }
    private void UpdateAttackTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f / 10f);

        timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
    public void SetupTimer(float timeLimit)
    {
        this.timeLimit = timeLimit;
    }
    public void SetupAttackTimerDmaage(float min, float max)
    {
        maxAttackTimerDamage = max;
        minAttackTimerDamage = min;
    }
}
