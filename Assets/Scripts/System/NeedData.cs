using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CarringObjectType
{
    Money,
    ToiletPaper,
    Coffee,
    Burger,
    Pasta,
    Soda,
    Ticket,
    Toilet,
    WaitForStation,
    Destroy
}

[CreateAssetMenu(fileName = "NeedData", menuName = "GameData")]
public class NeedData : ScriptableObject
{
    public Sprite[] _needSprites;
}
