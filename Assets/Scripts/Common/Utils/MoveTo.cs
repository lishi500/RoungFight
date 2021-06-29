using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    // if toObj is null, then we use toPos
    public GameObject toObj;
    public Vector3 toPos;
    public Vector3 fromPos = new Vector3(0, 0, -10000);
    public float moveTime;
    public float stopDistance;
    private MoveFunc moveFunc;

    public bool isUsingFromPos;
    public bool isFollow;
    public bool isUIElement;
    public bool isCurve;
    //public bool isInverseCurve;

    private float m_elapsedTime;
    private Coroutine moveCoroutine;
    private bool m_isMoving;

    public delegate void DestinationArrivedEvent();
    public event DestinationArrivedEvent notifyDestinationArrived;

    public void SetUp(GameObject toObj, float moveTime, MoveFunc moveFunc, bool isFollow = false) {
        this.toObj = toObj;
        this.moveTime = moveTime;
        this.moveFunc = moveFunc;
        this.isFollow = isFollow;
        if (!isFollow) {
            toPos = toObj.transform.position;
        }
    }
    public void SetUp( Vector3 toPos, float moveTime, MoveFunc moveFunc) {
        this.toPos = toPos;
        this.moveTime = moveTime;
        this.moveFunc = moveFunc;
    }

    public void SetFromPos(Vector3 fromPos) {
        this.fromPos = fromPos;
    }

    public void StartMove() {
        if (!m_isMoving) {
            if (isUsingFromPos) { // thing about this if resume coroutine
                transform.position = fromPos;
            }
            moveCoroutine = StartCoroutine(Move());
        }
    }

    public void PauseMove() {
        if (m_isMoving) {
            m_isMoving = false;
            StopCoroutine(moveCoroutine);
        }
    }

    private IEnumerator Move() {
        Debug.Log("Start move");
        Vector3 startPos = transform.position;

        float elapsedTime = 0f;
        bool isReached = false;
        m_isMoving = true;

        while (!isReached) {
            if (elapsedTime >= moveTime) {
                isReached = true;
                m_isMoving = false;
                OnDestinationArrived();
            }
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp(elapsedTime / moveTime, 0f, 1f);
            t = TimeFunc(t);

            Vector3 endPos = TransformUtils.Instance.PositionWithStopDistance(startPos, GetToPos(), stopDistance);
            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }
    }

    private Vector3 GetToPos() {
        return isFollow ? toObj.transform.position : toPos;
    }

    private float TimeFunc(float t) {
        switch (moveFunc) {
            case MoveFunc.Even:
                return t;
            case MoveFunc.EaseIn:
                return CommonUtil.Instance.EaseIn(t);
            case MoveFunc.EaseOut:
                return CommonUtil.Instance.EaseOut(t);
            case MoveFunc.Smoothstep:
                return CommonUtil.Instance.Smoothstep(t);
            default:
                return t;
        }
    }

    public void OnDestinationArrived() {
        Debug.Log("arrived");
        if (notifyDestinationArrived != null) {
            notifyDestinationArrived();
        }
        Destroy(this);
    }
}
