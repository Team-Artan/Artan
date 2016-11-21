using UnityEngine;

public class GravityObject : MonoBehaviour {
    public Vector3 g = new Vector3(0, -0.0098f, 0);

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        velocity += g * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
    }
}