using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    public GameObject effect;
    bool hit = false;
    GameObject temp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(temp != null)
        {
            if (temp.GetComponent<ParticleSystem>().isPlaying == false)
            {
                Destroy(temp);
                Destroy(this.gameObject);
            }
        }
	}
    void OnCollisionEnter(Collision coll)
    {

        temp = Instantiate(effect, transform.position, transform.rotation) as GameObject;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

    }
}
