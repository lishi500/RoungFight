using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public GameObject centerObj;
    public Vector3 centerPos;
    public bool isFollow;

    public bool m_isStarted;
    public float speed = 1f; // how many second to move 360 degrees
    public float distance = 1f;
    public bool isForever;

    public float startAngle;
    public float endAngle;
    private float currentAngle;

    public void SetUp(GameObject centerObj, float speed = 1f, bool isFollow = true) {
        this.centerObj = centerObj;
        this.speed = speed;
        this.isFollow = isFollow;
        if (!isFollow) {
            centerPos = centerObj.transform.position;
        }
    }
    public void SetUp(Vector3 centerPos, float speed) {
        this.centerPos = centerPos;
        this.speed = speed;
    }


    public void StartCycling() {
        m_isStarted = true;
        currentAngle = startAngle;
    }
    public void StopCycling() {
        m_isStarted = false;
    }

    private void MoveToNextAngle() {
        float moveAngle = Time.deltaTime * (360f / speed);
        currentAngle = (currentAngle + moveAngle) % 360f;
    }

    private Vector3 CalculatePosition(Vector3 center) {
        //Quaternion q = Quaternion.AngleAxis(currentAngle, Vector3.up);
        //return GetCenterPos() + (q * Vector3.right * distance);
        float r = currentAngle * Mathf.PI / 180f;
        float x = Mathf.Sin(r) * distance;
        float y = Mathf.Cos(r) * distance;
        Vector3 move = new Vector3(x, y, 0);
        return center + move;
    }

    private Vector3 GetCenterPos() {
        if (isFollow) {
            return centerObj.transform.position;
        }
        return centerPos;
    }
   
    // Update is called once per frame
    void Update()
    {
        if (m_isStarted) {
            Vector3 nextPosition = CalculatePosition(GetCenterPos());
            transform.position = nextPosition;
            MoveToNextAngle();
        }
    }
}
