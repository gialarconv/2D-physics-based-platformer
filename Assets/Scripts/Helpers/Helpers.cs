using UnityEngine;
using System;
using System.Collections;

public static class Helpers
{
    public static WW.Waiters.Waiter DoAfterFrames(this MonoBehaviour me, int count, Action a)
    {
        return WW.Waiters.WaitController.DoAfterFrames(count, a).SetID(me);
    }
    public static WW.Waiters.Waiter DoAfter(this MonoBehaviour me, Func<bool> inCompletionCheck, Action a)
    {
        return WW.Waiters.WaitController.DoAfter(inCompletionCheck, a).SetID(me);
    }
}