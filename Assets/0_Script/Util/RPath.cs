using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPath
{
    public static readonly string Katana = "Items/Weapon/Prefab/Katana/";
    public static readonly string Sword = "Items/Weapon/Prefab/Sword/";
    public static readonly string Lance = "Items/Weapon/Prefab/Lance/";
    public static readonly string Kunckle = "Items/Weapon/Prefab/Kunckle/";
    public static readonly string Mace = "Items/Weapon/Prefab/Mace/";

    public static readonly string WeponeIcon = "Items/Weapon/Icon/";
    public static readonly string ArmorIcon = "Items/Armor/Icon/";
    public static readonly string BootsIcon = "Items/Boots/Icon/";
    public static readonly string GuardIcon = "Items/Guard/Icon/";
    public static readonly string HelmetIcon = "Items/Helmet/Icon/";
    public static readonly string PantsIcon = "Items/Pants/Icon/";
    public static string GetItemIconPath(string itemtype) {
        switch (itemtype) {
            case "W":
                return WeponeIcon;
            case "A":
                return ArmorIcon;
            case "B":
                return BootsIcon;
            case "G":
                return GuardIcon;
            case "H":
                return HelmetIcon;
            case "P":
                return PantsIcon;


        }
        return "";
    }
    public static readonly string SkillIcon = "Skill/Icon/";
    public static readonly string EffectSound = "Sound/Effect/";





}
