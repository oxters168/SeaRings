using UnityEngine;
using UnityHelpers;

public class GyroRotator : MonoBehaviour
{
    //public Rigidbody target;
    private PhysicsTransform target { get { if (_target == null) { _target = GetComponent<PhysicsTransform>(); } return _target; } }
    private PhysicsTransform _target;
    private Quaternion previousRotation = Quaternion.identity;
    public Transform gyroVisualizer;
    private Quaternion deltaRotation = Quaternion.Euler(90, 0, 0);

    private void Start()
    {
        Input.gyro.enabled = true;
        StartCoroutine(ResetInTheBeginning());
    }
    private void Update()
    {
        Quaternion currentRotation = deltaRotation * Input.gyro.attitude.AdjustAttitude();
        Quaternion changeInRotation = Quaternion.Inverse(previousRotation) * currentRotation;
        previousRotation = currentRotation;
        
        Quaternion nextRotation = target.rotation * changeInRotation;
        target.rotation = nextRotation;
        if (gyroVisualizer != null)
            gyroVisualizer.localRotation = transform.rotation;
    }
    private System.Collections.IEnumerator ResetInTheBeginning()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        ResetRotation();
    }

    public void ResetRotation()
    {
        target.rotation = (Quaternion.identity);
    }
}
