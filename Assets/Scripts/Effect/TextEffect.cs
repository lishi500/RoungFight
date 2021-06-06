using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    BaseEffect baseEffect;
    Text uiText;
    Transform targetTransform;
    public Canvas textCanvas;

    public void SetText(string text) {
        uiText.text = text;
    }

    private void Start() {

    }
}
