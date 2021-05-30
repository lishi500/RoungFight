using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    protected GameObject self;
    protected List<GameObject> targets;
    //public CatCafe catCafe;
    public GameObject actionHolder;
    public ActionType type;

    public delegate void ActionEndEvent(Action action);
    public event ActionEndEvent notifyActionEnd;

    public abstract void StartAction();
    //public abstract void Interrupt();

    protected virtual void Awake() {
        //catCafe = GameManager.Instance.catCafe;
        actionHolder = GameManager.Instance.actionHolder;
    }
    
    public void Init(GameObject self, GameObject target) {
        this.self = self;
        this.targets = new List<GameObject>();
        this.targets.Add(target);
    }
    public void Init(GameObject self, List<GameObject> targets) {
        this.self = self;
        this.targets = targets;
    }

    public void ActionEnd() {
        if (notifyActionEnd != null) {
            notifyActionEnd(this);
        }

        Destroy(this);
    }

    public Cat GetCat() {
        return self.GetComponent<Cat>();
    }
}
