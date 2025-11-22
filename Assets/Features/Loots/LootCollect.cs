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

        if (!attracting && distanceToPlayer < attractRadius)
        {
            attracting = true;
        }

        if (attracting)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            if (distanceToPlayer < 0.5f)
            {
                ServiceLocator.Get<ISoundService>().Play(SoundCategory.Gameplay, SoundType.Gameplay_Collect);

                // Publish event instead of direct UI call
                SceneEventBus.Publish(new LootCollectedEvent
                {
                    Data = data,
                    Position = transform.position
                });

                ServiceLocator.Get<ILootManager>().UnregisterLoot(this);
                Destroy(gameObject);
            }
        }
    }
}
