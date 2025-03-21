using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] GameObject square1, square2;
    Grid grid; 
    void Start()
    {
        grid = new Grid(20, 20, 1);

        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                if (grid.gridArray[x, y].tileValue == 0)
                    Instantiate(square1, new Vector2((float)x + 0.5f, (float)y + 0.5f), Quaternion.identity);
                else
                    Instantiate(square2, new Vector2((float)x + 0.5f, (float)y + 0.5f), Quaternion.identity);
            }
                
    }
}
