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

    public static IEnumerator<object> TimedRoutine(float time, Func<object> during)
    {
        float start = Time.time;
        Func<bool> timedout = () => Time.time - start >= time;
        while (!timedout())
        {
            var x = during();
            yield return x != null ? x : new WaitUntil(timedout);
        }

    }

    public static IEnumerator<object> TillNullRoutine(Func<object> during)
    {
        object x = during();
        while (x != null)
        {
            yield return x;
            x = during();
        }
    }

    public static IEnumerator<object> WhileRoutine(Func<bool> condition, Func<object> during)
    {
        while (condition())
        {
            var x = during();
            if (x == null) yield break;
            yield return x;
        }
    }

    public static IEnumerator<object> UntilRoutine(Func<bool> condition, Func<object> during)
    {
        return WhileRoutine(() => !condition(), during);
    }
}

