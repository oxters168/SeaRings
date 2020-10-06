using UnityEngine;
using UnityHelpers;

public class PositionFitCamera : MonoBehaviour
{
    Bounds objectBounds;
    public Camera looker;
    public Transform child;
    [Space(10)]
    public float multiplier = 1;

    private float prevX, prevY;
    private float prevAspect;
    private float distance = 0;

    void Update()
    {
        objectBounds = transform.GetTotalBounds(Space.Self);
        if (Mathf.Abs(objectBounds.size.x - prevX) > float.Epsilon || Mathf.Abs(objectBounds.size.y - prevY) > float.Epsilon || Mathf.Abs(looker.aspect - prevAspect) > float.Epsilon)
        {
            prevX = objectBounds.size.x;
            prevY = objectBounds.size.y;
            prevAspect = looker.aspect;

            if (prevX / prevY < prevAspect)
                distance = looker.PerspectiveDistanceFromHeight(prevY);
            else
                distance = looker.PerspectiveDistanceFromWidth(prevX);
        }
        child.localPosition = new Vector3(0, 0, -(distance * multiplier));
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(objectBounds.center, objectBounds.size);
    }
}
