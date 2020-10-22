using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionHandlerGame : MonoBehaviour
{
    public SpriteRenderer HouseExterior;

    [Header("Halloween")]
    public RuntimeAnimatorController PlayerHalloweenController;
    public Sprite HouseExperiorHalloween;

    public void LoadEditionStuff()
    {
        if (Master.GameEdition == "Halloween")
        {
            Master.Player.GetComponent<Animator>().runtimeAnimatorController = PlayerHalloweenController;
            HouseExterior.sprite = HouseExperiorHalloween;
        }
    }
}