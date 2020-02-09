using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static bool OnIndoors;
    public GameObject IndoorsArea;
    public Vector3 Outdoors;
    public Vector3 Indoors;

    private void Start()
    {
        OnIndoors = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnIndoors)
        {
            collision.transform.position = Outdoors;
            StartCoroutine(LeaveBasement());
        }
        else
        {
            collision.transform.position = Indoors;
            StartCoroutine(GoToBasement());
        }
    }

    IEnumerator GoToBasement()
    {
        IndoorsArea.SetActive(true);
        OnIndoors = true;
        Color colorBasement = IndoorsArea.GetComponent<SpriteRenderer>().color;
        Color colorDark = IndoorsArea.transform.Find("Black background").gameObject.GetComponent<SpriteRenderer>().color;
        while (colorBasement.a < 1)
        {
            colorBasement = new Color(colorBasement.r, colorBasement.g, colorBasement.b, colorBasement.a + 0.1f);
            IndoorsArea.GetComponent<SpriteRenderer>().color = colorBasement;
            colorDark = new Color(colorDark.r, colorDark.g, colorDark.b, colorDark.a + 0.1f);
            IndoorsArea.transform.Find("Black background").gameObject.GetComponent<SpriteRenderer>().color = colorDark;
            yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator LeaveBasement()
    {
        Color colorBasement = IndoorsArea.GetComponent<SpriteRenderer>().color;
        Color colorDark = IndoorsArea.transform.Find("Black background").gameObject.GetComponent<SpriteRenderer>().color;
        while (colorBasement.a > 0)
        {
            colorBasement = new Color(colorBasement.r, colorBasement.g, colorBasement.b, colorBasement.a - 0.1f);
            IndoorsArea.GetComponent<SpriteRenderer>().color = colorBasement;
            colorDark = new Color(colorDark.r, colorDark.g, colorDark.b, colorDark.a - 0.1f);
            IndoorsArea.transform.Find("Black background").gameObject.GetComponent<SpriteRenderer>().color = colorDark;
            yield return new WaitForSeconds(0.025f);
        }
        IndoorsArea.SetActive(false);
        OnIndoors = false;
    }
}
