using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChain : MonoBehaviour
{
    LinkedList<ActionWithDelay> actionQueue;
    ActionWithDelay currentAction;

    public bool isExecuting {
        get { return m_isExecuting; }
    }
    public bool isStarted {
        get { return m_isStarted; }
    }

    private bool m_isExecuting = false;
    private bool m_isStarted = false;
    private const float waitTimeIntervalBetweenActions = 0.4f;

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

    public void AddAction(Action action, float delay = waitTimeIntervalBetweenActions) {
        ActionWithDelay actionWithDelay = new ActionWithDelay(action, waitTimeIntervalBetweenActions);
        actionQueue.AddLast(actionWithDelay);
        OnAddAction(action);
    }

    public void AddActionJumpQueue(Action action, float delay = waitTimeIntervalBetweenActions) {
        ActionWithDelay actionWithDelay = new ActionWithDelay(action, waitTimeIntervalBetweenActions);
        actionQueue.AddFirst(actionWithDelay);
        OnAddAction(action);
    }

    private void MoveToNextAction() {
        currentAction = actionQueue.First.Value;
        actionQueue.RemoveFirst();
    }

    private void ExecuteCurrentTask() {
        m_isExecuting = true;
        currentAction.action.StartAction();
    }

    private void ExecuteNextAction() {
        MoveToNextAction();
        currentAction.action.notifyActionEnd += ListenActionEnd;
        ExecuteCurrentTask();
    }

    private void ListenActionEnd(Action action) {
        Debug.Log("Listen action end " + action.GetType().ToString() + " current " + currentAction.action.GetType().ToString());
        if (action == currentAction.action) {
            currentAction.action.notifyActionEnd -= ListenActionEnd;
            if (!IsAllTaskCompleted()) {
                ActionWithDelay peekNextAction = actionQueue.First.Value;

                if (peekNextAction.delay == 0) {
                    m_isExecuting = false;
                } else {
                    StartCoroutine(WaitAndStartNextAction(peekNextAction.delay));
                }
            } else {
                m_isExecuting = false;
            }
        }
    }

    private void OnAddAction(Action action) {
        if (notifyAddAction != null) {
            notifyAddAction(action);
        }
    }

    private IEnumerator WaitAndStartNextAction(float waitingTime) {
        yield return new WaitForSeconds(waitingTime);
        m_isExecuting = false;
    }

    private void Awake() {
        actionQueue = new LinkedList<ActionWithDelay>();
    }

    public void Update() {
        if (m_isStarted && !m_isExecuting) {
            if (IsAllTaskCompleted()) {
                m_isStarted = false;
                if (notifyActionChainEnd != null) {
                    notifyActionChainEnd();
                }
               
            } else {
                ExecuteNextAction();
            }
        }
    }

    class ActionWithDelay {
        public Action action;
        public float delay;

        public ActionWithDelay(Action action, float delay) {
            this.action = action;
            this.delay = delay;
        }
    }
}
