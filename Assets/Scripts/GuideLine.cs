using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLine : MonoBehaviour {
    public GameObject noteLine;
	// Use this for initialization
	void Start () {
        for(float i = ((1f/36f)-0.0045f)*-15; i < ((1f / 36f) - 0.0045f) * 15; i += ((1f / 36f) - 0.0045f))
        {
            GameObject newNote = Instantiate(noteLine, noteLine.transform.parent) as GameObject;
            newNote.transform.localPosition = noteLine.transform.localPosition;
            newNote.transform.localRotation = noteLine.transform.localRotation;
            newNote.transform.Translate(new Vector3(i, 0, 0));

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
