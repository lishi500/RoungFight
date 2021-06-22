using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    public GameObject self;
    public List<GameObject> targets;
    protected Attribute actionAttr;
    public Creature from {
        get { return self.GetComponent<Creature>(); }
    }
    public Creature to {
        get { return targets[0].GetComponent<Creature>(); }
    }
    public List<Creature> toS {
        get { return targets.Select(tar => tar.GetComponent<Creature>()).ToList(); }
    }

    public List<ActionType> types;
    private bool isPrepared;

    public delegate void ActionReadyEvent(Action action);
    public event ActionReadyEvent notifyActionReady;
    public delegate void ActionEndEvent(Action action);
    public event ActionEndEvent notifyActionEnd;

    protected abstract void OnPrepareAction();
    protected abstract void OnStartAction();

    public void PrepareAction() {
        isPrepared = true;
        OnPrepareAction();
        if (notifyActionReady != null) {
            notifyActionReady(this);
        }
    }
    public void StartAction() {
        if (!isPrepared) {
            PrepareAction();
        }

        OnStartAction();
    }

    public abstract List<ActionType> DefaultActionType();
    //public abstract void Interrupt();
    public Action() {
        this.targets = new List<GameObject>();
        this.types = new List<ActionType>();
        types.AddRange(DefaultActionType());
    }

    public Action(GameObject self, Attribute actionAttr = null) {
        this.self = self;
        this.targets = new List<GameObject>();
        this.actionAttr = actionAttr;
        this.types = new List<ActionType>();
        types.AddRange(DefaultActionType());

    }

    public Action(GameObject self, GameObject target, Attribute actionAttr = null) {
        this.self = self;
        this.targets = new List<GameObject>();
        this.targets.Add(target);
        this.actionAttr = actionAttr;
        this.types = new List<ActionType>();
        types.AddRange(DefaultActionType());


    }
    public Action(GameObject self, List<GameObject> targets, Attribute actionAttr = null) {
        this.self = self;
        this.targets = targets;
        this.actionAttr = actionAttr;
        this.types = new List<ActionType>();
        types.AddRange(DefaultActionType());
    }

    public void AddActionType(ActionType actionType) {
        types.Add(actionType);
    }

    public void ActionCheck(int num) {
        if (notifyActionEnd == null) {
            Debug.Log("No one listen !!! " + num);
            //GetInvocationList
            //notifyActionEnd.GetInvocationList();
        } else { 
            Debug.Log("Some one listen : " + num);

        }
    }

    public void ActionEnd() {
        if (notifyActionEnd != null) {
            notifyActionEnd(this);
        } else {
            Debug.Log("No one listen");
        }
        //Destroy(this);
    }
}
