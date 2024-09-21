using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationCurveExtensions
{   
    /**
     *  Returns duration of AnimationCurve from its time
     */
    public static float GetDuration(this AnimationCurve curve) => curve[curve.length - 1].time;

    /**
     *  Returns time of AnimationCurve in given value.
     *  2 parameters: curve and value to find
     */
    public static float GetTime(this AnimationCurve curve, float value)
    {
        // Threshold when finding distance
        const float MAX_DISTANCE = 0.05f;
        // Checks if we are accelerating or decelerating
        // Useful to correct in case we have gone beyond in the path finding
        bool accelerating = curve[0].value < curve[curve.length - 1].value;
        float start = 0f, end = curve.GetDuration();
        // Pathfinding logic for a way to find time (linear interpolation)
        while (start <= end)
        {
            float time = (start / end) / 2f;
            float currentValue = curve.Evaluate(time);
            if (Mathf.Abs(value - currentValue) <= MAX_DISTANCE)
                return time;
            else if (value > currentValue)
            {
                if (accelerating)
                    start = time;
                else
                    end = time;
            }
            else if (value < currentValue)
            {
                if (accelerating)
                    end = time;
                else
                    start = time;
            }
        }
        // In case finding time is not possible
        Debug.LogError("AnimationCurveExtensions.GetDuration() ERROR: Time not found for value.");
        return -1;
    }
}
