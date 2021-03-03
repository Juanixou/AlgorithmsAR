using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualTrazeVariables : MonoBehaviour
{

    public TextMesh txt_k;
    public TextMesh txt_orden;
    public TextMesh txt_fil;
    public TextMesh txt_col;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMovementDirectionValue(int val)
    {
        txt_orden.text = val.ToString();
    }
    public void SetCurrentMazePosition(int x, int y)
    {
        txt_fil.text = x.ToString();
        txt_col.text = y.ToString();
    }

    public void SetFinishValue(bool isFinished)
    {

    }

    public void SetIterationLevel(int k)
    {
        txt_k.text = k.ToString();
    }

    public void SetAlgorithmStep(int phase)
    {

    }

}
