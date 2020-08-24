using UnityEngine;

[System.Serializable]
public class PlowedSoilData
{ 
    [SerializeField]
    public Crop Crop;
    [SerializeField]
    public bool HasCrop;
    [SerializeField]
    public bool HasFertilizer;
    [SerializeField]
    public bool HasDripBottle;
    [SerializeField]
    public int WaterUnits;
    [SerializeField]
    public Vector2 Pos;
    [SerializeField]
    public string Sprite;

    public PlowedSoilData(Crop crop, bool hasCrop, bool hasFertilizer, bool hasDripBottle, int waterUnits, Vector2 pos, string sprite)
    {
        Crop = crop;
        HasCrop = hasCrop;
        HasFertilizer = hasFertilizer;
        HasDripBottle = hasDripBottle;
        WaterUnits = waterUnits;
        Pos = pos;
        Sprite = sprite;
    }

    public void Load(GameObject plowedSoil)
    {
        plowedSoil.GetComponent<PlowedSoil>().Load(this);
        Farm.PlowedSoils.Add(plowedSoil.GetComponent<PlowedSoil>());
    }
}