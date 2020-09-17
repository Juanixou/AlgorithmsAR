using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullStateData
{

    public bool isValid = false;
    public bool isWall = false;
    public bool isEnd = false;
    public int k = 0;
    public int x = 0;
    public int y = 0;
    public OrderPosition[] postPos;
    public int numIterations = 0;
    public int prevId = 0;

    public FullStateData(int x, int y, int k, int prevId)
    {
        this.x = x;
        this.y = y;
        this.k = k;
        this.prevId = prevId;
        this.postPos =  new OrderPosition[4];
    }


    public class OrderPosition
    {
        public int x = 0;
        public int y = 0;

        public OrderPosition()
        {
            x = 0;
            y = 0;
        }

        public void SetXY(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X { set; get; }
        public int Y { set; get; }

    }

}
