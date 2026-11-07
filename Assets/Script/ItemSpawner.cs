
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("References")]
    public Item[] obstaclePrefabs;
    public Player_Manager player;
    public Transform parent;

    public List<Item> items = new List<Item>();

    [Header("Spawning")]
    public float spawnInterval = 1.5f;
    public float spawnZ = 300f;
    public float spawnY = -3f;
    public float laneWidth = 2f;

    private float spawnTimer;

    [Header("Movement")]
    public float speed = 150f;
    public float destroyZ = -50f;

    [Header("Damage")]
    public float obstacleDamage = 5f;

    private void Update()
    {
        HandleSpawning();
        MoveObstacles();
    }

    private void HandleSpawning()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            BeginSpawn();
        }
    }

    private void BeginSpawn()
    {
        if (obstaclePrefabs.Length == 0)
            return;

        int randomPrefab = Random.Range(
            0,
            obstaclePrefabs.Length
        );

        Item spawnedItem = Instantiate(
            obstaclePrefabs[randomPrefab],
            parent
        );

        int randomLane = Random.Range(0, 3);

        float x = (randomLane - 1) * laneWidth;

        spawnedItem.itemPosition = new Vector3(
            x,
            spawnY,
            spawnZ
        );

        spawnedItem.hasHitPlayer = false;

        items.Add(spawnedItem);
    }

    private void MoveObstacles()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Item item = items[i];

            float previousZ = item.itemPosition.z;

            item.itemPosition += new Vector3(
                0f,
                0f,
                -speed * Time.deltaTime
            );

            CheckCollision(item, previousZ);

            if (item.itemPosition.z < destroyZ)
            {
                items.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }
    }

    private void CheckCollision(Item item, float previousZ)
    {
        if (item.hasHitPlayer)
            return;

        bool crossedPlayer =
            previousZ >= 0f &&
            item.itemPosition.z <= 0f;

        if (!crossedPlayer)
            return;

        item.hasHitPlayer = true;

        if (player.CheckCollision(item))
        {
            player.TakeDamage(obstacleDamage);
        }
    }
}
