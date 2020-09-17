using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class Backtracking : MonoBehaviour
{

    int N;
    int[,] maze;
    private VisualMazeController visualMazeController;
    private List<StateData> stateDataList;
    private int k = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    public StateData[] StartAlgorithm()
    {
        //maze = new int[,]{
        //    { 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 },
        //    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        //    { 1, 1, 1, 0, 0, 0, 0, 0, 0, 1 },
        //    { 1, 1, 1, 0, 0, 0, 1, 1, 1, 1 },
        //    { 1, 1, 1, 0, 0, 0, 1, 1, 1, 1 },
        //    { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1 },
        //    { 1, 1, 1, 0, 0, 0, 0, 1, 1, 1 },
        //    { 1, 1, 1, 0, 1, 1, 0, 1, 1, 1 },
        //    { 1, 1, 1, 0, 1, 1, 0, 1, 1, 1 },
        //    { 1, 1, 1, 1, 1, 1, 0, 0, 0, 1 }
        //};

        maze = new int[,]{
            { 1, 1, 1, 1, 1 },
            { 1, 1, 0, 1, 1 },
            { 1, 0, 1, 0, 1 },
            { 1, 1, 1, 0, 1 },
            { 1, 1, 1, 0, 1 }
        };

        N = maze.GetLength(1);
        Debug.Log(N);
        stateDataList = new List<StateData>();
        visualMazeController = this.GetComponent<VisualMazeController>();
        visualMazeController.InitializeData(maze, N);
        SolveMaze(maze);
        return stateDataList.ToArray();
    }

    public bool SolveMaze(int[,] maze)
    {
        int[,] sol = new int[N, N];

        if (SolveMazeUtil(maze, 0, 0, sol,k) == false)
        {
            Debug.Log("No solution");
            return false;
        }
        return true;
    }

    public bool SolveMazeUtil(int[,] maze, int x, int y, int[,] sol, int prek)
    {
        int phase = k;
        Debug.Log(k);
        stateDataList.Add(new StateData(x, y, phase, prek));
        k++;

        //Comprueba si ha llegado al final
        if (x == N - 1 && y == N - 1 & maze[x, y] == 1)
        {
            sol[x, y] = 1;
            stateDataList[phase].isEnd = true;

            for(int i =0;i<N; i++)
            {
                string solution = "";
                for(int j = 0; j < N; j++)
                {
                    solution = solution + " " + sol[i, j];
                }
                Debug.Log(solution);
            }

            return true;
        }


        if (IsSafePosition(maze, x, y))
        {

            sol[x, y] = 1;
            stateDataList[phase].SetFirstId(k);
            
            /* Move forward in x direction */
            if(stateDataList[prek].x != x + 1)
            {
                if (SolveMazeUtil(maze, x + 1, y, sol, phase))
                {
                    stateDataList[phase].isValid = true;
                    return true;
                }
            }
            
            stateDataList[phase].SetSecondId(k);

            /* If moving in x direction doesn't give  
            solution then Move down in y direction */
            if (stateDataList[prek].y != y + 1)
            {
                if (SolveMazeUtil(maze, x, y + 1, sol, phase))
                {
                    stateDataList[phase].isValid = true;
                    return true;
                }
            }

            //if (stateDataList[prek].x != x - 1)
            //{
            //    if (SolveMazeUtil(maze, x - 1, y, sol, phase))
            //    {
            //        stateDataList[phase].isValid = true;
            //        return true;
            //    }
            //}

            //if (stateDataList[prek].y != y - 1)
            //{
            //    if (SolveMazeUtil(maze, x, y - 1, sol, phase))
            //    {
            //        stateDataList[phase].isValid = true;
            //        return true;
            //    }
            //}

            /* If none of the above movements works then  
            BACKTRACK: unmark x, y as part of solution  
            path */
            sol[x, y] = 0;
            stateDataList[phase].isValid = false;
            return false;
        }
        stateDataList[phase].isWall = true;
        stateDataList[phase].isValid = false;
        return false;

    }

    public bool IsSafePosition(int[,] maze, int x, int y)
    {
    return (x >= 0 && x < N && y >= 0 && y < N && maze[x,y] == 1);
    }


}
