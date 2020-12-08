using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsInterior;
    public Transform Teleport;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Player" || GameObject.Find("Farm handler").GetComponent<Build>().enabled) return;
        StartCoroutine(ChangeScene(collision.transform));
    }

    IEnumerator ChangeScene(Transform player)
    {
        if (IsInterior) TimeSystem.Data.OnIndoors = false; // Is going out
        else TimeSystem.Data.OnIndoors = true; // Is going inside

        // Fancy black fade in + tp + fade out
        GameObject Black = GameObject.Find("Black");
        Color blackColor = Black.GetComponent<SpriteRenderer>().color;
        while (blackColor.a < 1)
        {
            blackColor = new Color(blackColor.r, blackColor.g, blackColor.b, blackColor.a + 0.1f);
            Black.GetComponent<SpriteRenderer>().color = blackColor;
            yield return new WaitForSeconds(0.025f);
        }

        player.position = Teleport.position;

        while (blackColor.a > 0)
        {
            blackColor = new Color(blackColor.r, blackColor.g, blackColor.b, blackColor.a - 0.1f);
            Black.GetComponent<SpriteRenderer>().color = blackColor;
            yield return new WaitForSeconds(0.025f);
        }
    }
}