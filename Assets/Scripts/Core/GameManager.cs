using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public Cat testCat;
    public GameObject actionHolder;
    public Transform centerFightPoint;
    private int m_skillSeq = 0;
    public int skillSeq {
        get { return m_skillSeq++; }
    }

    private int m_buffSeq = 100000;
    private Dictionary<int, BaseBuff> buffMap;
    protected void Awake() {
        Instance = this;
        buffMap = new Dictionary<int, BaseBuff>();
    }

    public int RegisterBuff(BaseBuff baseBuff) {
        m_buffSeq++;
        buffMap.Add(m_buffSeq, baseBuff);
        return m_buffSeq;
    }
    public void UnRegisterBuff(int id) {
        buffMap.Remove(id);
    }
    public BaseBuff GetBuffById(int id) {
        return buffMap[id];
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
