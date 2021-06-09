using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected GameObject self;
    protected List<GameObject> targets;
    protected Attribute actionAttr;
    protected Creature from {
        get { return self.GetComponent<Creature>(); }
    }
    protected Creature to {
        get { return targets[0].GetComponent<Creature>(); }
    }
    protected List<Creature> toS {
        get { return targets.Select(tar => tar.GetComponent<Creature>()).ToList(); }
    }

    public ActionType type;

    public delegate void ActionEndEvent(Action action);
    public event ActionEndEvent notifyActionEnd;

    public abstract void StartAction();
    //public abstract void Interrupt();
    public Action() {
        this.targets = new List<GameObject>();
    }
    public Action(GameObject self, GameObject target, Attribute actionAttr = null) {
        this.self = self;
        this.targets = new List<GameObject>();
        this.targets.Add(target);
        this.actionAttr = actionAttr;
    }
    public Action(GameObject self, List<GameObject> targets, Attribute actionAttr = null) {
        this.self = self;
        this.targets = targets;
        this.actionAttr = actionAttr;
    }

    public void ActionEnd() {
        if (notifyActionEnd != null) {
            notifyActionEnd(this);
        }
        //Destroy(this);
    }
}
