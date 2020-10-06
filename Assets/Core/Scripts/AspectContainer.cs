using UnityEngine;
using UnityHelpers;

public class AspectContainer : MonoBehaviour
{
    private ContainerCreator CC { get { if (_cc == null) _cc = GetComponent<ContainerCreator>(); return _cc; } }
    private ContainerCreator _cc;

    public Transform child;

    [Space(10)]
    public float distance;
    public Camera fitTo;
    private float prevDistance;
    private float prevAspect;

    void Update()
    {
        if (Mathf.Abs(distance - prevDistance) > float.Epsilon || Mathf.Abs(fitTo.aspect - prevAspect) > float.Epsilon)
        {
            Vector2 size = fitTo.PerspectiveFrustum(distance);
            CC.size = new Vector3(size.x, size.y, CC.size.z);
            child.localPosition = new Vector3(0, 0, -distance);
        }

        prevAspect = fitTo.aspect;
        prevDistance = distance;
    }
}
