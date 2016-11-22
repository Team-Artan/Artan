using UnityEngine;
using System.Collections;
using System;

public class PositionGuide : MonoBehaviour {
    private ArtanHololensManager hm;

    public float standardHeight = 1;
    public Material material;

    void Start()
    {
        hm = ArtanHololensManager.Instance;
    }

    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8) {
            a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    void Update()
    {
        transform.position = hm.GazePosition;

        if (transform.position.y > standardHeight) {
            material.color = hexToColor("1AEE8440");
        }
        else {
            material.color = hexToColor("F0252552");
        }
    }
}
