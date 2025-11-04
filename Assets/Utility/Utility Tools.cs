using UnityEngine;

public static class UtilityTools
{
    public static float RandomVarianceFloat()
    {
        float variance = Random.Range(-0.1f, 0.1f);
        return variance;
    }

    public static float RandomVarianceFloat(float lower, float upper)
    {
        float variance = Random.Range(lower, upper);
        return variance;
    }

    public static int RandomVarianceInt(int lower, int upper)
    {
        int variance = Random.Range(lower, upper + 1); // BOTH INCLUSIVE
        return variance;
    }
}