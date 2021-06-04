using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIToWorld : MonoBehaviour
{
    public GameObject followObj;
    public bool isAboveSprite;
    public bool isKeepUpdating;
    public float yOffset;

    private RectTransform rectTransform;
    private RectTransform canvasRect;
    private Vector2 uiOffset;
    private Canvas canvas;

    // Start is called before the first frame update
    void Awake()
    {
        this.rectTransform = GetComponent<RectTransform>();
        this.canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        // Calculate the screen offset
        this.uiOffset = new Vector2((float)canvasRect.sizeDelta.x / 2f, (float)canvasRect.sizeDelta.y / 2f);

        if (followObj != null) {
            MoveToPosition(followObj.transform.position);
        }
    }

    public void MoveToPosition(Vector3 position) {
        //Debug.Log("MoveToPosition" + position.ToString());
        if (isAboveSprite) {
            position.y = position.y + (GetSpriteHeight() / 2);
        }
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);
        Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvasRect.sizeDelta.x, ViewportPosition.y * canvasRect.sizeDelta.y);

        Vector2 movePosition = proportionalPosition - uiOffset;
        //movePosition.y += yOffset;

        // Set the position and remove the screen offset
        this.rectTransform.localPosition = movePosition;
    }

    private float GetSpriteHeight() {
        SpriteRenderer sprite = followObj.GetComponent<SpriteRenderer>();

        float height = 0;
        if (sprite != null) {
            height = sprite.bounds.size.y;
        }

        return height;
    }

    private void Update() {
        if (isKeepUpdating && followObj != null) {
            MoveToPosition(followObj.transform.position);
        }
    }
}
