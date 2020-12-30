using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionHandlerGame : MonoBehaviour
{
    [Header("Halloween")]
    public RuntimeAnimatorController PlayerHalloweenController;
    public RuntimeAnimatorController PlayerChristmasController;
    public Sprite HouseExperiorHalloween;
    public Sprite HouseExperiorChristmas;
    public GameObject InitialModuleHalloween;
    public GameObject LastModuleHalloween;
    public Sprite DeliveryBoxChristmasAA;
    public Sprite PresentChristmasAA;

    public static Sprite HouseSprite;
    public static Sprite DeliveryBoxChristmas;
    public static Sprite PresentChristmas;

    public void LoadEditionStuff()
    {
        if (Master.GameEdition == "Halloween")
        {
            Master.Player.GetComponent<Animator>().runtimeAnimatorController = PlayerHalloweenController;
            HouseSprite = HouseExperiorHalloween;
            InitialModuleHalloween.SetActive(true);
            LastModuleHalloween.SetActive(true);
        }
        else if (Master.GameEdition == "Christmas")
        {
            Master.Player.GetComponent<Animator>().runtimeAnimatorController = PlayerChristmasController;
            HouseSprite = HouseExperiorChristmas;
            DeliveryBoxChristmas = DeliveryBoxChristmasAA;
            PresentChristmas = PresentChristmasAA;
        }
    }
}