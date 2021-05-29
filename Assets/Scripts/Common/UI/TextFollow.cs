using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFollow : MonoBehaviour
{
    GameObject textObj;
    TextMeshPro textMesh;
    public Color color;

    public void SetText(string text) {
        textMesh.text = text;
    }

    private void Awake() {
        GameObject textPrefab = CommonUtil.Instance.GetPrefabByName("TextPrefab");
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        float height = 0;
        if (sprite != null) {
            height = sprite.bounds.extents.y;
        }
        Vector3 textPos = new Vector3(transform.position.x, transform.position.y + height + 0.25f, 0);
        textObj = Instantiate(textPrefab, textPos, Quaternion.identity);
        textObj.transform.SetParent(transform);
        textMesh = textObj.GetComponent<TextMeshPro>();
        textMesh.color = color;
        SetText(gameObject.name);
    }

    private void SelfTextMesh() {
        textMesh = transform.gameObject.AddComponent<TextMeshPro>();
        textMesh.color = color;
        SetText(gameObject.name);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
