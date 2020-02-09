using System.Collections;
using UnityEngine;

public class CoinAnim : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyCoin());
    }

    IEnumerator DestroyCoin()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}