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

    public static void DrawRect(Rect r)
    {
        Vector2[] verticies =
        {
            new Vector2(r.xMin, r.yMin),
            new Vector2(r.xMin, r.yMax),
            new Vector2(r.xMax, r.yMax),
            new Vector2(r.xMax, r.yMin)
        };
        for (int i = 0; i < verticies.Length; i++)
        {
            Debug.DrawLine(verticies[i % 4], verticies[(i + 1) % 4], Color.blue, 1000, false);
        }
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
}

