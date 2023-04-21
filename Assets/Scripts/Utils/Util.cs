using System;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Rect RectFromCenterSize(Vector2 center, Vector2 size)
    {
        float w = Mathf.Abs(size.x) / 2;
        float h = Mathf.Abs(size.y) / 2;

        float xMin = center.x - w;
        float yMin = center.y - h;
        float xMax = center.x + w;
        float yMax = center.y + h;

        return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
    }

    public static IEnumerator<object> TimedRoutine(float time, Func<object> during, Action end)
    {
        float start = Time.time;
        Func<bool> nottimedout = () => Time.time - start < time;
        return WhileRoutine(nottimedout, during, end);
    }

    // Continues till timedout and the condition is satisfied
    public static IEnumerator<object> TimedWhileRoutine(float time, Func<bool> condition, Func<object> during, Action end)
    {
        float start = Time.time;
        Func<bool> shouldEnd = () => (Time.time - start < time) || condition();
        return WhileRoutine(shouldEnd, during, end);
    }


    public static IEnumerator<object> TillNullRoutine(Func<object> during, Action atEnd)
    {
        object x = during();
        while (x != null)
        {
            yield return x;
            x = during();
        }
        atEnd();
    }

    public static IEnumerator<object> WhileRoutine(Func<bool> condition, Func<object> during, Action atEnd)
    {
        while (condition())
        {
            var x = during();
            yield return x != null ? x : new WaitUntil(() => !condition());
        }
        atEnd();
    }

    public static IEnumerator<object> UntilRoutine(Func<bool> condition, Func<object> during, Action atEnd)
    {
        return WhileRoutine(() => !condition(), during, atEnd);
    }
    public static T ChooseRandomElement<T>(T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public class RandomEnum
    {
        static System.Random _R = new System.Random();
        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_R.Next(v.Length));
        }
    }
}

