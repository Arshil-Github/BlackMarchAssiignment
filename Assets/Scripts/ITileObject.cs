using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileObject
{
    public Tile GetTileParent();
    public void SetTileParent(Tile parentTile);
    public void ClearParent();
}
