using UnityEngine;
using System.Linq;
using UnityHelpers;

public class Sprayer : MonoBehaviour
{
    public Vector3 center, size;
    [Range(0, 1)]
    public float power;
    public float maxForce = 0.02f;

    void FixedUpdate()
    {
        Vector3 extents = size / 2;
        var hits = Physics.BoxCastAll(transform.TransformPoint(center), extents, transform.up, Quaternion.identity, 0.0001f, 1 << LayerMask.NameToLayer("Ring"));
        var ringsInVicinity = hits.Select((hit) => hit.rigidbody).Distinct();
        foreach (var ring in ringsInVicinity)
        {
            Vector3 ringOffset = transform.InverseTransformPoint(ring.transform.position);
            Vector3 outwardPercents = new Vector3(Mathf.Clamp(ringOffset.x / extents.x, -1, 1), 0, Mathf.Clamp(ringOffset.z / extents.z, -1, 1));
            float distanceMultiplier = 1 - outwardPercents.y;
            outwardPercents = outwardPercents * 2 - Vector3.one;
            outwardPercents = new Vector3(outwardPercents.x, 0, outwardPercents.z);
            outwardPercents = -outwardPercents;
            Vector3 ringExtents = ring.transform.GetTotalBounds(Space.World).extents;
            
            Vector3 ringForcePos = ring.transform.position + outwardPercents.Multiply(ringExtents) - Vector3.up * ringExtents.y;
            Vector3 outwardsDir = new Vector3(ringOffset.x, 0, ringOffset.z).normalized;
            outwardsDir = transform.TransformDirection(outwardsDir);
            float yPercent = Mathf.Clamp01(ringOffset.y / size.y) * 0.666f;
            Vector3 forceDir = Vector3.Lerp(transform.up, outwardsDir, yPercent * yPercent);
            Vector3 forceAppliedToRing = forceDir * maxForce * power * distanceMultiplier;
            ring.AddForceAtPosition(forceAppliedToRing, ringForcePos, ForceMode.Force);
        }
    }
}
