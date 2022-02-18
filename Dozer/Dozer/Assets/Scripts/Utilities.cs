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

    public static float AngleCalculator(Vector2 one)
    {
       return Mathf.Atan2(one.y,one.x) * Mathf.Rad2Deg;
    }
}
