using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public Transform spawnPoint;
    public float spawnInterval = 5f; 
    public float obstacleSpawnInterval = 7f;

    private GameObject currentSpawnedObject;
    private Coroutine spawnCoroutine;
    
    private Coroutine obstacleSpawnCoroutine;
    public GameObject[] obstaclePrefabs; 
    public int laneCount = 3;
    public float laneWidth = 1.5f;
    private List<Transform> obstacleSpawnPoints = new List<Transform>();

    private bool obstacleSpawned = false;

    void Start()
    {
        GenerateObstacleSpawnPoints();
        StartObstacleSpawnTimer();
        StartSpawnTimer(); 
    }

    void StartObstacleSpawnTimer()
    {
        if (obstacleSpawnCoroutine != null)
        {
            StopCoroutine(obstacleSpawnCoroutine);
        }
        obstacleSpawnCoroutine = StartCoroutine(ObstacleSpawnRoutine());
    }

    IEnumerator ObstacleSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(obstacleSpawnInterval);
            SpawnObstacle(); // Spawn obstacle at regular intervals
        }
    }
    
    void GenerateObstacleSpawnPoints()
    {
        float totalWidth = (laneCount - 1) * laneWidth;
        float startX = -totalWidth / 2f;

        obstacleSpawnPoints.Clear();

        for (int i = 0; i < laneCount; i++)
        {
            Vector3 pos = new Vector3(startX + i * laneWidth, 5.5f, 1);
        
            GameObject point = new GameObject("ObstacleSpawnPoint_" + i);
            point.transform.position = pos;

            obstacleSpawnPoints.Add(point.transform);
        }
    }

    void SpawnObject()
    {
        if (prefabsToSpawn.Length == 0 || currentSpawnedObject != null) return; 

        GameObject selectedPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
        currentSpawnedObject = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

        StartSpawnTimer(); 
    }

    void StartSpawnTimer()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnInterval); 
        SpawnObject();
    }

    void Update()
    {
        if (currentSpawnedObject == null) 
        {
            SpawnObject();
        }

        // Reset obstacle spawn flag if it is off-screen
        ResetObstacleFlagIfNeeded();
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0 || obstacleSpawnPoints.Count == 0 || obstacleSpawned)
            return; // Ensure we only spawn one obstacle at a time

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
    
        Transform point = obstacleSpawnPoints[Random.Range(0, obstacleSpawnPoints.Count)];

        GameObject spawnedObstacle = Instantiate(prefab, point.position, Quaternion.identity);

        spawnedObstacle.layer = LayerMask.NameToLayer("Obstacle");

        obstacleSpawned = true; // Mark that an obstacle is now on screen
    }

    // Reset the obstacle spawn flag if the obstacle goes off-screen
    void ResetObstacleFlagIfNeeded()
    {
        // Check if any obstacle is off-screen and reset the spawn flag
        if (obstacleSpawned && !IsObstacleOnScreen())
        {
            obstacleSpawned = false; // Reset the flag so a new obstacle can spawn
        }
    }

    bool IsObstacleOnScreen()
    {
        // Add logic to check if the obstacle is off-screen (e.g., using its position)
        // This example assumes the obstacles are on the "Obstacle" layer
        Collider[] obstacles = Physics.OverlapSphere(Vector3.zero, 50f, LayerMask.GetMask("Obstacle"));
        return obstacles.Length > 0;
    }
}
