using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New MotionCurve", menuName = "Create MotionCurve")]
public class MotionCurve : ScriptableObject
{
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
        var decelerationStartTime = m_Timeline.keys[m_Timeline.keys.Length - 1].time + 0.5f;
        var decelerationKeys = decelerationCurve.keys;
        for (var i = 0; i < decelerationKeys.Length; i++)
        {
            decelerationKeys[i].time += decelerationStartTime;
            m_Timeline.AddKey(decelerationKeys[i]);
        }
        
        // Fix m_Timeline curve going >1 after accelerationCurve
        AnimationUtility.SetKeyRightTangentMode(m_Timeline, accelerationCurve.length - 1, AnimationUtility.TangentMode.Constant);
    }
}
