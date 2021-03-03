using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataByStep
{

    public int step = -1;
    public int x = -1;
    public int y = -1;
    public int order = -1;
    public int postX = -1;
    public int postY = -1;
    public int k = -1;
    public bool isFinished = false;
    public bool isValid = true;
    public bool isWall = false;
    

    public DataByStep(int step, int x, int y, int order, int k)
    {
        this.step = step;
        this.x = x;
        this.y = y;
        this.order = order;
        this.k = k;
    }
    public DataByStep(int step, int x, int y, int order, int k, int postX, int postY, bool isFinished)
    {
        this.step = step;
        this.x = x;
        this.y = y;
        this.order = order;
        this.k = k;
        this.postX = postX;
        this.postY = postY;
        this.isFinished = isFinished;
    }

}
