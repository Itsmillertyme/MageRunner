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

    public static float RandomVarianceFloat(float lower, float upper, int precision)
    {
        if (precision < 0) precision = 0;
        
        float rounding = Mathf.Pow(10f, precision);
        float variance = Random.Range(lower, upper);

        return Mathf.Round(variance * rounding) / rounding;
    }

    public static int RandomVarianceInt(int lower, int upper)
    {
        int variance = Random.Range(lower, upper + 1); // BOTH INCLUSIVE
        return variance;
    }
}