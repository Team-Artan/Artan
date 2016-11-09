using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class TankController : MonoBehaviour {

    public GameObject TankHead;
    public GameObject TankCannon;
    public GameObject Bullet;

    private Coroutine MoveCoroutine;

    private float speed = 0.07f;

    public GameObject hpContent;

    private ArtanHololensManager hm;
    private HololensTarget holo;
    private Vector3 movePos;

    // Use this for initialization
    void Start()
    {
        hm = ArtanHololensManager.Instance;
        holo = GetComponent<HololensTarget>();
        movePos = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (hm == null) {
            float deltaX = Input.GetAxis("Horizontal");
            float deltaY = Input.GetAxis("Vertical");

            if (Input.GetKeyUp(KeyCode.Space)) {
                Shoot(100f, 0);
            }

            Vector3 deltaPos = new Vector3(deltaX, 0, deltaY);
            HeadRotate(deltaPos);

            if (Input.GetMouseButtonUp(0)) {
                Vector3 pos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(pos);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                for (int i = 0; i < hits.Length; ++i) {
                    RaycastHit hit = hits[i];

                    if (hit.collider.gameObject.name.Equals("Terrain")) {
                        if (MoveCoroutine != null) {
                            StopCoroutine(MoveCoroutine);
                        }

                        MoveTo(hit.point);
                    }
                }
            }
        }
        else {
            // Move
            if (hm.Tapped == true) {
                MoveTo(hm.GazePosition);
            }

            // Rotate
            if (hm.Holding == true) {
                HeadRotate(hm.TapHandDeltaMove);
            }

            Debug.DrawRay(transform.position, movePos - transform.position);
        }
    }

    private void MoveTo(Vector3 destPos)
    {
        movePos = destPos;

        if (MoveCoroutine != null) {
            StopCoroutine(MoveCoroutine);
        }

//        MoveCoroutine = StartCoroutine(PointMove(path.Path));
    }

    public void Shoot(float power, int bounceCount)
    {
        GameObject bullet = Instantiate(Bullet) as GameObject;
        bullet.transform.position = Bullet.transform.position;
        bullet.transform.rotation = Bullet.transform.rotation;
        bullet.GetComponent<MeshRenderer>().enabled = true;
        bullet.GetComponent<CapsuleCollider>().isTrigger = false;
        bullet.transform.localScale = transform.localScale;
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        bulletRigid.isKinematic = false;
        bulletRigid.AddRelativeForce(Vector3.up * power);
    }
    public void HeadRotate(Vector3 deltaPos)
    {
        deltaPos *= 100;
        TankHead.transform.Rotate(new Vector3(0, deltaPos.x, 0));
        TankCannon.transform.Rotate(new Vector3(deltaPos.z, 0, 0));
        float cannonAngle = TankCannon.transform.localRotation.eulerAngles.x;
        if (cannonAngle > 180f)
            cannonAngle -= 360;
        if (cannonAngle < -25f)
            TankCannon.transform.localRotation = Quaternion.Euler(new Vector3(-25f, 0, 0));
        else if (cannonAngle > 25f)
            TankCannon.transform.localRotation = Quaternion.Euler(new Vector3(25f, 0, 0));
    }
}
