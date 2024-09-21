using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New MotionCurve", menuName = "Create MotionCurve")]
public class MotionCurve : ScriptableObject
{
    // Animation curve variables
    public AnimationCurve 
        accelerationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1,1)),
        decelerationCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1,0));
    [SerializeField] AnimationCurve m_Timeline;

    // Updates m_Timeline when acceleration and deceleration curves are changed
    // This DOESN'T run at runtime, only when values are changed in a MotionCurve at the editor
    private void OnValidate()
    {
        m_Timeline = new AnimationCurve();

        // Acceleration
        foreach (var k in accelerationCurve.keys)
            m_Timeline.AddKey(k);

        // Deceleration
        var decelerationStartTime = m_Timeline.GetDuration() + 0.5f;
        var decelerationKeys = decelerationCurve.keys;
        for (var i = 0; i < decelerationKeys.Length; i++)
        {
            decelerationKeys[i].time += decelerationStartTime;
            m_Timeline.AddKey(decelerationKeys[i]);
        }
        
        // Fix m_Timeline curve going >1 after accelerationCurve
        AnimationUtility.SetKeyRightTangentMode(
            m_Timeline, 
            accelerationCurve.length - 1, 
            AnimationUtility.TangentMode.Constant
        );
    }
}

/**
 *  Handles the output from MotionCurve and makes the logic on when should the
 *  accelerationCurve and decelerationCurve be played.
 */
[System.Serializable]
public struct MotionController
{
    [SerializeField] private MotionCurve m_Curve;
    [SerializeField] private float m_speedMax;
    private float m_speedPercentage;
    public float speed => m_speedMax * m_speedPercentage;
    private float m_Time;
    private bool m_WasMoving;

    /**
     *  Acceleration and deceleration logic based on if the GameObject is moving or not.
     *  Requires a parameter isMoving of type bool.
     */
    public void Update(bool isInput, bool isMoving)
    {
        if (!isMoving && !isInput)
            return;
        AnimationCurve currentCurve = isInput ? m_Curve.accelerationCurve : m_Curve.decelerationCurve;
        // Make so transition between acceleration and deceleration is smooth even if
        // movement is stopped before accelerationCurve is finished (value != 1)
        if (isInput != m_WasMoving)
        {
            m_Time = currentCurve.GetTime(m_speedPercentage);
        }
        
        m_Time += Time.deltaTime;
        // Clamp is needed so we don't go further than m_Timeline's length
        m_Time = Mathf.Clamp(m_Time, 0f, currentCurve.GetDuration());
        m_speedPercentage = currentCurve.Evaluate(m_Time);
        
        m_WasMoving = isInput;
    }
}

