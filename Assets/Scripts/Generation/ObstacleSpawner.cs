using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Przeszkody do wyboru
    public Transform player; // Odnośnik do ptaka
    public float spawnDistance = 100f; // Jak daleko przed graczem generować przeszkody
    public float removeDistance = 50f; // Jak daleko za graczem usunąć przeszkody

    public int tilesPerRow = 8; // Ile przeszkód szeroko w jednym rzędzie
    public float tileSpacingX = 15f; // Odległość między przeszkodami w poziomie
    public float rowSpacingZ = 30f; // Odległość między rzędami przeszkód

    private float lastSpawnZ = 0f;
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Update()
    {
        float targetZ = player.position.z - spawnDistance;

        if (targetZ < lastSpawnZ - rowSpacingZ)
        {
            SpawnRow(lastSpawnZ - rowSpacingZ);
            lastSpawnZ -= rowSpacingZ;
        }

        CleanupObstacles();
    }

    void SpawnRow(float zPos)
    {
        for (int i = -tilesPerRow / 2; i <= tilesPerRow / 2; i++)
        {
            if (Random.value < 0.6f) // Szansa na wygenerowanie przeszkody
            {
                Vector3 pos = new Vector3(i * tileSpacingX, 0f, zPos);
                GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                GameObject obstacle = Instantiate(prefab, pos, Quaternion.identity);
                spawnedObstacles.Add(obstacle);
            }
        }
    }

    void CleanupObstacles()
    {
        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            if (spawnedObstacles[i] == null) continue;

            if (spawnedObstacles[i].transform.position.z > player.position.z + removeDistance)
            {
                Destroy(spawnedObstacles[i]);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }
}
