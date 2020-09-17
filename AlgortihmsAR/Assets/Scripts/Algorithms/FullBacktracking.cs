using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBacktracking : MonoBehaviour
{
    int[] mov_fil, mov_col;
    int N;
    int[,] lab,maze;
    bool exito = false;
    private List<FullStateData> stateDataList;
    private int phase = -1;
    private VisualMazeController visualMazeController;

    // Start is called before the first frame update
    void Start()
    {
        FullStateData[] ar = StartAlgorithm();
    }

    // Update is called once per frame
    
    public FullStateData[] StartAlgorithm()
    {
        mov_col = new int[4];
        mov_fil = new int[4];
        mov_fil[0] = 1; mov_col[0] = 0;     /*SUR*/
        mov_fil[1] = 0; mov_col[1] = 1;     /*ESTE*/
        mov_fil[2] = 0; mov_col[2] = -1;    /*OESTE*/
        mov_fil[3] = -1; mov_col[3] = 0;     /*NORTE*/

        maze = new int[,]{
            { 1, 1, 1, 1, 1 },
            { 1, 1, 0, 0, 1 },
            { 1, 0, 0, 1, 1 },
            { 1, 1, 0, 1, 0 },
            { 1, 1, 0, 1, 1 }
        };

        N = maze.GetLength(1);
        lab = new int[N, N];
        stateDataList = new List<FullStateData>();
        Laberinto(1, 0, 0, phase);

        visualMazeController = this.GetComponent<VisualMazeController>();
        visualMazeController.InitializeData(maze, N);

        Debug.Log("Solucion");
        string solution = "";
        for (int i = 0; i < N; i++)
        {

            for (int j = 0; j < N; j++)
            {
                solution = solution + " " + lab[i, j];
            }
            solution += "\n";
        }
        Debug.Log(solution);

        Debug.Log("TAMAÑO LISTA: " + stateDataList.Count);

        return stateDataList.ToArray();
    }

    public void Laberinto(int k, int fil, int col, int prePhase)
    {
        phase++;
        //Save the current state data necessary for simulation
        stateDataList.Add(new FullStateData(fil, col, phase, prePhase));
        
        int orden = -1;
        //Start the loop to check four directions
        do
        {
            //We try to move in one way
            orden++;
            fil += mov_fil[orden];
            col += mov_col[orden];
            stateDataList[phase].postPos[orden] = new FullStateData.OrderPosition();
            stateDataList[phase].postPos[orden].SetXY(fil, col);
            stateDataList[phase].numIterations++;
            //Check if is a possible position
            if ((fil >= 0) && (fil < N) && (col >= 0) && (col < N))
            {
                if ((maze[fil, col] == 1) &&(lab[fil, col] == 0))
                {
                    lab[fil, col] = k;
                    
                    //Check if is the end or we continue with the algorythm
                    if (fil == N - 1 && col == N - 1)
                    {
                        exito = true;
                        stateDataList[phase].isEnd = true;
                    }
                    else
                    {
                        stateDataList[phase].isValid = true;
                        //stateDataList[phase].SetGeneralId(orden, phase+1);
                        stateDataList[phase].isWall = false;
                        Laberinto(k + 1, fil, col, phase);
                        if (!exito)
                        {
                            lab[fil, col] = 0;
                            stateDataList[phase].isValid = false;
                        }
                    }
                }

            }
            else
            {
                stateDataList[phase].isWall = true;
                stateDataList[phase].isValid = false;
            }
            //We go back to the previous position
            fil -= mov_fil[orden];
            col -= mov_col[orden];
            if(!exito || orden == 3)
            {
                stateDataList[phase].isValid = false;
            }

        } while (!exito && orden < 3);
        //lab[fil, col] = 0;
    }

}
