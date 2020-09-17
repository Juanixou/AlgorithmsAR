using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class VisualMazeController : MonoBehaviour
{

    private GameObject labObject;
    private GameObject[,] cubesPositions;
    int N;
    private FullBacktrackingFlowController BCKT_flow;

    private GameObject btn_play;
    private GameObject btn_pause;

    public void InitializeData(int[,] maze, int n)
    {
        N = n;
        labObject = GameObject.Find("Target").transform.Find("Lab").gameObject;
        cubesPositions = new GameObject[N, N];
        DrawLabyrinth(maze);
        ControlButtons();
        BCKT_flow = GetComponent<FullBacktrackingFlowController>();
    }

    public void ControlButtons()
    {
        GameObject.Find("btn_showLab").GetComponent<Button>().onClick.AddListener(ShowLabyrinth);
        GameObject.Find("btn_nextStep").GetComponent<Button>().onClick.AddListener(GoToNext);
        GameObject.Find("btn_prevStep").GetComponent<Button>().onClick.AddListener(GoToPrev);
        btn_play = GameObject.Find("btn_playAuto");
        btn_pause = GameObject.Find("btn_pauseAuto");
        btn_play.GetComponent<Button>().onClick.AddListener(Automatic);
        btn_pause.GetComponent<Button>().onClick.AddListener(Automatic);
        btn_pause.SetActive(false);
    }

    public void SolveLabyrinth()
    {
        BCKT_flow.StartFlow();
    }


    public void ShowLabyrinth()
    {
        labObject.SetActive(!labObject.activeSelf);
    }

    public void DrawLabyrinth(int[,] maze)
    {

        GameObject target = GameObject.Find("Target");

        Vector2 imageSize = target.GetComponent<ImageTargetBehaviour>().GetSize();
        float xScale = imageSize.x / N;
        float yScale = 50;
        float zScale = imageSize.y / N;

        float xPosition = ((imageSize.x / 2) - (xScale / 2)) * -1;
        Vector3 imagePosition = new Vector3(xPosition, yScale / 2, ((imageSize.y / 2) - (zScale / 2)));
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                GameObject cube = Instantiate((GameObject)Resources.Load("Prefabs/Cube", typeof(GameObject)));
                if (maze[i, j] == 1)
                {
                    yScale = 10;
                    cube.GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
                }
                else
                {
                    cube.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                    yScale = 50;
                }


                cube.transform.SetParent(labObject.transform);
                cube.transform.localScale = new Vector3(xScale, yScale, zScale);
                cube.transform.position = new Vector3(imagePosition.x, yScale / 2, imagePosition.z);
                imagePosition.Set(imagePosition.x + xScale, imagePosition.y, imagePosition.z);
                //Almacenamos cubos
                cubesPositions[i, j] = cube;
            }
            imagePosition.Set(xPosition, imagePosition.y, imagePosition.z - zScale);

        }


    }

    public void SetColor(int x, int y, Color color)
    {
        cubesPositions[x, y].GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    public void GoToNext()
    {
        BCKT_flow.NextStep();
    }
    public void GoToPrev()
    {
        BCKT_flow.PreviousStep();
    }

    public void Automatic()
    {
        btn_play.SetActive(!btn_play.activeSelf);
        btn_pause.SetActive(!btn_pause.activeSelf);
        BCKT_flow.Automatic();
    }

}
