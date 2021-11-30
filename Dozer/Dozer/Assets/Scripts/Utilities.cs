using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static int PosOrNeg(float x)
    {
        if (x > 0)
        {
            return 1;
        }
        else if (x < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
