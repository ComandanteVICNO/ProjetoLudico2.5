using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyShatterd : MonoBehaviour
{
    public float minTimeToDestroy;
    public float maxTimeToDestroy;

    public float timeToDestoyParent;

    private void Awake()
    {
        DestroyChildrenWithRandomTimes();
        Invoke("DestroyGameObject", timeToDestoyParent);
    }

    void DestroyChildrenWithRandomTimes()
    {
        foreach (Transform child in transform)
        {
            float randomTime = Random.Range(minTimeToDestroy, maxTimeToDestroy);
            StartCoroutine(DestroyChildAfterTime(child.gameObject, randomTime));
        }
    }
    IEnumerator DestroyChildAfterTime(GameObject child, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(child);
    }


    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
