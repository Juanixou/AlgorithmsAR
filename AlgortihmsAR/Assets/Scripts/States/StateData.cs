using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateData
{

    public int x=0;
    public int y=0;
    public bool isValid = false;
    public int k=0;
    public bool isEnd = false;
    public bool isWall = false;
    public int[] postIds = { -1, -1, -1, -1};
    public int prevId = 0;

    public StateData() { }

    public StateData(int x, int y, int k, int prevId)
    {
        this.x = x;
        this.y = y;
        this.k = k;
        this.prevId = prevId;
    }

    public int X { set; get; }
    public int Y { set; get; }
    public int K { set; get; }
    public bool IsValid { set; get; }
    public bool IsEnd { set; get; }
    public bool IsWall { set; get; }

    public void SetFirstId(int  k)
    {
        postIds[0] = k;
    }
    public void SetSecondId(int k)
    {
        postIds[1] = k;
    }
    public void SetThirdId(int k)
    {
        postIds[2] = k;
    }

    public void SetGeneralId(int pos, int k)
    {
        postIds[pos] = k;
    }

    public int[] GetPostIds() { return postIds; }

    public void SetIsValid(bool isValid)
    {
        this.isValid = isValid;
    }
}
