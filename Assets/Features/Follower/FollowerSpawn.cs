using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                0f
            );

            var go = Instantiate(followerPrefab, spawnPosition, transform.rotation);
            var follower = go.GetComponent<Follower>();
            var playerController = go.GetComponent<PlayerController>();
            var img = go.GetComponent<SpriteRenderer>();
            img.sprite = characterService.GetInstance().baseData.ship;
            follower.characterData = characterService;
            playerController.playerData = characterService;
            GameplayManager.Instance.AddFollower(follower);
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnArea == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(spawnArea.position, spawnRadius);
    }
}
