using UnityEngine;
using UnityHelpers;

public class ContainerCreator : MonoBehaviour
{
    public Vector3 size;
    public float visualThickness = 0.01f;
    public float colliderThickness = 500;
    [Space(10)]
    public Transform liquid;
    public Sprayer[] sprayers;
    [Space(10)]
    public Transform visualsParent;
    public Transform collidersParent;
    [Space(10)]
    public Material visualMaterial;

    private Transform[] visuals = new Transform[5];
    private BoxCollider[] colliders = new BoxCollider[6];
    private Vector3 prevSize;
    private float prevVisThicc;
    private float prevColThicc;

    private static readonly Vector3[] directions = { Vector3.up, Vector3.right, Vector3.forward, -Vector3.up, -Vector3.right, -Vector3.forward };

    void Start()
    {
        for (int i = 0; i < visuals.Length; i++)
        {
            var side = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            side.name = "Side " + i;
            Destroy(side.GetComponent<Collider>());
            var sideRend = side.GetComponent<Renderer>();
            sideRend.sharedMaterial = visualMaterial;
            sideRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            side.SetParent(visualsParent);
            visuals[i] = side;
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            var side = new GameObject("Side " + i).transform;
            side.SetParent(collidersParent);
            colliders[i] = side.gameObject.AddComponent<BoxCollider>();
        }

        Redraw();
    }
    void Update()
    {
        if (SizeChanged())
            Redraw();

        prevSize = size;
        prevVisThicc = visualThickness;
        prevColThicc = colliderThickness;
    }

    private bool SizeChanged()
    {
        return Mathf.Abs(visualThickness - prevVisThicc) > float.Epsilon || Mathf.Abs(colliderThickness - prevColThicc) > float.Epsilon || Mathf.Abs(size.x - prevSize.x) > float.Epsilon || Mathf.Abs(size.y - prevSize.y) > float.Epsilon || Mathf.Abs(size.z - prevSize.z) > float.Epsilon;
    }
    public void Redraw()
    {
        liquid.localPosition = Vector3.zero;
        liquid.localScale = size * 0.99f;

        for (int i = 0; i < sprayers.Length; i++)
        {
            var sprayer = sprayers[i];
            sprayer.transform.localPosition = new Vector3(size.x / 4 * (i % 2 == 0 ? -1 : 1), -size.y / 2, 0);
            sprayer.center = Vector3.up * size.y / 2;
            sprayer.size = new Vector3(size.x / 2, size.y, size.z);
        }

        for (int i = 0; i < directions.Length; i++)
        {
            var currentDir = directions[i];
            Vector3 sizeOnAxis = size.Multiply(currentDir);

            Vector3 visualOffset = Vector3.zero;
            if (i < visuals.Length)
            {
                var visualScale = new Vector3(1 - Mathf.Abs(currentDir.x), 1 - Mathf.Abs(currentDir.y), 1 - Mathf.Abs(currentDir.z));
                visualScale = visualScale.Multiply(size);
                visualScale += currentDir * visualThickness;
                var localPos = sizeOnAxis / 2 + currentDir * visualThickness / 2;
                visuals[i].localPosition = localPos;
                visuals[i].localScale = visualScale;

                visualOffset = currentDir * visualThickness;
            }

            colliders[i].transform.localPosition = sizeOnAxis / 2 + visualOffset;
            colliders[i].size = Vector3.one * colliderThickness;
            colliders[i].center = currentDir * colliderThickness / 2;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
