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
    private PlayerService instance;
    public void Setup(PlayerService instance)
    {
        this.instance = instance;
        timer = timeLimit;
    }
    public void TriggerTimer()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            var damage = Random.Range(minAttackTimerDamage, maxAttackTimerDamage);
            instance.TakeDamage(damage);
            timer = timeLimit;
            Debug.Log($"OS Taken heavy Damage {damage}");
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
        timer = timeLimit;
    }
    public void SetupAttackTimerDmaage(float min, float max)
    {
        maxAttackTimerDamage = max;
        minAttackTimerDamage = min;
    }
}
