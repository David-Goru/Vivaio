using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsHandler : MonoBehaviour
{
    public List<GameObject> available;
    public List<GameObject> notAvailable;

    private void Start()
    {
        available = new List<GameObject>();
        notAvailable = new List<GameObject>();

        for (int i = 0; i < 6; i++)
        {
            available.Add((GameObject)Instantiate(Resources.Load("Cars/" + i), new Vector2(16, 2.75f + 0.05f * Random.Range(0, 3)), transform.rotation));
        }
        StartCoroutine(carTimer());
    }

    private void Update()
    {
        foreach (GameObject car in notAvailable.ToArray())
        {
            car.transform.position = Vector2.MoveTowards(car.transform.position, new Vector2(-40, 2.75f + 0.05f * Random.Range(0, 3)), Time.deltaTime * 4);
            if (car.transform.position.x < -30)
            {
                available.Add(car);
                notAvailable.Remove(car);
            }
        }
    }

    IEnumerator carTimer()
    {
        yield return new WaitForSeconds(1f);
        if  (Random.Range(0, 4) < 2)
        {
            if (available.Count > 0)
            {
                notAvailable.Add(available[0]);
                available[0].transform.position = new Vector2(16, 13 + 0.05f * Random.Range(0, 3));
                available.RemoveAt(0);
            }
        }
        StartCoroutine(carTimer());
    }
}