using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/**
 * Wrapper class, since jsonUtilities can't deal with raw lists.
 * Yes, it's silly, I know.
 */
[Serializable()]
public class ConditionList
{
    [SerializeField]
    public List<Condition> conditions = new List<Condition>();
}

[Serializable()]
public class Condition
{
    [SerializeField]
    public string conditionName;
    [SerializeField]
    public ConditionType conditionType;
    [SerializeField]
    public Visualizer visualizer;
    [SerializeField]
    public string conditionValue;

    public Condition(string conditionName, ConditionType conditionType, string conditionValue, Visualizer visualizer)
    {
        this.conditionName = conditionName;
        this.conditionType = conditionType;
        this.conditionValue = conditionValue;
        this.visualizer = visualizer;
    }

    public enum ConditionType
    {
        equals, notEquals, larger, largerEquals, smaller, smallerEquals
    };

    public bool isFullfilled(object instance)
    {
        PropertyInfo propertyInfo = instance.GetType().GetProperty(conditionName);
        
        if (propertyInfo != null)
        {
            object oValue = propertyInfo.GetValue(instance);
            string value = oValue.ToString();

            float fConditionValue, fValue;
            if(float.TryParse(value, out fValue) && float.TryParse(conditionValue, out fConditionValue))
            {
                switch (conditionType)
                {
                    case ConditionType.equals:
                        return fConditionValue == fValue;
                    case ConditionType.notEquals:
                        return fConditionValue != fValue;
                    case ConditionType.larger:
                        return fConditionValue < fValue;
                    case ConditionType.largerEquals:
                        return fConditionValue <= fValue;
                    case ConditionType.smaller:
                        return fConditionValue > fValue;
                    case ConditionType.smallerEquals:
                        return fConditionValue >= fValue;
                    default: Debug.Log("ERROR: Unknown Condition Type: " + conditionType); return false;
                }
            }

            switch (conditionType)
            {
                case ConditionType.equals:
                    return conditionValue.CompareTo(value) == 0;
                case ConditionType.notEquals:
                    return conditionValue.CompareTo(value) != 0;
                case ConditionType.larger:
                    return conditionValue.CompareTo(value) < 0;
                case ConditionType.largerEquals:
                    return conditionValue.CompareTo(value) <= 0;
                case ConditionType.smaller:
                    return conditionValue.CompareTo(value) > 0;
                case ConditionType.smallerEquals:
                    return conditionValue.CompareTo(value) >= 0;
                default: Debug.Log("ERROR: Unknown Condition Type: " + conditionType); return false;
            }
        }

        return false;
    }
   
}