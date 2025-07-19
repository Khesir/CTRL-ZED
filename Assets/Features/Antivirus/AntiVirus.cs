using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AntivirusEffect", menuName = "Status Effects/Antivirus/Composite")]
public class AntiVirus : StatusEffect
{
    [Header("Basic Info")]
    public string buffID = "antivirus_id_000";
    public string effectName = "Basic";
    public string description = "-5% enemy speed, +5 HP/sec, +2 Dex";
    public int cost = 500;
    public float duration = 10f;
    public Sprite icon;

    [Header("Stat Modifiers")]
    public StatusEffect playerBuff;
    public StatusEffect enemyDebuff;

    public override void Apply(GameObject target)
    {
        if (target.CompareTag("Player"))
            playerBuff?.Apply(target);
        else if (target.CompareTag("Enemy"))
            enemyDebuff?.Apply(target);
    }


    public override void Remove(GameObject target)
    {
        if (target.CompareTag("Player"))
            playerBuff?.Remove(target);
        else if (target.CompareTag("Enemy"))
            enemyDebuff?.Remove(target);
    }
}
