using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml.XPath;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.Rendering;

public class Pathfinding
{
    private const float DIAGONAL_MOVE_COST = 1.4f;
    private const float STRAIGHT_MOVE_COST = 1.4f;

    [SerializeField] private Grid grid;

    List<GNode> searchList = new List<GNode>();
    List<GNode> processedList = new List<GNode>();

    public Pathfinding (int width, int height)
    {
        
    }

    public List<GNode> FindPath(GNode startNode, GNode targetNode)
    {
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

            foreach(GNode neighbour in CheckNeighbourNodes(current))
            {
                if (processedList.Contains(neighbour))
                    continue;
                float totalG = current.g + CalculateDistanceCost(current, neighbour);
                if(totalG < neighbour.g)
                {
                    neighbour.previousNode = current;
                    neighbour.SetG(totalG);
                    neighbour.SetH(CalculateDistanceCost(neighbour, targetNode));
                    //maybe update F value

                    if (!searchList.Contains(neighbour))
                    {
                        searchList.Add(neighbour);
                    }
                }
            }

        }

        searchList.Clear(); //clear list after target node found
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
            if (current.y > 0)
                neighbourList.Add(grid.gridArray[current.x - 1, current.y - 1]); //left down
            if (current.y < grid.height - 1)
                neighbourList.Add(grid.gridArray[current.x - 1, current.y + 1]); //left up
        }
        if(current.x < grid.width - 1)
        {
            neighbourList.Add(grid.gridArray[current.x + 1, current.y]); // right
            if (current.y > 0)
                neighbourList.Add(grid.gridArray[current.x + 1, current.y - 1]); //right down
            if (current.y < grid.height - 1)
                neighbourList.Add(grid.gridArray[current.x + 1, current.y + 1]); //right up
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
            for (int y = 0; y < grid.height; y++)
            {
                grid.gridArray[x, y].SetG(int.MaxValue);
                grid.gridArray[x, y].previousNode = null;
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
