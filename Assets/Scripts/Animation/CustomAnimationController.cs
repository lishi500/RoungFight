using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomAnimationController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public SimpleEventHelper eventHelper;
    public AnimationState animationState;
    [HideInInspector]
    public AnimationState previousState;

    private List<ExceptionState> exceptionList;
    private Queue<ExceptionState> exceptionOnceQueue;

    private bool m_isBlocking;

    [HideInInspector]
    protected string[] animationStates = new string[]
    { "isWalking", "isRunning", "isEating", "isDead"};

    public Dictionary<AnimationState, string> stateMapping = new Dictionary<AnimationState, string>() {

        { AnimationState.NONE, "" },
        { AnimationState.IDLE, "" },
        { AnimationState.WALK, "isWalking" },
        { AnimationState.RUN, "isRunning" },
        { AnimationState.EAT, "isEating" },
        { AnimationState.DIE, "isDead" },
        { AnimationState.ATTACK, "isAttack" },
        { AnimationState.GET_HIT, "isGetHit" }
    };

    public abstract void UpdateAnimationByState();

    public void SetAllFalse() {
        SetBoolState("");
    }

    public void SetBoolState(AnimationState state, bool flag = true, bool isBlocking = false)
    {
        SetBoolState(GetStateMapping(state), flag, isBlocking);
    }

    public void SetBoolState(string stateName, bool flag = true, bool isBlocking = false)
    {
        if (!m_isBlocking) {
            if (isBlocking) { 
            
            }
            //Debug.Log("- SetBoolState -" + stateName);
            foreach (AnimatorControllerParameter param in animator.parameters) {
                if (param.type == AnimatorControllerParameterType.Bool) {
                    if (param.name == stateName) {
                        animator.SetBool(param.name, flag);
                    } else {
                        animator.SetBool(param.name, false);
                    }
                }
            }
        }
        
    }

    private void BlockAllState() {
        m_isBlocking = true;
        eventHelper.notifyAnimationEnd += UnblockAllState;
    }

    private void UnblockAllState() {
        m_isBlocking = false;
    }

    private void UpdateExceptionState()
    {
        if (exceptionList.Count > 0)
        {
            foreach (ExceptionState exceptionState in exceptionList)
            {
                //Debug.Log("exceptionState " + exceptionState.name + " - " + exceptionState.state);
                SetBool(exceptionState.name, exceptionState.state);
            }
        }
        while (exceptionOnceQueue.Count > 0)
        {
            ExceptionState exceptionState = exceptionOnceQueue.Dequeue();
            SetBool(exceptionState.name, exceptionState.state);
        }
    }
  
    //public void SetBool(AnimationState name, bool state)
    //{
    //    SetBool(GetStateMapping(name), state);
    //}

    private void SetBool(string name, bool state)
    {
        if (ContainsParam(animator, name))
        {
            animator.SetBool(name, state);
        }
    }


    public void AddAutoClearState(AnimationState name, bool state, bool isExclusive = true, float time = 0.2f)
    {
        //Debug.Log("AddAutoClearState " + name);
        if (isExclusive)
        {
            //animationState = AnimationState.NONE;
            SetBoolState(name);
        }
        else {
            AddExceptionOnce(name, state);
        }
       
        StartCoroutine(ClearState(name, !state, time));
    }

    public void AddExceptionOnce(AnimationState name, bool state) {
        AddExceptionOnce(GetStateMapping(name), state);
    }
    public void AddExceptionOnce(string name, bool state) {
        exceptionOnceQueue.Enqueue(new ExceptionState(name, state));
    }

    private IEnumerator ClearState(AnimationState name, bool state, float time)
    {
        yield return new WaitForSeconds(time);
        //Debug.Log("ClearState " + name + "  " + state);
        AddExceptionOnce(name, state);

        yield return null;
    }

    protected string GetStateMapping(AnimationState name)
    {
        string animationName;
        stateMapping.TryGetValue(name, out animationName);

        return animationName;
    }

    public void SetAnimatorSpeed(float speed)
    {
        animator.speed = speed;
    }

    public AnimationClip GetAnimationClipByName(string name)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == name)
            {
                return ac.animationClips[i];
            }
        }
        return null;
    }

    private void Awake()
    {
        animator = gameObject.transform.GetComponent<Animator>();
        eventHelper = GetComponent<SimpleEventHelper>();

        animationState = AnimationState.IDLE;
        previousState = AnimationState.NONE;
        exceptionList = new List<ExceptionState>();
        exceptionOnceQueue = new Queue<ExceptionState>();

        //AnimationClip attackClip = GetAnimationClipByName("attack");
        //if (attackClip != null)
        //{
        //    attackClipLength = attackClip.length;
        //}
    }
    public virtual void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationByState();
        UpdateExceptionState();
    }

    public static bool ContainsParam(Animator _Anim, string _ParamName)
    {
        foreach (AnimatorControllerParameter param in _Anim.parameters)
        {
            if (param.name == _ParamName) return true;
        }
        return false;
    }

    class ExceptionState
    {
        public string name;
        public bool state;
        public ExceptionState(string name, bool state)
        {
            this.name = name;
            this.state = state;
        }
    }
}
