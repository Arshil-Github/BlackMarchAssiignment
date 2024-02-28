using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator Instance;

    public event EventHandler OnGridGenerated;

    [SerializeField] private float padding;//Distance between Tiles
    [SerializeField] private Vector2 gridVector; //The row and column number
    [SerializeField] private Transform pf_tile; //Prefab for a tile Object
    [SerializeField] private GameObject startingPosition; //The grid starts here
    [SerializeField] private GridDataSO gridDataSO;


    public struct TileWithCordinate //For linking a coordinate with a tile Object for future use
    {
        public Vector2 cordinate;
        public Tile tileObject;
        public Node node;
        public int index;
    }
    public struct NeighbourData
    { 
        public List<Vector2> neighbors;
        public List<int> neighborsIndex;
        public List<Node> neighborNodes;
    }

    private List<TileWithCordinate> tilesData = new List<TileWithCordinate>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GenerateGrid();
    }
    private void GenerateGrid()
    {
        float sizeOfCell = pf_tile.transform.localScale.x + padding;

        int globalIndex = 0;
        for (int  i = 0; i < gridVector.x ; i++)
        {
            //Create a row empty GameObject
            GameObject newRowGameObject = new GameObject();
            newRowGameObject.name = $"Row {i}";
            Transform newRowParent = newRowGameObject.transform;
            newRowParent.parent = transform;

            //Run a loop to add tiles to the instantiateed row
            for (int j = 0; j < gridVector.y; j++)
            {

                int index = globalIndex++;
                //Caluclating the spawn Position based on the value of i and j
                Vector3 spawnPosition = startingPosition.transform.position + new Vector3(-i * sizeOfCell, 0, j * sizeOfCell);

                //Spawn a tile and set its coordinate
                Transform spawnedTile = Instantiate(pf_tile, spawnPosition, Quaternion.identity);
                spawnedTile.parent = newRowParent;

                spawnedTile.GetComponent<Tile>().SetGridData(new Vector2(i, j), index);

                Node newNode = new Node(new Vector2(i, j), index);

                //Add this tile to the tileData
                tilesData.Add(new TileWithCordinate { cordinate = new Vector2(i, j), tileObject = spawnedTile.GetComponent<Tile>(), index = index, node = newNode });
            }
        }

        AddObstacles();

        foreach (TileWithCordinate tileWithCordinate in tilesData)
        {
            //Go to each tile and Call the add neighbor function
            //tileWithCordinate.tileObject.SetNeighbors(CalculateNeighbors(tileWithCordinate.cordinate, tileWithCordinate.index));
            tileWithCordinate.node.neighborNodes = CalculateNeighbors(tileWithCordinate.cordinate, tileWithCordinate.index).neighborNodes;
        }

        OnGridGenerated?.Invoke(this, EventArgs.Empty);
        GameManager.Instance.SetState(GameManager.GameState.gameplay);
        PathGenerator.Instance.SetTilesData(tilesData);
    }
    public void SetObjectOnTile(int index, bool isOn)
    {
        tilesData[index].node.isWalkable = !isOn;
        //Debug.Log($"{index} and {tilesData[index].node.isWalkable}");
    }
    private void AddObstacles()
    {
        //Cycle through each tile and if the coordinate matches with obstacle coordinate add obstacle
        foreach(TileWithCordinate tileWithCordinate in tilesData)
        {
            if (gridDataSO.obstacleCordinates.Contains(tileWithCordinate.cordinate))
            {
                ObstacleGenerator.Instance.SpawnObstacle(tileWithCordinate.tileObject);
                tileWithCordinate.node.isWalkable = false;
            }
        }
    }
    private NeighbourData CalculateNeighbors(Vector2 coordinates, int index)
    {
        //(0, +1);(0, -1); (+5, 0) ; (-5, 0) are the neighbours

        List<Vector2> neighbors = new List<Vector2>();
        List<int> neighborsIndex = new List<int>();
        List<Node> nodes = new List<Node>();
        bool isValidCoordinate(Vector2 toCheckCordinate)
        {
            int maxIndex = (int)gridVector.x * (int)gridVector.y;
            bool isInvalid = toCheckCordinate.x < 0 || toCheckCordinate.y < 0 || toCheckCordinate.x >= gridVector.x || toCheckCordinate.y >= gridVector.y;
            return !isInvalid;
        }

        int[] dx = { 0, 0, +1, -1};
        int[] dy = { +1, -1, 0, 0};

        for (int i = 0; i < 4; i++)
        {
            Vector2 checkingVector =  new Vector2(coordinates.x + dx[i], coordinates.y + dy[i]);


            if (isValidCoordinate(checkingVector))
            {
                neighbors.Add(checkingVector);

                int indexToAdd = (dx[i] == 0) ? dy[i] : dx[i] * (int)gridVector.x;
                neighborsIndex.Add(indexToAdd + index);
                nodes.Add(tilesData[indexToAdd + index].node);
            }
        }

        NeighbourData outputData = new NeighbourData
        {
            neighbors = neighbors,
            neighborsIndex = neighborsIndex,
            neighborNodes = nodes,
        };

        return outputData;
    }
    public int GetMaxIndex() { return (int)gridVector.x * (int)gridVector.y; }
    public List<TileWithCordinate> GetTileWithCordinates() { return tilesData; }
    public List<TileWithCordinate> GetTilesData() { return tilesData; }
}
