using System.Collections;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // List of prefabs to choose from
    public float spawnInterval = 10f; // Time in seconds between spawns

    private GameObject currentSpawnedObject;
    private Coroutine spawnCoroutine;

    void Start()
    {
        StartSpawnTimer();
    }

    void SpawnObject()
    {
        if (prefabsToSpawn.Length == 0) return; // Ensure there are prefabs to spawn

        GameObject selectedPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
        currentSpawnedObject = Instantiate(selectedPrefab, transform.position, transform.rotation);
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

        if(currentSpawnedObject.transform.childCount == 0)
        {
            Destroy(currentSpawnedObject);
            currentSpawnedObject = null;
        }
    }
}