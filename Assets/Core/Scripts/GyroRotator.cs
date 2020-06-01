using UnityEngine;
using UnityHelpers;

public class GyroRotator : MonoBehaviour
{
    public Rigidbody target;
    private Quaternion previousRotation = Quaternion.identity;
    public Transform gyroVisualizer;
    private Quaternion deltaRotation = Quaternion.Euler(90, 0, 0);

    private void Start()
    {
        Input.gyro.enabled = true;
    }
    private void FixedUpdate()
    {
        Quaternion currentRotation = deltaRotation * GyroToUnity(Input.gyro.attitude);
        Quaternion changeInRotation = Quaternion.Inverse(previousRotation) * currentRotation;
        previousRotation = currentRotation;
        
        target.MoveRotation(target.rotation * changeInRotation);
        gyroVisualizer.localRotation = target.rotation;
    }

    //Source: https://gamedev.stackexchange.com/questions/174107/unity-gyroscope-orientation-attitude-wrong
    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    public void ResetRotation()
    {
        target.rotation = (Quaternion.identity);
    }
}
