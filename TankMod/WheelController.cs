using UnityEngine;
using RedLoader;

namespace TankMod;

[RegisterTypeInIl2Cpp]
public class WheelController : MonoBehaviour
{
    public Transform wheelModel;
    public WheelCollider WheelCollider;

    public bool steerable;
    public bool motorized;

    Vector3 position;
    Quaternion rotation;

    private void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    void Update()
    {
        WheelCollider.GetWorldPose(out position, out rotation);
        wheelModel.transform.position = position;
        wheelModel.transform.rotation = rotation;
    }

}
