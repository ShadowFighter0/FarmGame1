using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasingFunctions
{
    public static void EaseOutSine(Transform transform, Vector3 target, float easing)
    {
        Vector3 d = target - transform.position;
        Vector3 offset = d * Mathf.Sin(easing * (Mathf.PI * 0.5f));

        transform.position += offset;
        if (offset.sqrMagnitude < 0.0000001f)
        {
            transform.position = target;
        }
    }

    public static void EaseOutSine(RectTransform rectTransform, Vector3 target, float easing)
    {
        Vector3 d = target - rectTransform.localPosition;
        Vector3 offset = d * Mathf.Sin(easing * (Mathf.PI * 0.5f));

        rectTransform.localPosition += offset;
        if (offset.sqrMagnitude < 0.0000001f)
        {
            rectTransform.localPosition = target;
        }
    }

    public static float EaseOutSine(float current, float target, float easing)
    {
        float d = target - current;
        float offset = d * Mathf.Sin(easing * (Mathf.PI * 0.5f));
        return offset + current;
    }
}
