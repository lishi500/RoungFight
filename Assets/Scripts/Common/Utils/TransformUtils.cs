using System.Collections;
using System.Linq;

using System.Collections.Generic;
using UnityEngine;

public class TransformUtils : Singleton<TransformUtils>
{
    public Vector3 RandonPointInRange(Vector3 originPoint, float randomange)
    {
        Vector3 randomPoint = originPoint + Random.insideUnitSphere * randomange;
        return new Vector3(randomPoint.x, transform.position.y, randomPoint.z);
    }

    public float DistanceBetweenVector(Vector3 from, Vector3 to) {
        return Vector3.Distance(from, to);
    }

    public float DistanceBetweenObject(GameObject obj1, GameObject obj2)
    {
        return Vector3.Distance(obj1.transform.position, obj2.transform.position);
    }

    public Vector3 PositionWithStopDistance(Vector3 from, Vector3 to, float stopDistance)
    {
        Vector3 direction = to - from;
        return from + (direction - (direction.normalized * stopDistance));
    }

    public IEnumerator ScaleChange(Transform obj, float fromScale, float toScale = 1f, float duration = 1f, float delay = 0)
    {
        Vector3 initialScale = obj.localScale * fromScale;
        Vector3 finalScale = obj.localScale * toScale;
        float startTime = Time.time;

        obj.localScale = initialScale;
        float progress = 0;
        yield return new WaitForSeconds(delay);


        while (progress <= duration)
        {
            obj.localScale = Vector3.Lerp(initialScale, finalScale, progress / duration);
            progress = Time.time - startTime;
            yield return null;
        }
        obj.localScale = finalScale;
    }

    public IEnumerator ScaleChangeSmoothDamp(Transform obj, float fromScale, float toScale = 1f, float duration = 1f, float delay = 0)
    {
        Vector3 initialScale = obj.localScale * fromScale;
        Vector3 finalScale = obj.localScale * toScale;

        obj.localScale = initialScale;
        float progress = 0;
        Vector3 velocity = Vector3.zero;

        yield return new WaitForSeconds(delay);


        while (progress <= duration * 10)
        {
            obj.localScale = Vector3.SmoothDamp(obj.localScale, finalScale, ref velocity, duration);
            progress += Time.deltaTime;
            yield return null;
        }
        //obj.localScale = finalScale;
    }

    public IEnumerator YPosTransform(Transform obj, float fromYOffset, float toYOffset, float duration = 2f)
    {
        Vector3 initialTrans = obj.position + new Vector3(0, fromYOffset, 0);
        Vector3 fianllTrans = obj.position + new Vector3(0, toYOffset, 0);
        obj.position = initialTrans;
        float progress = 0;


        while (progress <= 1)
        {
            obj.position = Vector3.Lerp(initialTrans, fianllTrans, progress);
            progress += Time.deltaTime * duration;
            yield return null;
        }


        //obj.position = fianllTrans;
    }

    public IEnumerator YPosTransformSmoothDamp(Transform obj, float fromYOffset, float toYOffset, float duration = 1f)
    {
        Vector3 initialTrans = obj.position + new Vector3(0, fromYOffset, 0);
        Vector3 fianllTrans = obj.position + new Vector3(0, toYOffset, 0);
        obj.position = initialTrans;
        float progress = 0;
        Vector3 velocity = Vector3.zero;


        while (progress <= duration * 10)
        {
            obj.position = Vector3.SmoothDamp(obj.position, fianllTrans, ref velocity, duration);
            progress += Time.deltaTime;
            yield return null;
        }

        obj.position = fianllTrans;
    }

    // TODO need to check if there is performance issue
    public GameObject[] GetObjectWithInRadiusByTags(Vector3 central, float radius, string[] tags)
    {
        List<GameObject> allObjects = new List<GameObject>();
        foreach (string tag in tags)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            allObjects.AddRange(objs);
        }

        return allObjects.Where(enemy => {
            float distance = DistanceBetweenVector(central, enemy.transform.position);
            return distance <= radius;
        }).ToArray<GameObject>();
    }

    public IEnumerator DelayDestory(GameObject obj, float delay) {
        yield return new WaitForSeconds(delay);
        if (obj != null) {
            Destroy(obj);
        }
    }

    public GameObject SelectNearestObj(GameObject source, GameObject[] targets) {
        //GameObject nearest = targets.OrderBy(obj => Vector3.Distance(source.transform.position, obj.transform.position)).First();
        if (targets.Length == 1) {
            return targets[0];
        } else if (targets.Length > 1) { 
            return targets.Aggregate((min, x) => DistanceBetweenObject(source, x) < DistanceBetweenObject(source, min) ? x : min);
        }
        return null;
    }

    /*public IEnumerator FadeOut(MeshRenderer targetMeshRender, float duration) {
        if (targetMeshRender != null) {
            float startTime = Time.time;
            float progress = 0;
            Color startColor = targetMeshRender.material.color;
            Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0);
       
            while (progress <= duration)
            {
                yield return new WaitForEndOfFrame();
                progress = Time.time - startTime;
                if (targetMeshRender != null)
                {
                    targetMeshRender.material.color = Color.Lerp(startColor, targetColor, progress / duration);
                }
                else
                {
                    break;
                }
            }
        }
    }*/

}
