using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GNode
{
    public int x, y;
    public int tileValue;

    public GNode previousNode;
    public float h { get; set; }
    public float g { get; set; }
    float f => g + h; 

    public GNode(int x, int y)
    {
        this.x = x;
        this.y = y;
        previousNode = null; 

        tileValue = Random.Range(0, 2);
    }

    public void SetG(float g) => g = g;

    public void SetH(float h) => h = h;
    public float GetF() => f;


}
