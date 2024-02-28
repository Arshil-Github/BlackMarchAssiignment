using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject tileObjectPoint;

    public event EventHandler<int> OnHighlight;
    public event EventHandler OnNormal;

    private ITileObject childTileObject;

    public struct GridData
    {
        public Vector2 coordinates;
        public int index;
    }
    private GridData gridData = new GridData();


    public void ChangeHighlight(bool highlight, bool blueHighlight = false)
    {
        if(highlight)
        {
            int highlightType = blueHighlight ? 1 : 0;
            OnHighlight?.Invoke(this, highlightType);
        }
        else
        {
            OnNormal?.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //For when the mouse enters this gameObject
        //OnHighlight?.Invoke(this, EventArgs.Empty);

        CordinatesUI.Instance.ChangeCoordinatesText(gridData.coordinates, gridData.index);
        CordinatesUI.Instance.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //For when the mouse exits this gameObject
        //OnNormal?.Invoke(this, EventArgs.Empty);

        CordinatesUI.Instance.Hide();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!HasChild())
        {
            GameManager.Instance.QueuePlayerMovement(gridData.index);
        }
    }
    public void SetGridData(Vector2 newCordinates, int index)
    {
        gridData.coordinates = newCordinates;
        gridData.index = index;
    }
    public void SetChildTileObject(ITileObject newChild)
    {
        ClearChildTileObject();
        childTileObject = newChild;
    }

    public GridData GetGridData() { return gridData; }
    public ITileObject GetChildTileObject() { return childTileObject; }
    public void ClearChildTileObject()
    {
        childTileObject = null;
    }
    public bool HasChild() { return childTileObject != null; }
    public Vector3 GetTileObjectLocalPosition() { return tileObjectPoint.transform.localPosition; }

}
