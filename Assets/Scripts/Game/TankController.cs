using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class TankController : MonoBehaviour {
    public GameObject TankHead;
    public GameObject TankCannon;
    public GameObject Bullet;

    private Coroutine MoveCoroutine;

    public GameObject hpContent;

    private float speed = 0.07f;

    private ArtanHololensManager hm;
    private HololensTarget holo;
    private Vector3 movePos;

    private Seeker seeker;
    private Unit unit;

    public bool TurnEnded { get; private set; }

    // Use this for initialization
    void Start()
    {
        hm = ArtanHololensManager.Instance;
        holo = GetComponent<HololensTarget>();
        movePos = new Vector3();

        seeker = GetComponent<Seeker>();
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        // State
        if (unit.IsDead == true) {
            GameManager.Instance.DestroyUnit(unit);
            return;
        }

        HandleInput();
    }

    private void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        if (p.error) {
            Debug.Log("Error : " + p.errorLog);
        }
        else {
            Debug.Log("path : " + p.path.Count);
        }

        if (MoveCoroutine != null) {
            StopCoroutine(MoveCoroutine);
        }

        MoveCoroutine = StartCoroutine(PointMove(p.vectorPath));
    }

    private void MoveTo(Vector3 destPos)
    {
        movePos = destPos;

        seeker.StartPath(transform.position, destPos, OnPathComplete);
    }

    public void Shoot(float power, int bounceCount)
    {
        GameObject bullet = Instantiate(Bullet) as GameObject;
        bullet.transform.position = Bullet.transform.position;
        bullet.transform.rotation = Bullet.transform.rotation;
        bullet.GetComponent<MeshRenderer>().enabled = true;
        bullet.GetComponent<CapsuleCollider>().isTrigger = false;
        bullet.transform.localScale = transform.localScale;
        bullet.GetComponent<Bullet>().owner = this;

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

    public void HeadRotate2()
    {
        float cannonAngle = TankCannon.transform.localRotation.eulerAngles.x;
        if (hm.TapHandDeltaMove.x > 0.005f) {
            TankHead.transform.Rotate(new Vector3(0, 1f, 0), Space.Self);
        }
        else if (hm.TapHandDeltaMove.x < -0.005f) {
            TankHead.transform.Rotate(new Vector3(0, -1f, 0), Space.Self);
        }

        if (hm.TapHandDeltaMove.y > 0) {
            TankCannon.transform.Rotate(new Vector3(-1f, 0, 0), Space.Self);
        }
        else if (hm.TapHandDeltaMove.y < 0) {
            TankCannon.transform.Rotate(new Vector3(1f, 0, 0), Space.Self);
        }
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
                transform.rotation = rot;

                var dir = (point - transform.position).normalized;
                transform.position += dir * speed * Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }

        MoveCoroutine = null;
    }

    public void Damage(int damage)
    {
        unit.Damage(damage);
        hpContent.GetComponent<Health_UI>().GetDamaged(damage);
    }

    public void BeginTurn()
    {
        TurnEnded = false;
    }

    public void EndTurn()
    {
        TurnEnded = true;
    }

    public void HandleInput()
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
            if (hm.Tapped == true || hm.GetVoiceCommand("select") == true) {
                MoveTo(hm.GazePosition);
            }
            // Rotate
            else if (hm.Holding == true) {
                //HeadRotate(hm.TapHandDeltaMove);
                HeadRotate2();
            }
            // Attack
            else if (hm.GetVoiceCommand("fire") == true) {
                Shoot(100, 0);
            }
            // Force end turn
            else if (hm.GetVoiceCommand("end turn") == true) {
                EndTurn();
            }

            Debug.DrawRay(transform.position, movePos - transform.position);
        }
    }
}
