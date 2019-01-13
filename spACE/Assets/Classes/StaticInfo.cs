using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInfo {
    //static public SortedDictionary<string, string> strings = new SortedDictionary<string, string>();
    static public string player = "ImperialWolf";
    static public string shipName = "shema";
    static public string currentWeaponColor;
    static public string currentBarrelType;
    static public float score;
    static public Spaceship shipInfo;
    static public GameObject ship;
    static public string weapon = "Lazerator";
    static public SortedDictionary<string, Bullet> lasers = new SortedDictionary<string, Bullet>();
    static public List<string> barrels = new List<string>();
}
