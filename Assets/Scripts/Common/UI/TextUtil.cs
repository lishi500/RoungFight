using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUtil : Singleton<TextUtil>
{
    public void SetFollowText(GameObject obj, string text) {
        TextFollow textFollow = obj.GetComponent<TextFollow>();
        if (textFollow != null) {
            textFollow.SetText(text);
        }
    }

    public void ChangeTextAlpha(Text text, float a) {
        Color temp = text.color;
        temp.a = a;
        text.color = temp;
    }
}
