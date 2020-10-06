using UnityEngine;

public class AspectContainer : MonoBehaviour
{
    private ContainerCreator CC { get { if (_cc == null) _cc = GetComponent<ContainerCreator>(); return _cc; } }
    private ContainerCreator _cc;

    public float width;
    public Camera fitTo;
    private float prevWidth;
    private float prevAspect;

    void Update()
    {
        if (Mathf.Abs(width - prevWidth) > float.Epsilon || Mathf.Abs(fitTo.aspect - prevAspect) > float.Epsilon)
        {
            float heightFromAspect = width / fitTo.aspect;
            CC.size = new Vector3(width, heightFromAspect, CC.size.z);
        }

        prevAspect = fitTo.aspect;
        prevWidth = width;
    }
}
