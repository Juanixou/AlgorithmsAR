using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacktrackingFlowController : MonoBehaviour
{
    //Public
    public int steps = 5;
    public GameObject[] sectionImages;

    //Private
    private Backtracking algthm;
    private StateData[] stateDataList;

    private int curStep = 0;
    private bool paused = false;
    private int k = 0;
    private VisualMazeController vslMazeCtrll;
    private bool isFinished;
    private List<ControlData> controlDataList;
    private List<ControlPhases> controlPhasesList;
    private int lastIteration = 0;
    private bool goBack = false;

    private bool automatic = true;
    private bool stopAlgortihm = true;

    //Variables to preStep
    private bool isBack = false;
    private int preIteration;

    //CONSTS
    const string SECTION = "sprt_step_";

    // Start is called before the first frame update
    void Start()
    {
        algthm= GetComponent<Backtracking>();
        vslMazeCtrll = GetComponent<VisualMazeController>();
        controlDataList = new List<ControlData>();
        controlPhasesList = new List<ControlPhases>();
        if (sectionImages.Length == 0) 
        {
            sectionImages = new GameObject[steps];

            for (int i = 0; i < steps; i++)
            {
                sectionImages[i] = GameObject.Find(SECTION + (i+1));
                sectionImages[i].SetActive(false);
            }
        }

        SolveMaze();
    }

    public void StartFlow()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (automatic && !stopAlgortihm)
        {
            automatic = false;
            StartCoroutine(WaitSeconds());
        }
        if (!paused)
        {
            paused = true;
            sectionImages[curStep].SetActive(true);
            DisableImages(curStep);
            ManageSteps();
        }
    }

    public void SolveMaze()
    {
        stateDataList = algthm.StartAlgorithm();
    }

    public void DisableImages(int step)
    {
        for(int i = 0; i < steps; i++)
        {
            if (i != step) sectionImages[i].SetActive(false);
        }
    }

    public void NextStep()
    {
        if (curStep == steps-1)
        {
            if (k == stateDataList.Length -1) return;
            curStep = 0;
            k++;
        }
        else
        {
            curStep++;
        }
        isBack = false;
        paused = false;
    }

    public void PreviousStep()
    {
        if(controlPhasesList.Count != 0)
        {
            isBack = true;
            k = controlPhasesList[controlPhasesList.Count - 1].iteration;
            curStep = controlPhasesList[controlPhasesList.Count - 1].step;
            Paint(stateDataList[k].x, stateDataList[k].y, Color.grey);
            if(preIteration != k) Paint(stateDataList[preIteration].x, stateDataList[preIteration].y, Color.grey);
            paused = false;

        }
    }

    public void Automatic()
    {
        stopAlgortihm = !stopAlgortihm;
    }

    public void ManageSteps()
    {
        switch (curStep)
        {
            case 0:
                Paint(stateDataList[k].x, stateDataList[k].y, Color.yellow);

                if (!isBack)
                {
                    controlDataList.Add(new ControlData(curStep, k));
                    controlPhasesList.Add(new ControlPhases(k, curStep));
                    preIteration = k;
                }
                else
                {
                    preIteration = k;
                    controlPhasesList.RemoveAt(controlPhasesList.Count - 1);
                }
                if (stateDataList[k].isEnd)
                {
                    isFinished = true;
                    Paint(stateDataList[k].x, stateDataList[k].y, Color.green);
                }
                break;
            case 1:

                if (!isBack)
                {
                    controlPhasesList.Add(new ControlPhases(k, curStep));
                    preIteration = k;
                }
                else
                {
                    preIteration = k;
                    controlPhasesList.RemoveAt(controlPhasesList.Count - 1);
                }

                if (stateDataList[k].isWall)
                {
                    k = stateDataList[k].prevId;
                    curStep = controlDataList[k].currentStep;
                }
                break;
            case 2:

                if (!isBack)
                {
                    controlPhasesList.Add(new ControlPhases(k, curStep));
                    preIteration = k;
                }
                else
                {
                    preIteration = k;
                    controlPhasesList.RemoveAt(controlPhasesList.Count - 1);
                }

                //Move Vertical
                if (isFinished) return;
                Paint(stateDataList[k].x, stateDataList[k].y, Color.cyan);
                controlDataList[k].currentStep = curStep;
                curStep = -1;
                k = stateDataList[k].GetPostIds()[0];
                break;
            case 3:

                if (!isBack)
                {
                    controlPhasesList.Add(new ControlPhases(k, curStep));
                    preIteration = k;
                }
                else
                {
                    preIteration = k;
                    controlPhasesList.RemoveAt(controlPhasesList.Count - 1);
                }

                //Move Horizontal
                if (isFinished) return;
                Paint(stateDataList[k].x, stateDataList[k].y, Color.cyan);
                controlDataList[k].currentStep = curStep;
                curStep = -1;
                k = stateDataList[k].GetPostIds()[1];
                break;
            case 4:

                if (!isBack)
                {
                    controlPhasesList.Add(new ControlPhases(k, curStep));
                }
                else
                {
                    preIteration = k;
                    controlPhasesList.RemoveAt(controlPhasesList.Count - 1);
                }

                if (!isFinished)
                {
                    if (!stateDataList[k].isValid)
                    {
                        Paint(stateDataList[k].x, stateDataList[k].y, Color.red);
                        k = stateDataList[k].prevId;
                        curStep = controlDataList[k].currentStep;
                    }
                }
                break;
        }
    }


    public void Paint(int x, int y, Color color)
    {
        if (stateDataList[k].isWall) return;
        vslMazeCtrll.SetColor(x, y, color);
    }


    public void JumpToIteration()
    {
        curStep = -1;
        k++;
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(0.5f);
        NextStep();
        automatic = true;
        
    }

}

public class ControlData{

    public int currentStep;
    public int currentIteration;
    public ControlData(){}

    public ControlData(int currentStep, int currentIteration)
    {
        this.currentStep = currentStep;
        this.currentIteration = currentIteration;
    }

    public int CurrentStep { get; set; }
    public int TryRight { get; set; }

}

public class ControlPhases
{
    public int iteration;
    public int step;

    public ControlPhases(int iteration, int step)
    {
        this.iteration = iteration;
        this.step = step;
    }

    public int Iteration { get; set; }
    public int Step { get; set; }

}
