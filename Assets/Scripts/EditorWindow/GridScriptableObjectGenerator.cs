using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class GridScriptableObjectGenerator : EditorWindow
{
    [MenuItem("Window/Grid Generator")]
    public static void ShowWindow()
    {
        GridScriptableObjectGenerator wnd = GetWindow<GridScriptableObjectGenerator>("Grid Generator");
        wnd.minSize = new Vector2(250, 300);
        wnd.maxSize = new Vector2(250, wnd.maxSize.y);
    }

    private bool[,] toggleStates = new bool[10, 10];
    string gridDataName;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 230, 300));
        for (int y = 0; y < 10; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                toggleStates[x, y] = EditorGUILayout.Toggle(toggleStates[x, y]);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(10, 230, 230, 300));
        gridDataName = GUILayout.TextField(gridDataName);


        if (GUILayout.Button("Generate Grid"))
        {

            GridDataSO newGridData = CreateInstance<GridDataSO>();
            List<Vector2> obstaclesVector = new List<Vector2>();

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (toggleStates[y, x])
                    {
                        //There is an obstacle here
                        obstaclesVector.Add(new Vector2(y, x));
                    }
                }
            }

            newGridData.obstacleCordinates = obstaclesVector;
            AssetDatabase.CreateAsset(newGridData, $"Assets/ScriptableObjects/{gridDataName}.asset");

        }
        GUILayout.EndArea();

    }
}
