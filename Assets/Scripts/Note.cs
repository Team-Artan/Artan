using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {
    const float adjust = 0.532f;
    bool isOn = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float y = transform.localPosition.y;
        if (!isOn&&y > adjust)
        {
            transform.Translate(new Vector3(0, -0.1f * Time.deltaTime, 0), Space.Self);
            if (transform.localPosition.y - (transform.localScale.y/2f) < adjust)
            {
                isOn = true;
                Vector3 newPos = transform.localPosition;
                newPos.y = adjust + (transform.localScale.y / 2f);
                transform.localPosition = newPos;
            }
        }
        else
        {
            isOn = true;
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;
            scale.y-= 0.2f * Time.deltaTime;
            if (scale.y < 0)
                Destroy(this.gameObject);
            transform.localScale = scale;
            pos.y = adjust + (transform.localScale.y/2f);
            transform.localPosition = pos;
        }
	}
    void OnCollisionEnter(Collision coll)
    {
        if(coll.transform.tag == "Board")
        {

        }
    }
}
