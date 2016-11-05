using UnityEngine;
using System.Collections;

public class TankTracksController : MonoBehaviour 
{

    public GameObject effect;
    public GameObject Location;
    GameObject temp;
    public bool reload;
    public TrackController leftTrack;
	public TrackController rightTrack;
	public float leftSpeed;
	public bool  leftFreeWheel;
	public float rightSpeed;
	public bool  rightFreeWheel;
	public bool  showDebug;
    public GameObject tankHead;
    public GameObject barrel;
    [Range(-180f, 180f)]
    public float tankHead_degree;
    [Range(0.0f, 26f)]
    public float barrel_degree;
	// Use this for initialization
	void Start () 
	{
        reload = false;
		if (leftTrack != null)
		{
			leftTrack.Init();
		}

		if (rightTrack != null)
		{
			rightTrack.Init();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (leftTrack != null)
		{
			leftTrack.Update(leftSpeed, leftFreeWheel);
		}
		
		if (rightTrack != null)
		{
			rightTrack.Update(rightSpeed, rightFreeWheel);
		}


        tankHead.transform.localRotation = Quaternion.Euler(0, tankHead_degree, 0);
        barrel.transform.localRotation = Quaternion.Euler(-barrel_degree,0 , 0);

        if (Input.GetKeyDown(KeyCode.Space) && temp == null)
        {
            temp = GameObject.Instantiate(effect, Location.transform.position, Location.transform.rotation) as GameObject;
            reload = true;
        }
        if (temp != null)
        {

            if (temp.GetComponent<ParticleSystem>().isPlaying == false)
            {
                Debug.Log("stop");
                Destroy(temp);
                reload = false;
            }
        }

    }

	public void OnDrawGizmos()
	{
		if ( showDebug )
		{
			if ( leftTrack != null )
			{
				leftTrack.OnDrawGizmos();
			}

			if ( rightTrack != null )
			{
				rightTrack.OnDrawGizmos();
			}
		}
	}
}
