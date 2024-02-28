using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, ITileObject
{
    private Tile tileParent;

    public Tile GetTileParent() { return tileParent; }
    public void SetTileParent(Tile parentTile)
    {
        parentTile.SetChildTileObject(this);

        //Change transform parent to parentTile
        transform.parent = parentTile.transform;
        //Change Position to the localPosition of Tile
        transform.localPosition = parentTile.GetTileObjectLocalPosition();
    }
    public void ClearParent()
    {
        if(tileParent != null)
        {
            tileParent.ClearChildTileObject();
            tileParent = null;
        }
    }
}
