using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml.XPath;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class Pathfinding
{
    private const float DIAGONAL_MOVE_COST = 1.4f;
    private const float STRAIGHT_MOVE_COST = 1.4f;

    [SerializeField] private Grid grid;

    List<GNode> searchList = new List<GNode>();
    List<GNode> processedList = new List<GNode>();

    public Pathfinding (Grid grid)
    {
        this.grid = grid;
    }

    public List<GNode> FindPath(GNode startNode, GNode targetNode)
    {
        searchList.Clear();
        processedList.Clear();
        searchList.Add(startNode);
        ResetGridValues();

        startNode.SetG(0);
        startNode.SetH(CalculateDistanceCost(startNode, targetNode));

        while(searchList.Count() > 0)
        {
            GNode current = GetLowestFNode();

            if (current == targetNode)
                return CalculatePath(targetNode); //found target
            searchList.Remove(current);
            processedList.Add(current);
            int i = 0;

            foreach (GNode neighbour in CheckNeighbourNodes(current))
            {
                i++;
                if (processedList.Contains(neighbour))
                {
                    continue;
                }
                if (neighbour.wall == true)
                {
                    processedList.Add(neighbour);
                    continue;
                }
                float totalG = current.g + CalculateDistanceCost(current, neighbour);
                //Debug.Log("total g : " + totalG);
                //Debug.Log("neighbour g: " + neighbour.g);
                //Debug.Log("node: " + neighbour.x + " " + neighbour.y + " has values: f " + neighbour.GetF() + " g " + neighbour.g + " h " + neighbour.h);
                if (totalG < neighbour.g)
                {
                    //Debug.Log("1 node: " + neighbour.x + " " + neighbour.y + " " + neighbour.GetF());
                    neighbour.previousNode = current;
                    neighbour.SetG(totalG);
                    neighbour.SetH(CalculateDistanceCost(neighbour, targetNode));
                    //maybe upfate F
                    //Debug.Log("2 node: " + neighbour.x + " " + neighbour.y + " " + neighbour.GetF());

                    if (!searchList.Contains(neighbour))
                    {
                        searchList.Add(neighbour);
                    }
                }
            }

        }

        //processedList.Clear();
        //searchList.Clear(); //clear list after target node found
        return null; 
    }

    private List<GNode> CalculatePath(GNode target)
    {
        List<GNode> path = new List<GNode>();
        path.Add(target);
        GNode current = target;
        while(current.previousNode != null)
        {
            path.Add(current.previousNode);
            current = current.previousNode;
        }
        path.Reverse();
        return path; 
    }

    private List<GNode> CheckNeighbourNodes(GNode current)
    {
        List<GNode> neighbourList = new List<GNode>();

        if(current.x > 0) //check left neighbours
        {
            neighbourList.Add(grid.gridArray[current.x - 1, current.y]); // left
            /*
            if (current.y > 0)
                neighbourList.Add(grid.gridArray[current.x - 1, current.y - 1]); //left down
            if (current.y < grid.height - 1)
                neighbourList.Add(grid.gridArray[current.x - 1, current.y + 1]); //left up
            */
        }
        if(current.x < grid.width - 1)
        {
            neighbourList.Add(grid.gridArray[current.x + 1, current.y]); // right
            /*
            if (current.y > 0)
                neighbourList.Add(grid.gridArray[current.x + 1, current.y - 1]); //right down
            if (current.y < grid.height - 1)
                neighbourList.Add(grid.gridArray[current.x + 1, current.y + 1]); //right up
            */
        }
        if (current.y > 0)
            neighbourList.Add(grid.gridArray[current.x, current.y - 1]); //down
        if (current.y < grid.height - 1)
            neighbourList.Add(grid.gridArray[current.x, current.y + 1]); //up

        return neighbourList;
    }

    private float CalculateDistanceCost(GNode a, GNode b)
    {
        int xd = Mathf.Abs(a.x - b.x);
        int yd = Mathf.Abs(a.y - b.y);
        int remainingd = Mathf.Abs(xd - yd);
        return DIAGONAL_MOVE_COST * Mathf.Min(xd, yd) + STRAIGHT_MOVE_COST * remainingd; 
    }

    private void ResetGridValues()
    {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                grid.gridArray[x, y].SetG(int.MaxValue);
                grid.gridArray[x, y].previousNode = null;
            }
        }
            
    }
    private GNode GetLowestFNode()
    {
        GNode lowFNode = searchList[0];
        for (int i = 1; i < searchList.Count; i++)
            if (lowFNode.GetF() > searchList[i].GetF())
                lowFNode = searchList[i];
        return lowFNode;
    }
}
