using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CordinatesUI : MonoBehaviour
{
    public static CordinatesUI Instance { get; private set; } //Singleton Pattern for ease of use

    [SerializeField] private TextMeshProUGUI coordinatesText;
    private void Awake()
    {
        Instance = this;
        Hide();
    }
    public void ChangeCoordinatesText(Vector2 coordinates, int index)
    {
        coordinatesText.text = $"{coordinates.x} , {coordinates.y}  ||   {index}";
    }
    public void Show()
    {
        coordinatesText.gameObject.SetActive(true);
    }
    public void Hide()
    {
        coordinatesText.gameObject.SetActive(false);
    }
}
