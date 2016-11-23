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
    private float lifeTIme;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        lifeTIme = 10.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;

        Vector3 relativePos = transform.position - prePosition;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation * Quaternion.Euler(90, 0, 0);

        prePosition = transform.position;

        lifeTIme -= Time.fixedDeltaTime;
        if (lifeTIme <= 0) {
            Destroy(gameObject);
        }
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
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Tank");

            foreach (GameObject obj in objects) {
                if (obj == owner) {
                    continue;
                }

                if (Vector3.Distance(obj.transform.position, transform.position) < 0.5) {
                    tank.Damage(20);
                }
            }
        }

        owner.EndTurn();

        Instantiate(particle, this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(GameManager.Instance.explodeSound, transform.position);
        Destroy(gameObject);
    }
}