using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksSystem : MonoBehaviour
{
    public GameObject[] Fireworks;

    void Start()
    {
        StartCoroutine(RandomFirework());
    }

    IEnumerator RandomFirework()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        Instantiate(Fireworks[Random.Range(0, 4)], new Vector3(-8.5f + Random.Range(0, 1.5f), 10.5f + Random.Range(0, 0.25f), 0), Quaternion.Euler(0, 0, 0));
        StartCoroutine(RandomFirework());
    }
}
