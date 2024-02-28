using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PathAIMovement
{
    public static PlayerMovement Instance { get; private set; }
    private void Awake()
    {
        Instance = this;//Singleton Pattern
        OnMovementComplete += PlayerMovement_OnMovementComplete;
    }

    private void PlayerMovement_OnMovementComplete(object sender, EventArgs e)
    {
        GameManager.Instance.SetState(GameManager.GameState.gameplay);
    }

    public override void QueueTileMovement(List<Tile> targetTiles)
    {
        //Return based on if the player can move
        base.QueueTileMovement(targetTiles);
    }

}
