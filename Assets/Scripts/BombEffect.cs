    using UnityEngine;
    using System.Collections;

public class BombEffect : MonoBehaviour
{

    public GameObject effect;
    public GameObject Location;
    GameObject temp;

    // Use this for initialization
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && temp == null)
        {
            temp = GameObject.Instantiate(effect, Location.transform.position, Location.transform.rotation) as GameObject;
        }
        if (temp != null)
        {
            if(temp.GetComponent<ParticleSystem>().isPlaying == false)
            {
                Debug.Log("stop");
                Destroy(temp);
            }
        }
  

        
    }
}
