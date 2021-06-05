using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEventHelper : MonoBehaviour
{
    public delegate void AnimationEndDelegate();
    public event AnimationEndDelegate notifyAnimationEnd;
    public delegate void AnimationEventTrigger(string name);
    public event AnimationEventTrigger notifyAnimationEventTrigger;

    public void OnAnimationEventTrigger(string name) {
        if (notifyAnimationEventTrigger != null) {
            notifyAnimationEventTrigger(name);
        }
    }
    public void OnAnimationEnd()
    {
        if (notifyAnimationEnd != null) {
            notifyAnimationEnd();
        }
    }
}
