using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class TankController : MonoBehaviour {

    public GameObject TankHead;
    public GameObject TankCannon;
    public GameObject Bullet;

    private Pathfinding path;
    private Coroutine MoveCoroutine;

    private float speed = 0.07f;

    private ArtanHololensManager hm;
    private HololensTarget holo;
    private Vector3 movePos;

    // Use this for initialization
    void Start()
    {
        hm = ArtanHololensManager.Instance;
        holo = GetComponent<HololensTarget>();
        movePos = new Vector3();
        path = GetComponent<Pathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hm == null) {
            float deltaX = Input.GetAxis("Horizontal");
            float deltaY = Input.GetAxis("Vertical");

            if (Input.GetKeyUp(KeyCode.Space)) {
                Shoot(1000f, 0);
            }

            Vector3 deltaPos = new Vector3(deltaX, deltaY, 0);
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

        path.FindPath(transform.position, movePos);
        if (path.Path.Count != 0) {
            if (MoveCoroutine != null) {
                StopCoroutine(MoveCoroutine);
            }

            MoveCoroutine = StartCoroutine(PointMove(path.Path));
        }
    }

    public void Shoot(float power, int bounceCount)
    {
        GameObject bullet = Instantiate(Bullet) as GameObject;
        bullet.transform.position = Bullet.transform.position;
        bullet.transform.rotation = Bullet.transform.rotation;
        bullet.GetComponent<MeshRenderer>().enabled = true;
        bullet.GetComponent<CapsuleCollider>().isTrigger = true;
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
    public IEnumerator PointMove(List<Vector3> pathList)
    {
        foreach (var point in pathList) {
            float distanceX = point.x - this.transform.position.x;
            float distanceZ = point.z - this.transform.position.z;
            float angle = Mathf.Atan2(distanceX, distanceZ) * Mathf.Rad2Deg;
            Quaternion target = Quaternion.Euler(0, angle, 0);

            while (true) {
                var vec3rotation = transform.rotation.eulerAngles;
                var rotation = Quaternion.Euler(0, vec3rotation.y, 0);
                if (Quaternion.Angle(rotation, target) <= 0.1f) {
                    break;
                }

                this.transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 90f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            while (Vector3.Distance(this.transform.position, point) > 0.01f) {
                Quaternion rot = Quaternion.Euler(new Vector3(0, angle, 0));
                this.transform.rotation = rot;
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                RaycastHit hitData;
                Physics.Raycast(transform.position, Vector3.down, out hitData);
                if (Physics.Raycast(transform.position, Vector3.down, 2)) {
                    transform.rotation = Quaternion.FromToRotation(transform.up, hitData.normal) * transform.rotation;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        MoveCoroutine = null;
    }
}
