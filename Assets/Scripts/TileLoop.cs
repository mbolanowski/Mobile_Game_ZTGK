using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class TileLoop : MonoBehaviour
{

    private TurningScript ts;
    private float baseFlySpeed = 9f;

    [SerializeField] private float zScaleFactor = 0.8f;

    TileManager manager;

    private List<GameObject> tilePrefabs;
    private List<GameObject> tilePrefabsR;

    public float minOppositeAngle = 120f;

    private void Start()
    {
        manager = FindObjectOfType<TileManager>();
        if (manager != null)
        {
            tilePrefabs = new List<GameObject>(manager.tilePrefabs);
            tilePrefabsR = new List<GameObject>(manager.tilePrefabsR);
            zScaleFactor = manager.zScaleFactor;
        }
        ts = FindFirstObjectByType<TurningScript>();
        if (ts != null)
        {
            //baseFlySpeed = ts.FlySpeed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (gameObject.name == "LeftDetection")
            {
                Vector3 directionToOther = (other.transform.position - transform.position).normalized;
                Vector3 rightDirection = transform.right;
                float angle = Vector3.Angle(rightDirection, directionToOther);

                if (angle >= minOppositeAngle)
                {
                    gameObject.transform.parent.parent.position = new Vector3(
                    gameObject.transform.parent.parent.position.x + 160f,
                    gameObject.transform.parent.parent.position.y,
                    gameObject.transform.parent.parent.position.z
                    );
                }
            }
            else if (gameObject.name == "RightDetection")
            {
                Vector3 directionToOther = (other.transform.position - transform.position).normalized;
                Vector3 rightDirection = transform.right;
                float angle = Vector3.Angle(rightDirection, directionToOther);

                if (angle >= minOppositeAngle)
                {
                    gameObject.transform.parent.parent.position = new Vector3(
                    gameObject.transform.parent.parent.position.x - 160f,
                    gameObject.transform.parent.parent.position.y,
                    gameObject.transform.parent.parent.position.z
                    );
                }
            }
            // FrontDetection
            else if (gameObject.name == "FrontDetection")
            {
                Vector3 currentPos = gameObject.transform.parent.parent.position;
                if (tilePrefabs != null && tilePrefabs.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Count);
                    GameObject randomPrefab = tilePrefabs[randomIndex];

                    float scaleZ = zScaleFactor * (ts.currentSpeed / baseFlySpeed);
                    Vector3 spawnPos = new Vector3(currentPos.x, currentPos.y, manager.nextSpawn.transform.position.z);

                    SpawnStretchedTile(randomPrefab, spawnPos, scaleZ);

                    manager.LastSpawned = randomIndex;
                }
            }

            // FrontDetectionR
            else if (gameObject.name == "FrontDetectionR")
            {
                Vector3 currentPos = gameObject.transform.parent.parent.position;
                if (tilePrefabsR != null && tilePrefabsR.Count > 0)
                {
                    int randomIndex = manager.LastSpawned;
                    GameObject randomPrefab = tilePrefabsR[randomIndex];

                    float scaleZ = zScaleFactor * (ts.currentSpeed / baseFlySpeed);
                    Vector3 spawnPos = new Vector3(currentPos.x, currentPos.y, manager.nextSpawnR.transform.position.z);

                    SpawnStretchedTile(randomPrefab, spawnPos, scaleZ);
                }
            }

            else if (gameObject.name == "DeleteDetection")
            {
                Destroy(gameObject.transform.parent.parent.gameObject);
            }
        }
    }

    private void SpawnStretchedTile(GameObject prefab, Vector3 spawnPosition, float scaleZ)
    {
        GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);

        foreach (Transform child in instance.transform)
        {
            MeshCollider meshCol = child.GetComponent<MeshCollider>();

            if (meshCol != null)
            {
                // Move MeshCollider objects apart
                Vector3 localPos = child.localPosition;
                localPos.z *= scaleZ;
                child.localPosition = localPos;
            }
        }

        // Find the specific child named "Detections" and scale it
        Transform det = instance.transform.Find("Detections");
        if (det != null)
        {
            Vector3 detScale = det.localScale;
            detScale.z *= scaleZ;
            det.localScale = detScale;
            Transform spawn = det.transform.Find("Spawn");
            if (spawn != null)
            {
                manager.nextSpawn = spawn.gameObject;
            }
            Transform spawnr = det.transform.Find("SpawnR");
            if (spawnr != null)
            {
                manager.nextSpawnR = spawnr.gameObject;
            }
        }    
    }

}


