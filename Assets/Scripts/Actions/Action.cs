using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected GameObject self;
    protected List<GameObject> targets;
    protected Creature from {
        get { return self.GetComponent<Creature>(); }
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
    public Action(GameObject self, GameObject target) {
        this.self = self;
        this.targets = new List<GameObject>();
        this.targets.Add(target);
    }
    public Action(GameObject self, List<GameObject> targets) {
        this.self = self;
        this.targets = targets;
    }

    public void ActionEnd() {
        if (notifyActionEnd != null) {
            notifyActionEnd(this);
        }
        ;
        //Destroy(this);
    }
}
