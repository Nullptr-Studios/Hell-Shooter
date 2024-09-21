using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public class FieldChangesTracker
{
    Dictionary<string, string> lastValuesByFieldPath = new Dictionary<string, string>();


    public bool TrackFieldChanges<TOwner, TMember>(TOwner rootOwnerInstance, Expression<Func<TOwner, TMember>> fieldSelector)
    {
        // Get the field info path:
        var fieldInfoPath = GetMemberInfoPath(rootOwnerInstance, fieldSelector);
        if (fieldInfoPath.Count == 0)
        {
            Debug.LogError("No member info path could be retrieved");
            return false;
        }


        // Get the current field value, and its path as a string to use as key:
        FieldInfo fieldInfo = null;
        object targetObject = rootOwnerInstance;
        string fieldPath = null;

        for (int i = 0; i < fieldInfoPath.Count; i++)
        {
            if (fieldInfo != null)
                targetObject = fieldInfo.GetValue(targetObject);

            fieldInfo = fieldInfoPath[i] as FieldInfo;
            if (fieldInfo == null)
            {
                Debug.LogError("One of the members in the field path is not a field");
                return false;
            }

            if (i > 0)
                fieldPath += ".";
            fieldPath += fieldInfo.Name;
        }

        object currentValueObject = fieldInfo.GetValue(targetObject);
          

        // Get the current value as a string:
        string currentValueString = null;

        if (currentValueObject != null)
        {
            if (currentValueObject is UnityEngine.Object)
            {
                currentValueString = currentValueObject.ToString();
            }
            else
            {
                try
                {
                    currentValueString = JsonUtility.ToJson(currentValueObject);
                }
                catch (Exception)
                {
                    Debug.LogError("Couldn't get the current value with \"JsonUtility.ToJson\"");
                    return false;
                }

                if (string.IsNullOrEmpty(currentValueString)  ||  currentValueString == "{}")
                    currentValueString = currentValueObject.ToString();
            }
        }


        // Check if the value was changed, and store the current value:
        bool changed = lastValuesByFieldPath.TryGetValue(fieldPath, out string lastValue)  &&  lastValue != currentValueString;

        lastValuesByFieldPath[fieldPath] = currentValueString;

        return changed;
    }


    public static List<MemberInfo> GetMemberInfoPath<TOwner, TMember>(TOwner ownerInstance, Expression<Func<TOwner, TMember>> memberSelector)
    {
        Expression body = memberSelector;
        if (body is LambdaExpression lambdaExpression)
        {
            body = lambdaExpression.Body;
        }

        List<MemberInfo> membersInfo = new List<MemberInfo>();
        while (body is MemberExpression memberExpression)
        {
            membersInfo.Add(memberExpression.Member);
            body = memberExpression.Expression;
        }

        membersInfo.Reverse();
        return membersInfo;
    }
}