using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollect : MonoBehaviour
{
    public LootDropData data;
    public float attractRadius = 3f;
    public float moveSpeed = 5f;
    private bool attracting = false;

    public void TryAttract(Transform player)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Start attracting if within range
        if (!attracting && distanceToPlayer < attractRadius)
        {
            attracting = true;
        }

        // Move toward player if attraction is active
        if (attracting)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            if (distanceToPlayer < 0.5f)
            {
                SoundManager.PlaySound(SoundCategory.Gameplay, SoundType.Gameplay_Collect);
                GameplayManager.Instance.gameplayUI.lootHolder.AddAmount(data);
                GameplayManager.Instance.lootManager.UnregisterLoot(this);
                Destroy(gameObject);
            }
        }
    }
}
