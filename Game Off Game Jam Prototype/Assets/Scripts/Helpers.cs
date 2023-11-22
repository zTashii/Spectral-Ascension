using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    //public static float Map(float from, float to, float from2, float to2, float value)
    //{
    //    if (value <= from2)
    //    {
    //        return from;
    //    }
    //    else if (value >= to2)
    //    {
    //        return to;
    //    }
    //    else
    //    {
    //        return (to - from) * ((value - from2) / (to2 - from2)) + from;
    //    }
    //}
    public static float Map(float value, float min1, float max1, float min2, float max2)
    { //map value from one range to another
        return min2 + (max2 - min2) * ((value - min1) / (max1 - min1)); //second min + delta2 * (distance between value and first / delta2)
    }

}