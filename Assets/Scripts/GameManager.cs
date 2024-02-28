using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState { 
        beforePlay,
        playerMoving,
        enemyMoving,
        gameplay
    }
    private GameState state;
    public static GameManager Instance { get; private set; }//Singleton Pattern

    private void Awake()
    {
        Instance = this;
        SetState(GameState.beforePlay);
    }
    public void SetState(GameState newState)
    {
        state = newState;
    }

    public void QueuePlayerMovement(int endIndex)
    {
        if (state != GameState.gameplay) return;

        SetState(GameState.playerMoving);

        List<Tile> pathTileList = PathGenerator.StartPathFindingAStar(PlayerMovement.Instance.GetParentTileIndex(), endIndex);
        PlayerMovement.Instance.QueueTileMovement(pathTileList);

        pathTileList.ForEach(tile => { tile.ChangeHighlight(true); });
    }
}
