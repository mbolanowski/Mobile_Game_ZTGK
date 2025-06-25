using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<GameObject> tilePrefabs;
    public List<GameObject> tilePrefabsR;

    public GameObject nextSpawn;
    public GameObject nextSpawnR;

    public float zScaleFactor;

    public int LastSpawned = 100;
    public float CurrentLevel = 0;
}
