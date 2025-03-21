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

    Vector3 startPosT, endPosT;

    

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
                    transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
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
                currentPath.Clear();

                //Default
                for(int i = 0; i < path.Count; i++)
                currentPath.Add(new Vector2(path[i].x, path[i].y));

                RaycastHit2D ray = Physics2D.Raycast(transform.position, new Vector3(path[path.Count - 1].x + 0.5f, path[path.Count - 1].y + 0.5f, 0) - transform.position);
                startPosT = transform.position;
                endPosT = new Vector3(path[path.Count - 1].x + 0.5f, path[path.Count - 1].y + 0.5f, 0);
                if (ray.collider.CompareTag("Obstacle"))
                    Debug.Log("collision");
                else
                    Debug.Log("no collision");

                    /*
                    //Any Angle
                    Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
                    Vector2 endPos;
                    for(int i = 0; i < path.Count; i++)
                    {
                        endPos = new Vector2(path[i].x, path[i].y);
                        RaycastHit2D ray = Physics2D.Raycast(startPos, endPos - startPos);
                        if (!ray.collider.CompareTag("Obstacle"))
                        {
                            currentPath.Add(new Vector2(path[i - 1].x, path[i - 1].y));
                            startPos = new Vector2(path[i - 1].x, path[i - 1].y);
                            Debug.DrawRay(transform.position, new Vector3(endPos.x, endPos.y, 0) - transform.position, Color.red);
                        }
                        else
                        {

                        }



                    }
                    */
                    currentPathIndex = 0;
                for (int i = 0; i < path.Count; i++)
                    Debug.Log("Node " + i + " : " + path[i].x + " " + path[i].y);
            }
            //Vector2 mouseWorldPos = get
            //pathfinding.FindPath(grid.gridArray[mouse])
        }

        Debug.DrawRay(startPosT, endPosT - startPosT);
        HandlePlayerMovement();
    }

    private void HandlePlayerMovement()
    {
        if (currentPath.Count > 0) //there is a path
        {
            Vector2 nextPos = currentPath[currentPathIndex];
            nextPos = new Vector2(nextPos.x + 0.5f, nextPos.y + 0.5f); //OFFSET
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
