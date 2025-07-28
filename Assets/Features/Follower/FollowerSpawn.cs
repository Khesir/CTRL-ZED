using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowerSpawn : MonoBehaviour
{
    public Transform spawnArea;
    public GameObject followerPrefab;
    public float spawnRadius = 5f;
    public List<GameObject> SpawnedFollowers { get; private set; } = new();

    public void Setup(List<CharacterBattleState> characterServices)
    {
        foreach (var characterService in characterServices)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
                spawnArea.position.x + randomCircle.x,
                spawnArea.position.y,
                0f
            );

            var go = Instantiate(followerPrefab, spawnPosition, transform.rotation);
            var follower = go.GetComponent<Follower>();
            var img = go.GetComponent<SpriteRenderer>();

            img.sprite = characterService.data.GetInstance().ship;
            follower.characterData = characterService.data;

            GameplayManager.Instance.followerManager.AddFollower(follower);
            SpawnedFollowers.Add(go);
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnArea == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spawnArea.position, spawnRadius);
    }
}
