using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBacktrackingFlowController : MonoBehaviour
{
    //Public
    public int steps = 5;
    public GameObject[] sectionImages;

    /*Start Private*/
    //Other Classes Controllers
    private FullBacktracking algthm;
    private FullStateData[] stateDataList;
    private VisualMazeController vslMazeCtrll;
    private VisualTrazeVariables vslTrazeCtrll;

    private int curStep = 0;
    private int curSubStep = 0;
    private bool paused = false;
    private int k = 0;
    private bool isFinished;

    //Data
    private List<ControlData> controlDataList;
    private List<ControlPhases> controlPhasesList;

    private bool automatic = true;
    private bool stopAlgortihm = true;
    private int curOrder = 0;

    //Variables to preStep
    private bool isBack = false;
    private int preIteration;

    /*End Private*/

    //CONSTS
    const string SECTION = "sprt_step_";

    //New Section for new way to read the trace
    int curStepData = 0;
    private DataByStep[] dataByStepList;


    void Start()
    {
        algthm = GetComponent<FullBacktracking>();
        vslMazeCtrll = GetComponent<VisualMazeController>();
        vslTrazeCtrll = GetComponent<VisualTrazeVariables>();
        controlDataList = new List<ControlData>();
        controlPhasesList = new List<ControlPhases>();
        if (sectionImages.Length == 0)
        {
            sectionImages = new GameObject[steps];

            for (int i = 0; i < steps; i++)
            {
                sectionImages[i] = GameObject.Find(SECTION + (i + 1));
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
            sectionImages[dataByStepList[curStepData].step].SetActive(true);
            DisableImages(dataByStepList[curStepData].step);
            //ManageSteps();
            ManageTrace();
        }
    }

    public void SolveMaze()
    {
        //stateDataList = algthm.StartAlgorithm();
        dataByStepList = algthm.StartAlgorithm();
    }

    public void ManageTrace()
    {
        switch (dataByStepList[curStepData].step)
        {
            case 0:
                Paint(dataByStepList[curStepData].x, dataByStepList[curStepData].y, Color.yellow);
                if(dataByStepList[curStepData].order != -1) vslTrazeCtrll.SetMovementDirectionValue(dataByStepList[curStepData].order);
                if(dataByStepList[curStepData].k != -1) vslTrazeCtrll.SetIterationLevel(dataByStepList[curStepData].k);
                //SetVariablesByStep

                break;
            case 1:
                Paint(dataByStepList[curStepData].x, dataByStepList[curStepData].y, Color.cyan);
                if (dataByStepList[curStepData].order != -1) vslTrazeCtrll.SetMovementDirectionValue(dataByStepList[curStepData].order);
                vslTrazeCtrll.SetCurrentMazePosition(dataByStepList[curStepData].x, dataByStepList[curStepData].y);
                break;
            case 2:
                if (dataByStepList[curStepData].isFinished)
                {
                    Paint(dataByStepList[curStepData].x, dataByStepList[curStepData].y, Color.green);
                }
                
                break;
            case 3:
                break;
            case 4:
                if (!dataByStepList[curStepData].isValid)
                {
                    Paint(dataByStepList[curStepData].x, dataByStepList[curStepData].y, Color.red);
                }
                vslTrazeCtrll.SetCurrentMazePosition(dataByStepList[curStepData].x, dataByStepList[curStepData].y);
                break;
            default:
                break;
        }
    }


    //Generic function used to control the flow step by step
    public void ManageSteps()
    {
        switch (curStep)
        {
            case 0:
                curOrder = -1;
                //SetVariablesByStep(0);
                break;
            case 1:
                //Row and Column INC
                Paint(stateDataList[k].x, stateDataList[k].y, Color.yellow);

                if (!isBack)
                {
                    controlDataList.Add(new ControlData(curStep, k));
                    controlPhasesList.Add(new ControlPhases(k, curStep));
                    curSubStep++;
                    curOrder++;
                    preIteration = k;
                }
                else
                {
                    preIteration = k;
                    controlPhasesList.RemoveAt(controlPhasesList.Count - 1);
                    curSubStep--;
                }
                break;
            case 2:
                //If is valid position, check if is the end
                Paint(stateDataList[k].x, stateDataList[k].y, Color.cyan);
                if (stateDataList[k].isEnd)
                {
                    isFinished = true;
                    Paint(stateDataList[k].x, stateDataList[k].y, Color.green);
                }
                break;
            case 3:
                //Recursive call. Check if we finish the maze
                
                break;
            case 4:
                //If the movement it's not posible, we go back to the original position.
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
            default:
                break;
        }
    }


    //Function used to continue with execution
    public void NextStep()
    {

        if (curStepData == dataByStepList.Length) return;

        curStepData++;
        isBack = false;
        paused = false;
    }

    //Function used to go back one step
    public void PreviousStep()
    {
        if (controlPhasesList.Count != 0)
        {
            isBack = true;
            k = controlPhasesList[controlPhasesList.Count - 1].iteration;
            curStep = controlPhasesList[controlPhasesList.Count - 1].step;
            Paint(stateDataList[k].x, stateDataList[k].y, Color.grey);
            if (preIteration != k) Paint(stateDataList[preIteration].x, stateDataList[preIteration].y, Color.grey);
            paused = false;

        }
    }

    public void JumpToIteration()
    {
        curStep = -1;
        k++;
    }


    //Used to activate automatic steps
    public void Automatic()
    {
        stopAlgortihm = !stopAlgortihm;
    }

    //Function used to disable visual steps
    public void DisableImages(int step)
    {
        for (int i = 0; i < steps; i++)
        {
            if (i != step) sectionImages[i].SetActive(false);
        }
    }

    //Generic function to paint visual maze
    public void Paint(int x, int y, Color color)
    {
        if (dataByStepList[curStepData].isWall) return;
        vslMazeCtrll.SetColor(x, y, color);
    }

    //Generic function for variables
    public void SetVariablesByStep(int order)
    {
        switch (curStep)
        {
            case 0:
                
                vslTrazeCtrll.SetFinishValue(false);
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    public bool CheckNextMovement()
    {
        if (stateDataList[k].postPos[curOrder].x == stateDataList[k+1].x && stateDataList[k].postPos[curOrder].y == stateDataList[k + 1].y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    //Coroutine used to controll the steps speed
    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(0.5f);
        NextStep();
        automatic = true;

    }

}
