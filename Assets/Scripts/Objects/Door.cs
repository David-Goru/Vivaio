using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsHouse;
    public Vector3 Outdoors;
    public Vector3 Indoors;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Player" || GameObject.Find("Farm handler").GetComponent<Build>().enabled) return;
        StartCoroutine(ChangeScene(!TimeSystem.Data.OnIndoors, collision.transform));
    }

    IEnumerator ChangeScene(bool toIndoors, Transform player)
    {
        if (toIndoors) TimeSystem.Data.OnIndoors = true;
        else TimeSystem.Data.OnIndoors = false;

        GameObject Black = GameObject.Find("Black");
        Color blackColor = Black.GetComponent<SpriteRenderer>().color;
        while (blackColor.a < 1)
        {
            blackColor = new Color(blackColor.r, blackColor.g, blackColor.b, blackColor.a + 0.1f);
            Black.GetComponent<SpriteRenderer>().color = blackColor;
            yield return new WaitForSeconds(0.025f);
        }

        if (toIndoors) player.position = Indoors;
        else player.position = Outdoors;

        while (blackColor.a > 0)
        {
            blackColor = new Color(blackColor.r, blackColor.g, blackColor.b, blackColor.a - 0.1f);
            Black.GetComponent<SpriteRenderer>().color = blackColor;
            yield return new WaitForSeconds(0.025f);
        }
    }
}