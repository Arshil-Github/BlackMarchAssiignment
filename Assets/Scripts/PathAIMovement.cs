using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAIMovement : MonoBehaviour, ITileObject
{
    private Tile tileParent;

    [SerializeField] private float moveTimerMax = 0.5f;//time between times --> Should be set before
    [SerializeField] protected int startIndex = 0;

    private float moveTimer = 0f;//updated in realtime
    private List<Tile> moveQueue = new List<Tile>();//For keeping record of which tiles to move to
    public event EventHandler OnMovementComplete;

    private void Start()
    {
        GridGenerator.Instance.OnGridGenerated += GridGenerator_OnGridGenerated;
    }
    private void GridGenerator_OnGridGenerated(object sender, System.EventArgs e)
    {
        SetTileParent(GridGenerator.Instance.GetTileWithCordinates()[startIndex].tileObject);
    }
    private void Update()
    {
        if (moveQueue.Count > 0)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > moveTimerMax)
            {
                SetTileParent(moveQueue[0]);
                moveTimer = 0f;

                moveQueue[0].ChangeHighlight(false);

                moveQueue.RemoveAt(0);

                if (moveQueue.Count == 0)
                {
                    OnMovementComplete?.Invoke(this, EventArgs.Empty);
                    //Movement is completed
                    GridGenerator.Instance.SetObjectOnTile(GetParentTileIndex(), true);
                }
            }
        }
    }

    public virtual void QueueTileMovement(List<Tile> targetTiles)
    {
        //for defining the queue tile
        if (moveQueue.Count > 0) return;

        //If targetTiles.Empty simply change state
        if (targetTiles.Count == 0)
        {
            GameManager.Instance.SetState(GameManager.GameState.gameplay);
            return;
        }

        //Movement is started
        GridGenerator.Instance.SetObjectOnTile(GetParentTileIndex(), false);

        moveQueue = targetTiles;
    }
    public void SetTileParent(Tile parentTile)
    {
        if (tileParent != null) tileParent.ClearChildTileObject();

        parentTile.SetChildTileObject(this);
        tileParent = parentTile;

        //Change transform parent to parentTile
        transform.parent = parentTile.transform;
        //Change Position to the localPosition of Tile
        transform.localPosition = parentTile.GetTileObjectLocalPosition();
    }
    public void ClearParent() { tileParent = null; }

    public Tile GetTileParent() { return tileParent; }
    public int GetParentTileIndex()
    {
        if (tileParent == null) return -1;
        return tileParent.GetGridData().index;
    }
}
