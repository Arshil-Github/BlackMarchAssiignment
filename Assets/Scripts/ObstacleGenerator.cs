using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    //This script handles the obstacle generation
    public static ObstacleGenerator Instance { get; private set;}
    [SerializeField] private Transform pf_obstacle;

    private void Awake()
    {
        Instance = this;
    }
    public void SpawnObstacle(Tile tile)
    {
        Transform spawnedObstacle = Instantiate(pf_obstacle, tile.transform);

        spawnedObstacle.GetComponent<Obstacle>().SetTileParent(tile);
    }
}
