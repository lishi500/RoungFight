using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Cat testCat;
    public GameObject actionHolder;
    public Transform centerFightPoint;
    private int m_skillSeq = 0;
    public int skillSeq {
        get { return m_skillSeq++; }
    }

    protected override void Awake() {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void TestFunction() {
       
    }

    int delayAction = 5;
    float delayTimer = 0;
    bool isDelayActionTriggered = false;
    void Update() {
        delayTimer += Time.deltaTime;
        if (delayTimer >= delayAction && !isDelayActionTriggered) {
            isDelayActionTriggered = true;
            TestFunction();
        }
    }
}
