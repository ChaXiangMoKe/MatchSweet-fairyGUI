using System;
using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour {
    
    public static CoroutineManager Instance { get; private set; }

    public delegate bool Condition();

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public Coroutine WaitAction(Condition condition, Action action)
    {
        return StartCoroutine(DoWaitConditionAction(condition, action));
    }

    public Coroutine StarDelayFrameAction(Action action)
    {
        return StartCoroutine(DoWaitFrameAction(action));
    }

    public Coroutine StartDelaySecondAction(Action action, float second)
    {
        return StartCoroutine(DoWaitSecondAction(action, second));
    }
    private IEnumerator DoWaitConditionAction(Condition condition, Action action)
    {
        while (!condition.Invoke())
        {
            yield return 0;
        }
        action();
    }

    private IEnumerator DoWaitFrameAction(Action action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }

    private IEnumerator DoWaitSecondAction(Action action, float second)
    {
        yield return new WaitForSeconds(second);
        action();
    }
}
