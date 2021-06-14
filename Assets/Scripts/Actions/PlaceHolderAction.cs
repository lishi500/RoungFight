using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Not suppose to be used in any other class except combine with IdleAction to achieve idle time
 */
public class PlaceHolderAction : Action
{
    public override void StartAction() {
        ActionEnd();
    }
}
