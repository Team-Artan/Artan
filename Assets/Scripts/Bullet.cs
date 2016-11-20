using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Bullet : MonoBehaviour {
    public GameObject particle;
    public TankController owner;
    public Vector3 velocity = Vector3.zero;

    private Vector3 prePosition = Vector3.zero;
    private Rigidbody rigid;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rigid.velocity != Vector3.zero) {
            Vector3 relativePos = transform.position - prePosition;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation * Quaternion.Euler(90, 0, 0);
        }
        prePosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        var target = collision.gameObject;
        var tank = target.GetComponent<TankController>();

        // Return if self
        if (tank != null && tank == owner) {
            return;
        }

        if (target.CompareTag("Bullet") == false && tank != null && tank != owner) {
            Instantiate(particle, this.transform.position, Quaternion.identity);

            GameObject[] objects = GameObject.FindGameObjectsWithTag("Tank");

            foreach (GameObject obj in objects) {
                if (Vector3.Distance(obj.transform.position, transform.position) < 0.5)
                    tank.Damage(20);
            }
        }

        owner.EndTurn();
        Destroy(gameObject);
    }
}
