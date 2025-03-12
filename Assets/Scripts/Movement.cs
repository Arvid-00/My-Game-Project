using System.Net;
using Unity.Mathematics;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Experimental.GraphView;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject gridManager;

    [SerializeField] private float speed;
    //private MousePosition2D mousePos2D; 
    private Grid grid;
    private Pathfinding pathfinding;
    private Vector2 destinationPos;
    private Vector2 nextPos;
    int currentPathIndex = 0;

    private List<Vector2> currentPath = new List<Vector2>();

    private void Start()
    {
        grid = gridManager.GetComponent<GridGenerator>().grid;
        pathfinding = new Pathfinding(grid);

        PlacePlayer();

        

    }

    void PlacePlayer()
    {
        for (int x = 5; x < grid.width; x++)
        {
            for (int y = 5; y < grid.height; y++)
            {
                if (!grid.gridArray[x, y].wall)
                {
                    transform.position = new Vector3(x, y, 0);
                    return;
                }
            }
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //GetNewPath

            Vector3 mouseWorldPos = MousePosition2D.GetMouseWorldPos2D();
            destinationPos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            //Debug.Log(destinationPos);
            if (GetGridNode(destinationPos).wall)
                return;

            Debug.Log("starting node: " + new Vector2(transform.position.x, transform.position.y));
            Debug.Log("destination node: " + destinationPos);
            List<GNode> path = pathfinding.FindPath(GetGridNode(new Vector2(transform.position.x, transform.position.y)),GetGridNode(destinationPos));

            if(path != null)
            {
                Debug.Log("test5");
                currentPath.Clear();
                for(int i = 0; i < path.Count; i++)
                    currentPath.Add(new Vector2(path[i].x, path[i].y));
                currentPathIndex = 0;
                for (int i = 0; i < path.Count; i++)
                    Debug.Log("Node " + i + " : " + path[i].x + " " + path[i].y);
            }
            //Vector2 mouseWorldPos = get
            //pathfinding.FindPath(grid.gridArray[mouse])
        }

        HandlePlayerMovement();
    }

    private void HandlePlayerMovement()
    {
        if (currentPath.Count > 0) //there is a path
        {
            Vector2 nextPos = currentPath[currentPathIndex];
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), nextPos) < 0.1f)
            {
                currentPathIndex++;
                if(currentPathIndex >= currentPath.Count)
                {
                    currentPathIndex = 0;
                    currentPath.Clear();
                }

                //Vector2 nextPos = currentPath[currentPathIndex];
            }
            else
            {
                Debug.Log("test6");
                Vector3 dir = (new Vector3(nextPos.x, nextPos.y, 0) - transform.position).normalized;
                transform.position = transform.position + dir * speed * Time.deltaTime;
            }
        }
    }
    private GNode GetGridNode(Vector2 pos)
    {
        GNode node = grid.gridArray[(int)pos.x, (int)pos.y];
        //Debug.Log(node.x + " " + node.y);
        return grid.gridArray[(int)pos.x,(int)pos.y];
    }
}
