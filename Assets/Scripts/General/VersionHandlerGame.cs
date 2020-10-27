using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionHandlerGame : MonoBehaviour
{
    public SpriteRenderer HouseExterior;

    [Header("Halloween")]
    public RuntimeAnimatorController PlayerHalloweenController;
    public Sprite HouseExperiorHalloween;
    public GameObject InitialModuleHalloween;
    public GameObject LastModuleHalloween;

    public void LoadEditionStuff()
    {
        if (Master.GameEdition == "Halloween")
        {
            Master.Player.GetComponent<Animator>().runtimeAnimatorController = PlayerHalloweenController;
            HouseExterior.sprite = HouseExperiorHalloween;
            InitialModuleHalloween.SetActive(true);
            LastModuleHalloween.SetActive(true);
        }
    }
}