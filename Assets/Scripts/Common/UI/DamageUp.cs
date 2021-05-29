using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageUp : MonoBehaviour
{
    public bool isAvailable = true;

    //private DamageDef m_damageDef;
    //private GameObject targetObj;
    //private float timePast;
    private Vector3 initialPosition;
    private UIToWorld uiToWorld;
    private Text text;


    private const float MoveUpTime = 1f;
    private const float FadeOffStartPercent = 0.7f; 

    public void SetDamageUp(GameObject obj, DamageDef damageDef) {
        gameObject.SetActive(true);
        isAvailable = false;
        FormText(damageDef);
        TextUtil.Instance.ChangeTextAlpha(text, 1);

        initialPosition = obj.transform.position;
        uiToWorld = GetComponent<UIToWorld>();
        uiToWorld.followObj = obj;
        uiToWorld.isAboveSprite = true;
        uiToWorld.isKeepUpdating = false;
        uiToWorld.yOffset = 0;
        uiToWorld.MoveToPosition(initialPosition);

        StartCoroutine(MoveUp());
    }

    private IEnumerator MoveUp() {
        float targetYOffset = 40;
        float currentYOffset = 0;
        bool reachTarget = false;
        float elapsedTime = 0f;
        float percentage = 0;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3 startPosition = rectTransform.localPosition;
        float targetTextAlpha = 0;
        float startTextAlpha = text.color.a;
        float alphaElapsedTime = 0;

        while (!reachTarget) {
            if (Mathf.Abs(targetYOffset - currentYOffset) <= 0.1f) {
                reachTarget = true;
                Disable();
                break;
            }

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp(elapsedTime / MoveUpTime, 0f, 1f);
            t = CommonUtil.Instance.Smoothstep(t);
            percentage = t;
            currentYOffset = Mathf.Lerp(0, targetYOffset, t);

            Vector3 tempPosition = startPosition;
            tempPosition.y += currentYOffset;
            rectTransform.localPosition = tempPosition;


            if (percentage > FadeOffStartPercent) {
                alphaElapsedTime += Time.deltaTime;
                float alphaT = Mathf.Clamp(alphaElapsedTime / MoveUpTime * FadeOffStartPercent, 0f, 1f);
                alphaT = CommonUtil.Instance.EaseOut(alphaT);
                float a = Mathf.Lerp(startTextAlpha, targetTextAlpha, alphaT);

                TextUtil.Instance.ChangeTextAlpha(text, a);
            }

            yield return null;
        }
    }


    private void Disable() {
        gameObject.SetActive(false);
        isAvailable = true;
        // notify pool to recycle;
    }

    private void FormText(DamageDef damageDef) {
        text = GetComponent<Text>();
        Color damageColor = CommonUtil.Instance.ConvertColor(243, 52, 11, 1);
        Color healColor = CommonUtil.Instance.ConvertColor(40, 243, 10, 1);
        Color shieldColor = CommonUtil.Instance.ConvertColor(104, 189, 236, 1);

        if (damageDef.isCritical) {
            text.fontSize = 28;
        } else {
            text.fontSize = 18;
        }

        if (damageDef.type == DamageType.HEAL) {
            text.color = healColor;
            text.text = "+" + damageDef.damage;
        } else if (damageDef.type == DamageType.SHIELD) {
            text.color = shieldColor;
            text.text = "+" + damageDef.damage;
        } else {
            text.color = damageColor;
            text.text = "-" + damageDef.damage;
        }
    }

   
}
