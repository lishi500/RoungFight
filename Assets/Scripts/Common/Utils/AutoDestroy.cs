using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeToLive = 3.0f;
    public bool isComponentOnly = false;
    public Component component;
    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0.0f)
        {
            DestoryNow();
        }
    }

    public void DestoryNow() {
        if (isComponentOnly) {
            if (component != null) {
                Destroy(component);
            }
            Destroy(this);
        } else {
            Destroy(gameObject);
        }
    }
}
