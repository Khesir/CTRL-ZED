using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerSpawn : MonoBehaviour
{
    public Transform spawnArea;
    public GameObject followerPrefab;
    public float spawnRadius = 5f;

    public void Setup(List<CharacterService> characterServices)
    {
        foreach (var characterService in characterServices)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
                spawnArea.position.x + randomCircle.x,
                spawnArea.position.y,
                spawnArea.position.z + randomCircle.y
            );

            var go = Instantiate(followerPrefab, spawnPosition, Quaternion.identity);
            GameplayManager.Instance.AddFollower(go.GetComponent<Follower>());
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnArea == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spawnArea.position, spawnRadius);
    }
}
