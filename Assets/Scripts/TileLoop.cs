using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileLoop : MonoBehaviour
{
    [SerializeField] private List<GameObject> tilePrefabs;

    private void Start()
    {
        TileManager manager = FindObjectOfType<TileManager>();
        if (manager != null)
        {
            tilePrefabs = new List<GameObject>(manager.tilePrefabs);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (gameObject.name == "LeftDetection")
            {
                gameObject.transform.parent.position = new Vector3(
                    gameObject.transform.parent.position.x + 60f,
                    gameObject.transform.parent.position.y,
                    gameObject.transform.parent.position.z
                );
            }
            else if (gameObject.name == "RightDetection")
            {
                gameObject.transform.parent.position = new Vector3(
                    gameObject.transform.parent.position.x - 60f,
                    gameObject.transform.parent.position.y,
                    gameObject.transform.parent.position.z
                );
            }
            else if (gameObject.name == "FrontDetection")
            {
                Vector3 currentPos = gameObject.transform.parent.position;
                if (tilePrefabs != null && tilePrefabs.Count > 0)
                {
                    int randomIndex = Random.Range(0, tilePrefabs.Count);
                    GameObject randomPrefab = tilePrefabs[randomIndex];
                    Instantiate(randomPrefab, currentPos + new Vector3(0f, 0f, -170f), Quaternion.identity);
                }
            }
        }
    }
}
