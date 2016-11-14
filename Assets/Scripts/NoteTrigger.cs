using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTrigger : MonoBehaviour {

    public bool isEmitter = false;

	// Use this for initialization
	void Start () {
        if (isEmitter) {
            isEmitter = false;
            for(float i = -((1f/38f)*20) ; i< ((1f / 38f) * 20) ; i += (1f/38f)) {
                GameObject obj = Instantiate(this.gameObject, this.transform.parent) as GameObject;
                obj.transform.localPosition = this.transform.localPosition;
                obj.transform.localRotation = this.transform.localRotation;
                obj.transform.Translate(new Vector3(i, 0, 0), Space.Self);
            }
            Destroy(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Note")) {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Note")) {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

}
