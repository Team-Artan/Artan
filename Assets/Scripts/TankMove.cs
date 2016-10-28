using UnityEngine;
using System.Collections;

public class TankMove : MonoBehaviour {
    public Camera mainCamera;
    public float speed = 10f;
    public float power = 30f;
    TankTracksController trackCon;
    public GameObject Bomb;
	// Use this for initialization
	void Start () {
        trackCon = GetComponent<TankTracksController>();
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        Shot();
        turretMove();
	}

    //Space 를 누르면 발사
    void Shot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && trackCon.reload == false)
        {
            GameObject temp = Instantiate(Bomb, trackCon.Location.transform.position, Quaternion.Euler(trackCon.Location.transform.rotation.x + 90, trackCon.Location.transform.rotation.y + trackCon.tankHead_degree, trackCon.Location.transform.rotation.z + trackCon.barrel_degree)) as GameObject;
            temp.GetComponent<Rigidbody>().velocity = trackCon.Location.transform.forward * power;

        }
    }

    //마우스를 누른 곳으로 이동
    void Move()
    {
        float step = speed * Time.deltaTime;
        RaycastHit hitobj;
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitobj, Mathf.Infinity))
            {
                if (hitobj.transform.tag != "tank")
                {
                    transform.position = Vector3.MoveTowards(transform.position, hitobj.point, step);
                    transform.rotation = Quaternion.LookRotation(hitobj.point - transform.position);
                }
                
            }
        }
        if (this.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            trackCon.leftSpeed = 1f;
            trackCon.rightSpeed = 1f;
        }else
        {
            trackCon.leftSpeed = 0f;
            trackCon.rightSpeed = 0f;
        }
    }
    void turretMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (trackCon.barrel_degree < 26)
            {
                trackCon.barrel_degree += 0.5f;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (trackCon.barrel_degree > 0)
            {
                trackCon.barrel_degree -= 0.5f;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (trackCon.tankHead_degree == -180)
            {
                trackCon.tankHead_degree = 180;
            }
            trackCon.tankHead_degree -= 1f;

        }
        if (Input.GetKey(KeyCode.D))
        {
            if(trackCon.tankHead_degree == 180)
            {
                trackCon.tankHead_degree = -180;
            }
            trackCon.tankHead_degree += 1f;
        }
    }
}
