using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtil : Singleton<CommonUtil>
{

    public float Flip(float x) {
        return 1 - x;
    }

    private float Square(float x) {
        return x * x;
    }
    public float EaseOut(float t) {
        return Flip(Square(Flip(t)));
    }

    public float EaseIn(float t) {
      return 1 - Mathf.Cos((t * Mathf.PI) / 2);
    }

// ease in and out
    public float Smoothstep(float t) {
        return t * t * (3 - 2 * t);
    }

    public GameObject FindPrafab(string prafabName, string subpath = "") {
        subpath = subpath != "" ? subpath + "/" : "";
        GameObject variableForPrefab = Resources.Load<GameObject>("Prefabs/" + subpath + prafabName);
        return variableForPrefab;
    }
    public GameObject GetPrefabByName(string name) {
        return (GameObject) Resources.Load("Prefabs/" + name);
    }

    public float Distance(GameObject a, GameObject b) {
        return Distance(a.transform, b.transform);
    }

    public float Distance(Transform a, Transform b) {
        return Vector3.Distance(a.position, b.position);
    }

    public Color ConvertColor(float r, float g, float b, float a = 1) {
        return new Color(r / 255f, g / 255f, b / 255f, a);
    }
}
