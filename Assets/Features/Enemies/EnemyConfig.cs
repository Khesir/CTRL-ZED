using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy")]
public class EnemyConfig : ScriptableObject
{
    public string enemyName;
    public float maxHealth = 50f;
    public float baseDamage = 10f;
    public float defense = 10f;
    public float dex = 10f;
    public int experienceToGive = 10;
    public GameObject destroyEffect;
    public Sprite sprite;
    public int difficultyMultiplier = 3;

    [Header("AI Settings")]
    public float movementSpeed = 3f;

    public float detectionRange = 5f;
    public float stopDistance = 1f;

    [Header("Misc")]
    public List<LootDropData> lootDrops;
}
