using System.Collections;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float Timer;

    void Start()
    {
        StartCoroutine(RemoveObject());
    }

    IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(Timer);
        Destroy(gameObject);
    }
}