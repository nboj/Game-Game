using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PF_Grid {
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;

    public int[,] GridArray {
        get => gridArray;
    }

    public float CellSize {
        get => cellSize;
    }

    public PF_Grid(int width, int height, float cellSize) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) { }
        }
    } 

    private void CreateWorldText() { }
}