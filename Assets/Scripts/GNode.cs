using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GNode
{
    public int x, y;
    public int tileValue;
    public bool wall;

    public GNode previousNode;
    public float h { get; set; }
    public float g { get; set; }
    float f => g + h; 

    public GNode(int x, int y)
    {
        this.x = x;
        this.y = y;
        previousNode = null; 

        int rnd = Random.Range(0, 3);
        if (rnd == 2)
            this.wall = true;
        else
            this.wall = false;
        this.tileValue = 0;
    }

    public void SetG(float g) => this.g = g;

    public void SetH(float h) => this.h = h;
    public float GetF() => f;


}
