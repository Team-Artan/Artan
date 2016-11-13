using UnityEngine;
using System.Collections;

public class PositionGuide : MonoBehaviour {
    private ArtanHololensManager hm;

    public float standardHeight = 1;
    public Material material;
    public GameObject GuidePanel;
    public GameObject player;

	// Use this for initialization
	void Start () {
        hm = ArtanHololensManager.Instance;
    }

    public static Color hexToColor(string hex) {
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
    // Update is called once per frame
    void Update () {
        transform.position = hm.GazePosition;

        if (transform.position.y > standardHeight) {
            material.color = hexToColor("1AEE8440");

            if (hm.Tapped == true || hm.GetVoiceCommand("Select") == true) {
                var obj = Instantiate(player, transform.position, Quaternion.identity);
                obj.SetActive(true);
                obj.transform.position = hm.GazePosition;
                var obj2 = Instantiate(player, transform.position, Quaternion.identity);
                obj2.SetActive(true);
                obj2.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z+15f); ;
                GuidePanel.SetActive(false);
                Destroy(this.gameObject);
            }
        }
        else
            material.color = hexToColor("F0252552");
    }
}
