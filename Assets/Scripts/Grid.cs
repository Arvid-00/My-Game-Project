using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Xml.Serialization;

public class Grid
{
    public int width;
    public int height;
    private float tileSize;
    public GNode[,] gridArray;

    public Grid(int width, int height, float tileSize)
    {
        this.width = width;
        this.height = height;
        this.tileSize = tileSize;

        gridArray = new GNode[width, height];

        GridSetup();
    }

    private void GridSetup()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                gridArray[x, y] = new GNode(x, y); 
    }
}

