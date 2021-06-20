using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStartHandler : Singleton<DelayStartHandler>
{
    public void DelayAction(Action action, float delay) {
        StartCoroutine(DelayStart(action, delay));
    }

    private IEnumerator DelayStart(Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action.StartAction();
    }

}
