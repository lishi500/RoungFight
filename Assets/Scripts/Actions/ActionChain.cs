using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChain : MonoBehaviour
{
    LinkedList<Action> actionQueue;
    Action currentAction;

    public bool isExecuting {
        get { return m_isExecuting; }
    }
    public bool isStarted {
        get { return m_isStarted; }
    }

    private bool m_isExecuting = false;
    private bool m_isStarted = false;
    public float waitTimeIntervalBetweenActions = 0.3f;

    public delegate void ActionChainAddEvent(Action action);
    public event ActionChainAddEvent notifyAddAction;

    public delegate void ActionChainEndEvent();
    public event ActionChainEndEvent notifyActionChainEnd;

    public void StartActionChain() {
        m_isStarted = true;
    }
    public bool IsAllTaskCompleted() {
        return actionQueue.Count == 0;
    }

    public void AddAction(Action action) {
        actionQueue.AddLast(action);
        OnAddAction(action);
    }

    public void AddActionJumpQueue(Action action) {
        actionQueue.AddFirst(action);
        OnAddAction(action);
    }

    private void MoveToNextAction() {
        currentAction = actionQueue.First.Value;
        actionQueue.RemoveFirst();
    }

    private void ExecuteCurrentTask() {
        m_isExecuting = true;
        currentAction.StartAction();
    }

    private void ExecuteNextAction() {
        MoveToNextAction();
        currentAction.notifyActionEnd += ListenActionEnd;
        ExecuteCurrentTask();
    }

    private void ListenActionEnd(Action action) {
        if (action == currentAction) {
            currentAction.notifyActionEnd -= ListenActionEnd;
            StartCoroutine(WaitAndStartNextAction());
        }
    }

    private void OnAddAction(Action action) {
        if (notifyAddAction != null) {
            notifyAddAction(action);
        }
    }

    private IEnumerator WaitAndStartNextAction() {
        yield return new WaitForSeconds(waitTimeIntervalBetweenActions);
        m_isExecuting = false;
    }

    private void Awake() {
        actionQueue = new LinkedList<Action>();
    }

    public void Update() {
        if (m_isStarted && !m_isExecuting) {
            if (IsAllTaskCompleted()) {
                if (notifyActionChainEnd != null) {
                    notifyActionChainEnd();
                }
                //Destroy(this);
            } else {
                ExecuteNextAction();
            }
        }
    }
}
