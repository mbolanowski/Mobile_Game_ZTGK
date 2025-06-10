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
                    gameObject.transform.parent.position = new Vector3(
                    gameObject.transform.parent.position.x + 160f,
                    gameObject.transform.parent.position.y,
                    gameObject.transform.parent.position.z
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
                    gameObject.transform.parent.position = new Vector3(
                    gameObject.transform.parent.position.x - 160f,
                    gameObject.transform.parent.position.y,
                    gameObject.transform.parent.position.z
                    );
                }
            }
            else if (gameObject.name == "FrontDetection")
            {
                Vector3 currentPos = gameObject.transform.parent.position;
                if (tilePrefabs != null && tilePrefabs.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Count);
                    GameObject randomPrefab = tilePrefabs[randomIndex];
                    GameObject instance = Instantiate(randomPrefab, currentPos + new Vector3(0f, 0f, -170f), Quaternion.identity);
                    float scaleZ = zScaleFactor * (ts.actualFlySpeed / baseFlySpeed);
                    Vector3 scale = instance.transform.localScale;
                    scale.z *= scaleZ;
                    instance.transform.localScale = scale;
                    manager.LastSpawned = randomIndex;
                }
            }
            else if (gameObject.name == "FrontDetectionR")
            {
                Vector3 currentPos = gameObject.transform.parent.position;
                if (tilePrefabsR != null && tilePrefabsR.Count > 0)
                {
                    int randomIndex = manager.LastSpawned;
                    GameObject randomPrefab = tilePrefabsR[randomIndex];
                    GameObject instance = Instantiate(randomPrefab, currentPos + new Vector3(0f, 0f, -170f), Quaternion.identity);
                    float scaleZ = zScaleFactor * (ts.actualFlySpeed / baseFlySpeed);
                    Vector3 scale = instance.transform.localScale;
                    scale.z *= scaleZ;
                    instance.transform.localScale = scale;
                }
            }
            else if (gameObject.name == "DeleteDetection")
            {
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }
}
