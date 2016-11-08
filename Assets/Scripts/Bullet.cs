using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Bullet : MonoBehaviour {
    private Vector3 prePosition = Vector3.zero;
    public GameObject particle;
    private Rigidbody rigid;
    private int bounceCount = 0;
    float time = 0f;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        time += Time.fixedDeltaTime;
        if (rigid.velocity!=Vector3.zero) {
            Vector3 relativePos = transform.position - prePosition;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation * Quaternion.Euler(90,0,0);
        }
        prePosition = transform.position;
	}

    void OnCollisionEnter(Collision collision) {

        if (!collision.gameObject.CompareTag("Bullet")&& collision.gameObject.name!="PlayerTank")    {
            print(collision.gameObject.name);

            Instantiate(particle, this.transform.position, Quaternion.identity);
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Tank");
            foreach (GameObject obj in objects) {
                if (Vector3.Distance(obj.transform.position, transform.position) < 0.5)
                    obj.GetComponent<TankController>().hpContent.GetComponent<Health_UI>().GetDamaged(20);
            }
            Destroy(this.gameObject);
        }
    }
}
