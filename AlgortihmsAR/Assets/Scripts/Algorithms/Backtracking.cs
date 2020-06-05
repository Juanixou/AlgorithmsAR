using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Backtracking : MonoBehaviour
{

    int N;
    private GameObject[,] cubesPositions;
    int[,] maze;

    // Start is called before the first frame update
    void Start()
    {
        maze = new int[,]{ 
            { 1, 0, 0, 0 },
            { 1, 1, 0, 1 },
            { 0, 1, 0, 0 },
            { 1, 1, 1, 1 } 
        };

        N = 4;
        cubesPositions = new GameObject[N, N];

        DrawLabyrinth(maze);



    }


    public void ShowLabyrinth()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                cubesPositions[i, j].SetActive(true);
            }
        }
    }

    public void HideLabyrinth()
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                cubesPositions[i, j].SetActive(false);
            }
        }
    }

    public void SolveLabyrinth()
    {
        SolveMaze(maze);
    }

    public void PrintSolution(int[,] sol)
    {
        string row = "";
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                row += sol[i, j];
            }

            row += "\n";

        }
        Debug.Log(row);
    }


    public bool IsSafePosition(int[,] maze, int x, int y)
    {
    return (x >= 0 && x < N && y >= 0
                && y < N && maze[x,y] == 1);
    }

    public bool SolveMaze(int[,] maze)
    {
        int[,] sol = new int[N,N];

        if(SolveMazeUtil(maze, 0, 0, sol) == false)
        {
            Debug.Log("No solution");
            return false;
        }
        PrintSolution(sol);
        return true;
    }

    public bool SolveMazeUtil(int[,] maze, int x, int y, int[,] sol)
    {

        //If is goal
        if (x == N - 1 && y == N - 1 && maze[x, y] == 1)
        {
            sol[x, y] = 1;
            cubesPositions[x, y].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
            return true;
        }

        if(IsSafePosition(maze,x,y))
        {
            sol[x, y] = 1;

            /* Move forward in x direction */
            if (SolveMazeUtil(maze, x + 1, y, sol))
            {
                cubesPositions[x, y].GetComponent<Renderer>().material.SetColor("_Color",Color.green);
                return true;
            }


            /* If moving in x direction doesn't give  
            solution then Move down in y direction */
            if (SolveMazeUtil(maze, x, y + 1, sol))
            {
                cubesPositions[x, y].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                return true;
            }

            /* If none of the above movements works then  
            BACKTRACK: unmark x, y as part of solution  
            path */
            sol[x,y] = 0;
            cubesPositions[x, y].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            return false;
        }

        return false;

    }

    public void DrawLabyrinth(int[,] maze)
    {

        GameObject target = GameObject.Find("Target");

        Vector2 imageSize = target.GetComponent<ImageTargetBehaviour>().GetSize();
        Debug.Log("Image Size: " + imageSize);
        float xScale = imageSize.x / N;
        float yScale = 50;
        float zScale = imageSize.y / N;

        float xPosition = ((imageSize.x / 2) - (xScale / 2)) * -1;
        Vector3 imagePosition = new Vector3(xPosition, yScale/2, ((imageSize.y / 2) - (zScale / 2)));
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                GameObject cube = Instantiate((GameObject)Resources.Load("Prefabs/Cube", typeof(GameObject)));
                if (maze[i,j] == 1)
                {
                    yScale = 10;
                }
                else
                {
                    cube.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                    yScale = 50;
                }
                
                cube.transform.SetParent(target.transform);
                cube.transform.localScale = new Vector3(xScale, yScale, zScale);
                cube.transform.position = new Vector3(imagePosition.x, yScale/2,imagePosition.z);
                imagePosition.Set(imagePosition.x + xScale, imagePosition.y, imagePosition.z);
                //Almacenamos cubos
                cubesPositions[i, j] = cube;
                cube.SetActive(false);
            }
            imagePosition.Set(xPosition, imagePosition.y, imagePosition.z - zScale);

        }


    }

}
