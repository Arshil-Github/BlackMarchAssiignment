using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
    [SerializeField]private MeshRenderer myMesh;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material enemyMaterial;
    [SerializeField] private Tile tile;

    private void Start()
    {
        tile.OnHighlight += Tile_OnHighlight;
        tile.OnNormal += Tile_OnNormal;

        myMesh.material = normalMaterial;
    }

    private void Tile_OnHighlight(object sender, int e)
    {
        if(e == 0)
        {
            myMesh.material = highlightMaterial;
        }
        else
        {
            myMesh.material = enemyMaterial;
        }
    }

    private void Tile_OnNormal(object sender, System.EventArgs e)
    {
        myMesh.material = normalMaterial;
    }
}
