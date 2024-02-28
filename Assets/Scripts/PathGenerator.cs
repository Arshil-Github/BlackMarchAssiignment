
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathGenerator : MonoBehaviour
{
    public static PathGenerator Instance;

    private static List<GridGenerator.TileWithCordinate> tilesData;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
       GridGenerator.Instance.OnGridGenerated += GridGenerator_OnGridGenerated;

    }


    private void GridGenerator_OnGridGenerated(object sender, EventArgs e)
    {
        UnityEngine.Debug.Log("Heya");
        SetTilesData(GridGenerator.Instance.GetTileWithCordinates());

    }
    public void SetTilesData (List<GridGenerator.TileWithCordinate> data) { tilesData = data; }

    public List<Tile> StartPathFindingBFS(int start, int end, Action<GridGenerator.TileWithCordinate> tileCallback = null)
    {
        //This algorithm uses Breadh first search

        int maxIndex = GridGenerator.Instance.GetMaxIndex();
        List<int> queue = new List<int>
        {
            start
        };//The queue of tiles Index that are next in line for visiting

        bool[] visited = new bool[maxIndex];//For keeping record of all the tiles that have been visited
        visited[start] = true;

        int[] prev = new int[maxIndex];//For keeping record of which tiles is reached by which index

        while(queue.Count > 0)
        {
            int node = queue[0];//Get one index and pop it
            queue.RemoveAt(0);

            List<int> neighbors = tilesData[node].node.neighborsIndex;//Get this from tile object

            for(int i = 0; i < neighbors.Count; i++)
            {
                if (!visited[neighbors[i]])
                {
                    //Only add to the queue if its not empty

                    //if it has obstacle ---> visited is true but dont do anything
                    //if its not --> add to the queue

                    if (!tilesData[neighbors[i]].tileObject.HasChild())
                    {
                        queue.Add(neighbors[i]);
                        prev[neighbors[i]] = node;
                    }
                    visited[neighbors[i]] = true;

                }
            }
        }

        //Run a reverse function on prev to get path
        List<int> path = new List<int>();

        for(int i = end; i != start ; i = prev[i])
        {
            UnityEngine.Debug.Log("Running");
            path.Add(i);
        }
        //path.Add(start);
        path.Reverse();

        List<Tile> pathTiles = new List<Tile>();
        path.ForEach(i =>
        {
            if (tileCallback != null) tileCallback(tilesData[i]);
            pathTiles.Add(tilesData[i].tileObject);
        });

        return pathTiles;
    }

    /*public static List<Tile> StartPathFindingAStar(int start, int end)
    {
        Stopwatch sw  = new Stopwatch();
        sw.Start();

        int maxIndex = GridGenerator.Instance.GetMaxIndex();

        List<int> openTiles = new List<int>(maxIndex);
        openTiles.Add(start);
        List<int> closedTiles = new List<int>(maxIndex);

        int[] prev = new int[maxIndex];//For keeping record of which tiles is reached by which index

        while (openTiles.Count > 0)
        {
            int current = openTiles[0];//Change this to lowest f cost

            for(int i = 0; i < openTiles.Count; i++)
            {

                if ((tilesData[openTiles[i]].tileObject.GetGridData().FCost < tilesData[current].tileObject.GetGridData().FCost) || (tilesData[openTiles[i]].tileObject.GetGridData().FCost == tilesData[current].tileObject.GetGridData().FCost && tilesData[openTiles[i]].tileObject.GetGridData().hCost < tilesData[current].tileObject.GetGridData().hCost))
                {
                    current = openTiles[i];
                }
            }

            openTiles.Remove(current);
            closedTiles.Add(current);

            if (current == end)
            {
                break;
            }

            foreach(int neighbor in tilesData[current].tileObject.GetGridData().neighborsIndex)
            {

                if (tilesData[neighbor].tileObject.HasChild() || closedTiles.Contains(neighbor))
                {
                    continue;
                }

                //If the new path to neighbour is shorter or if the neighbour is not in open
                //Set a new f cost
                //Set parent of neighbour to current
                //if neighbour is not in oopen add it 

                int newMovementCostToNeighbor = tilesData[current].tileObject.GetGridData().gCost + GetDistance(current, neighbor);

                if(newMovementCostToNeighbor < tilesData[neighbor].tileObject.GetGridData().gCost || !openTiles.Contains(neighbor))
                {
                    int newFCost = GetDistance(neighbor, end);
                    tilesData[neighbor].tileObject.SetPathFinderData(newMovementCostToNeighbor, newFCost);
                    prev[neighbor] = current;

                    //Debug.Log($"{neighbor} : fCost - {tilesData[neighbor].tileObject.GetGridData().fCost}");

                    if (!openTiles.Contains(neighbor))
                    {
                        openTiles.Add(neighbor);
                    }
                }
            }

        }

        int GetDistance(int startIndex, int endIndex)
        {
            Vector2 startCoordinate = tilesData[startIndex].tileObject.GetGridData().coordinates;
            Vector2 endCoordinate = tilesData[endIndex].tileObject.GetGridData().coordinates;

            int distX = (int)Mathf.Abs(endCoordinate.x - startCoordinate.x);
            int distY = (int)Mathf.Abs(endCoordinate.y - startCoordinate.y);

            return distX + distY;
        }

        //Run a reverse function on prev to get path
        List<int> path = new List<int>();
        UnityEngine.Debug.Log(prev[start]);
        for (int i = end; i != start; i = prev[i])
        {
            if (path.Count > maxIndex)
            {
                UnityEngine.Debug.Log($"{start} to {end}");
                break;
            }
            path.Add(i);
        }
        //path.Add(start);
        path.Reverse();

        List<Tile> pathTiles = new List<Tile>();
        path.ForEach(i =>
        {
            pathTiles.Add(tilesData[i].tileObject);
        });

        closedTiles.ForEach(i =>
        {
            tilesData[i].tileObject.SetPathFinderData(0, 0);
        });
        return pathTiles;
    }*/
    public static List<Tile> StartPathFindingAStar(int start, int end)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        bool pathFound = false;

        int maxIndex = GridGenerator.Instance.GetMaxIndex();

        List<int> openTiles = new List<int>(maxIndex);
        openTiles.Add(start);
        List<int> closedTiles = new List<int>(maxIndex);

        int[] prev = new int[maxIndex];//For keeping record of which tiles is reached by which index

        while (openTiles.Count > 0)
        {
            int current = openTiles[0];//Change this to lowest f cost

            for (int i = 0; i < openTiles.Count; i++)
            {

                if ((tilesData[openTiles[i]].node.fCost < tilesData[current].node.fCost) || (tilesData[openTiles[i]].node.fCost == tilesData[current].node.fCost && tilesData[openTiles[i]].node.hCost < tilesData[current].node.hCost))
                {
                    current = openTiles[i];
                }
            }

            openTiles.Remove(current);
            closedTiles.Add(current);

            if (current == end)
            {
                pathFound = true;
                break;
            }

            foreach (Node neighborNode in tilesData[current].node.neighborNodes)
            {
                int neighbor = neighborNode.index;
                if (!tilesData[neighbor].node.isWalkable || closedTiles.Contains(neighbor))
                {
                    continue;
                }

                //If the new path to neighbour is shorter or if the neighbour is not in open
                //Set a new f cost
                //Set parent of neighbour to current
                //if neighbour is not in oopen add it 

                int newMovementCostToNeighbor = tilesData[current].node.gCost + GetDistance(current, neighbor);

                if (newMovementCostToNeighbor < tilesData[neighbor].node.gCost || !openTiles.Contains(neighbor))
                {
                    int newHCost = GetDistance(neighbor, end);
                    tilesData[neighbor].node.hCost = newHCost;
                    tilesData[neighbor].node.gCost = newMovementCostToNeighbor;

                    //tilesData[neighbor].tileObject.SetPathFinderData(newMovementCostToNeighbor, newHCost);
                    prev[neighbor] = current;

                    //Debug.Log($"{neighbor} : fCost - {tilesData[neighbor].tileObject.GetGridData().fCost}");

                    if (!openTiles.Contains(neighbor))
                    {
                        openTiles.Add(neighbor);
                    }
                }
            }

        }

        int GetDistance(int startIndex, int endIndex)
        {
            Vector2 startCoordinate = tilesData[startIndex].node.coordinates;
            Vector2 endCoordinate = tilesData[endIndex].node.coordinates;

            int distX = (int)Mathf.Abs(endCoordinate.x - startCoordinate.x);
            int distY = (int)Mathf.Abs(endCoordinate.y - startCoordinate.y);

            return distX + distY;
        }

        //If no path is found simply return an empty list
        if (!pathFound) {
            UnityEngine.Debug.Log("Path not found");
            return new List<Tile>();
        }

        //Run a reverse function on prev to get path
        List<int> path = new List<int>();
        for (int i = end; i != start; i = prev[i])
        {
            if (path.Count > maxIndex)
            {
                UnityEngine.Debug.Log($"{start} to {end}");
                break;
            }
            path.Add(i);
        }
        //path.Add(start);
        path.Reverse();

        List<Tile> pathTiles = new List<Tile>();
        path.ForEach(i =>
        {
            pathTiles.Add(tilesData[i].tileObject);
        });

        return pathTiles;
    }

}
