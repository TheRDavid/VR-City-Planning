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
    public string conditionType;
    [SerializeField]
    public Visualizer visualizer;
    [SerializeField]
    public string conditionValue;

    public Condition(string conditionName, ConditionType conditionType, string conditionValue, Visualizer visualizer)
    {
        this.conditionName = conditionName;
        this.conditionType = ConditionTypeNames[(int)conditionType];
        this.conditionValue = conditionValue;
        this.visualizer = visualizer;
    }

    public enum ConditionType
    {
        equals, notEquals, larger, largerEquals, smaller, smallerEquals
    }

    public static readonly string[] ConditionTypeNames = new string[]
    {
        "equals", "notequals", "larger", "largerequals", "smaller", "smallerequals"
    };

    public bool isFullfilled(object instance)
    {
        PropertyInfo propertyInfo = instance.GetType().GetProperty(conditionName);
        
        if (propertyInfo != null)
        {
            object oValue = propertyInfo.GetValue(instance);
            string value = oValue.ToString();

            if (float.TryParse(value, out float fValue) && float.TryParse(conditionValue, out float fConditionValue))
            {
                // yes, this if-else thing is gross, feel free to impress me with a better way :P
                if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.equals]))
                    return fConditionValue == fValue;
                else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.notEquals]))
                    return fConditionValue != fValue;
                else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.larger]))
                    return fConditionValue < fValue;
                else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.largerEquals]))
                    return fConditionValue <= fValue;
                else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.smaller]))
                    return fConditionValue > fValue;
                else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.smallerEquals]))
                    return fConditionValue >= fValue;
                else
                    ErrorHandler.instance.reportError("Unknown Condition Type: " + conditionType);
                return false;
            }

            if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.equals]))
                return conditionValue.CompareTo(value) == 0;
            else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.notEquals]))
                return conditionValue.CompareTo(value) != 0;
            else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.larger]))
                return conditionValue.CompareTo(value) < 0;
            else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.largerEquals]))
                return conditionValue.CompareTo(value) <= 0;
            else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.smaller]))
                return conditionValue.CompareTo(value) > 0;
            else if (conditionType.ToLower().Equals(ConditionTypeNames[(int)ConditionType.smallerEquals]))
                return conditionValue.CompareTo(value) >= 0;
            else
                ErrorHandler.instance.reportError("Unknown Condition Type: " + conditionType);
            return false;
        }
        return false;
    }
}