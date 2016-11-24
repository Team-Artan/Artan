using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NoteTrigger : MonoBehaviour {

    public bool isEmitter = false;
    public List<GameObject> keyboard = new List<GameObject>();
    public List<GameObject> keyboard2 = new List<GameObject>();
    public List<GameObject> keyboard_color = new List<GameObject>();
    Color defaultColor;
	// Use this for initialization
	void Start () {
        defaultColor = GetComponent<Renderer>().material.color;
        if (isEmitter) {
            isEmitter = false;
            for(float i = -((1f/36f)*20) ; i< ((1f / 36f) * 20) ; i += (1f/36f)) {
                GameObject obj = Instantiate(this.gameObject, this.transform.parent) as GameObject;
                obj.transform.localPosition = this.transform.localPosition;
                obj.transform.localRotation = this.transform.localRotation;
                obj.transform.Translate(new Vector3(i/1.2f, 0, 0), Space.Self);
                keyboard_color.Add(obj);
            }
            for (float i = -((1f / 36f) * 20); i < ((1f / 36f) * 20); i += (1f / 36f))
            {
                GameObject obj = Instantiate(this.gameObject, this.transform.parent) as GameObject;
                obj.transform.localPosition = new Vector3(transform.localPosition.x, -0.5f, transform.localPosition.z);
                obj.transform.localRotation = this.transform.localRotation;
                obj.transform.localScale = new Vector3(obj.transform.localScale.x, 0, obj.transform.localScale.z);
                obj.transform.Translate(new Vector3(i / 1.2f, 0, 0), Space.Self);
                obj.GetComponent<Renderer>().material.color = Color.green;
                keyboard.Add(obj);
            }
            for (float i = -((1f / 36f) * 20); i < ((1f / 36f) * 20); i += (1f / 36f))
            {
                GameObject obj = Instantiate(this.gameObject, this.transform.parent) as GameObject;
                obj.transform.localPosition = new Vector3(transform.localPosition.x, -0.5f, transform.localPosition.z);
                obj.transform.localRotation = this.transform.localRotation;
                obj.transform.localScale = new Vector3(obj.transform.localScale.x, 0, obj.transform.localScale.z);
                obj.transform.Translate(new Vector3(i / 1.2f, 0, 0), Space.Self);
                obj.GetComponent<Renderer>().material.color = Color.magenta;
                keyboard2.Add(obj);
            }
            //Destroy(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {

	}

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Note"))
    //    {
    //        this.GetComponent<Renderer>().material.color = defaultColor;
    //    }
    //}

    //void OnTriggerExit(Collider other) {
    //    if (other.gameObject.CompareTag("Note")) {
    //        this.GetComponent<Renderer>().material.color = Color.black;
    //    }
    //}

}
