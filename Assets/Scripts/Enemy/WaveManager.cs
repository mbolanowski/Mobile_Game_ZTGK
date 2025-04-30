using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public Transform spawnPoint; // The spawn location
    public float spawnInterval = 5f; // Time in seconds between spawns
    public float obstacleSpawnInterval = 7f;

    private GameObject currentSpawnedObject;
    private Coroutine spawnCoroutine;
    
    private Coroutine obstacleSpawnCoroutine;
    public GameObject[] obstaclePrefabs; // List of prefabs to choose from
    public int laneCount = 3;
    public float laneWidth = 1.5f;
    private List<Transform> obstacleSpawnPoints = new List<Transform>();

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
            SpawnObstacle();
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
        if (prefabsToSpawn.Length == 0) return; // Ensure there are prefabs to spawn

        GameObject selectedPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
        currentSpawnedObject = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

        SpawnObstacle(); // Spawn obstacles when an object is spawned

        StartSpawnTimer(); // Restart countdown whenever a new object is spawned
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
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0 || obstacleSpawnPoints.Count == 0)
            return;

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
    
        Transform point = obstacleSpawnPoints[Random.Range(0, obstacleSpawnPoints.Count)];

        GameObject spawnedObstacle = Instantiate(prefab, point.position, Quaternion.identity);

        spawnedObstacle.layer = LayerMask.NameToLayer("Obstacle");
    }
}