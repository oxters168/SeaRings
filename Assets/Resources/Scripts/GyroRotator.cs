using UnityEngine;
using UnityHelpers;

public class GyroRotator : MonoBehaviour
{
    public Rigidbody target;
    private Quaternion previousRotation = Quaternion.identity;
    private uint decimalPlaces = 2;
    public Transform gyroVisualizer;
    private Vector3 deltaRotation = new Vector3(-90, 0, -180);
    public float lerpAmount = 5;

    private void Start()
    {
        Input.gyro.enabled = true;
    }
    private void FixedUpdate()
    {
        Quaternion deviceRotation = Quaternion.Inverse(Quaternion.Euler(deltaRotation) * Input.gyro.attitude);
        gyroVisualizer.localRotation = deviceRotation;
        
        target.MoveRotation(Quaternion.Lerp(target.rotation, deviceRotation.SetDecimalPlaces(decimalPlaces), Time.fixedDeltaTime * lerpAmount));
    }

    public void SetDecimalPlaces(int places)
    {
        decimalPlaces = (uint)places;
    }
    public void SetDeltaX(string x)
    {
        deltaRotation.x = System.Convert.ToSingle(x);
    }
    public void SetDeltaY(string y)
    {
        deltaRotation.y = System.Convert.ToSingle(y);
    }
    public void SetDeltaZ(string z)
    {
        deltaRotation.z = System.Convert.ToSingle(z);
    }
}
