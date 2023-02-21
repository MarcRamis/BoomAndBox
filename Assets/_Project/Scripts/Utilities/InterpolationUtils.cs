using UnityEngine;
using System;
using System.Collections;

public static class InterpolationUtils
{
    public static IEnumerator Interpolate(Transform objectToMove, Vector3 start, Vector3 end, float timeToTake, Action callback = null)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / timeToTake;
            objectToMove.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        // Llamamos la función de devolución de llamada (callback) si se proporcionó
        if (callback != null)
        {
            callback();
        }
    }
}