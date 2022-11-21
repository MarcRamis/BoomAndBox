using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator
{
    public enum Type { LINEAR, SIN, COS, QUADRATIC, SMOOTH, SMOOTHER }
    private readonly Type type;
    public enum State { MIN, MAX, GROWING, SHRINKING }
    public State state { get; private set; } = State.MIN;

    private readonly float duration = 0f;
    private float currentLerpTime = 0f;
    public float Value { get; private set; }
    public float Inverse { get { return 1f - Value; } }

    private readonly float threshold = 0.05f;
    public bool IsMax { get { return Value >= 1f - threshold; } }
    public bool IsMin { get { return Value <= threshold; } }
    public bool IsMaxPrecise { get { return Value >= 1f; } }
    public bool IsMinPrecise { get { return Value <= 0f; } }


    public Interpolator(float _duration, Type _type)
    {
        duration = _duration;
        type = _type;
    }

    public void Update(float delta)
    {
        if (state == State.MAX || state == State.MIN)
            return;
        if (state == State.SHRINKING)
            delta *= -1f;

        currentLerpTime += delta;
        float t = currentLerpTime / duration;

        if (t <= 0f)
        {
            currentLerpTime = 0f;
            t = 0f;
            state = State.MIN;
        }
        else if (t >= 1f)
        {
            currentLerpTime = duration;
            t = 1f;
            state = State.MAX;
        }
        switch (type)
        {
            case Type.LINEAR:
                break;
            case Type.SIN:
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
                break;
            case Type.COS:
                t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
                break;
            case Type.QUADRATIC:
                t = t * t;
                break;
            case Type.SMOOTH:
                t = t * t * (3f - 2f * t);
                break;
            case Type.SMOOTHER:
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                break;
            default:
                break;
        }
        Value = t;
    }
    public void ToMax()
    {
        state = state == State.MAX ? State.MAX : State.GROWING;
    }

    public void ToMin()
    {
        state = state == State.MIN ? State.MIN : State.SHRINKING;
    }

    public void ForceMax()
    {
        state = State.MAX;
        currentLerpTime = 1f;
    }

    public void ForceMin()
    {
        state = State.MIN;
        currentLerpTime = 0f;
        Value = 0f;
    }

    ///<summary> Update that performs a simple interpolation </summary>
    public void DefaultUpdate(float dt)
    {
        Update(dt);

        if (IsMin)
        {
            ToMax();
        }
        else if (IsMax)
        {
            ToMin();
        }
    }

    ///<summary> Update that performs a precise interpolation </summary>
    public void PreciseUpdate(float dt)
    {
        Update(dt);

        if (IsMinPrecise)
        {
            ToMax();
        }
        else if (IsMaxPrecise)
        {
            ToMin();
        }
    }
}