using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : PathAIMovement
{

    public void Start()
    {
        PlayerMovement.Instance.OnMovementComplete += Player_OnMovementComplete;
        this.OnMovementComplete += EnemyAI_OnMovementComplete;
        GridGenerator.Instance.OnGridGenerated += GridGenerator_OnGridGenerated;
    }

    private void GridGenerator_OnGridGenerated(object sender, EventArgs e)
    {
        SetTileParent(GridGenerator.Instance.GetTileWithCordinates()[startIndex].tileObject);
    }

    private void EnemyAI_OnMovementComplete(object sender, EventArgs e)
    {
        GameManager.Instance.SetState(GameManager.GameState.gameplay);
    }

    private void Player_OnMovementComplete(object sender, EventArgs e)
    {
        GameManager.Instance.SetState(GameManager.GameState.enemyMoving);

        List<Tile> newPath = PathGenerator.StartPathFindingAStar(GetParentTileIndex(), PlayerMovement.Instance.GetParentTileIndex());
        newPath.RemoveAt(newPath.Count - 1);

        //Highlight differennt for enemy
        newPath.ForEach(tile => tile.ChangeHighlight(true, true));


        QueueTileMovement(newPath);
    }

}
