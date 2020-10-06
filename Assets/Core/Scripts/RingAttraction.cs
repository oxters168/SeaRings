using UnityEngine;
using UnityHelpers;

public class RingAttraction : MonoBehaviour
{
    public Rigidbody PhysicsBody { get { if (_physicsBody == null) _physicsBody = GetComponent<Rigidbody>(); return _physicsBody; } }
    private Rigidbody _physicsBody;

    public float attractionConstant = 0.01f;
    public float distPower = 0.5f;

    void Update()
    {
        var rings = FindObjectsOfType<RingAttraction>();
        foreach (var ring in rings)
        {
            Vector3 offset = transform.position - ring.transform.position;
            Vector3 force = ((PhysicsBody.velocity * Vector3.Dot(PhysicsBody.velocity.normalized, offset.normalized)) / Mathf.Pow(offset.magnitude, distPower)) * attractionConstant;
            if (!float.IsNaN(force.x + force.y + force.z))
                ring.PhysicsBody.AddForce(force, ForceMode.Force);
        }
    }
}
