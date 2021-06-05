using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimationController : CustomAnimationController
{
    public override void UpdateAnimationByState()
    {
        if (animationState != previousState)
        {
            if (animationState == AnimationState.IDLE)
            {
                SetAllFalse();
            }
            else {
                SetBoolState(animationState);
            }
            previousState = animationState;
        }
    }
}
