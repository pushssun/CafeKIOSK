using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Representative,
    New,
    Recommended,
    Coffee,
    Decaffein,
    Beverage,
    Tea,
    Food
}

[Serializable]
public class Menu
{
    public string Name;
    public string Description;
    public int Price;
    public Sprite Image;
    public Type[] Type;
}
