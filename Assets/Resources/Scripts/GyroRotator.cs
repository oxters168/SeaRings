using UnityEngine;

public class GyroRotator : MonoBehaviour
{
    public Rigidbody target;

    private void Start()
    {
        Input.gyro.enabled = true;
    }
    private void FixedUpdate()
    {
        Quaternion deviceRotation = Input.gyro.attitude;
        target.MoveRotation(deviceRotation);
    }
}
