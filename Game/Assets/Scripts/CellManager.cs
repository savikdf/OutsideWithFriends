using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public Vector2 gridSize;
    GameObject[,] cellGrid;
    GameObject cellPrefab;

    private void Awake()
    {
        gridSize = gridSize == Vector2.zero ? Vector2.one * 10f : gridSize;
        //clamp the size vector to int values
        gridSize = new Vector2(Mathf.CeilToInt(gridSize.x), Mathf.CeilToInt(gridSize.y));
    }


    void Spawn() {
        cellGrid = new GameObject[(int)gridSize.x, (int)gridSize.y];
        for(int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {



            }
        }    
    }



}
