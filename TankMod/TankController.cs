using RedLoader;
using Sons.Ai.Vail;
using Sons.TerrainDetail;
using TheForest.Utils;
using UnityEngine;

namespace TankMod;

[RegisterTypeInIl2Cpp]
public class TankController : MonoBehaviour
{
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 45;
    public float steeringRangeAtMaxSpeed = 20;
    public float centreOfGravityOffset = -1f;

    WheelController[] wheels;
    Rigidbody rigidBody;

    Transform turret;
    Transform muzzle;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.drag = 1f;

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelController>();

        turret = transform.Find("turret");
        muzzle = turret.GetChild(0);
    }

    float smoothTime = 0.7f;
    float muzzleRotation;
    Vector3 velocity = Vector3.zero;

    float timer;

    void Update()
    {
        if (!TankMod.IsInTank)
            return;

        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        timer += Time.deltaTime;

        // shooting
        if (Physics.Raycast(muzzle.position, turret.forward + muzzle.forward, out RaycastHit hit, Mathf.Infinity) && Input.GetMouseButtonDown(0) && timer > 1.5f)
        {
            timer = 0;

            var explosion = Instantiate(TankExplosion.TankExplosionPrefab, hit.point, Quaternion.identity);

            var dist = Vector3.Distance(hit.point, transform.position);

            if (dist < 10)
            {
                LocalPlayer.HitReactions._cameraShakeController.TriggerShakeLarge();
            }
            else if (dist >= 10 && dist < 100)
            {
                LocalPlayer.HitReactions._cameraShakeController.TriggerShakeMedium();
            }
            else
            {
                LocalPlayer.HitReactions._cameraShakeController.TriggerShakeSmall();
            }

            AudioController.PlaySound("tankshoot", AudioController.SoundType.Sfx, false, 2);

            VailActorManager._instance.IgniteActorsInRadius(hit.point, 4, 0, false);
            VailActorManager._instance.DismemberActorsInRadius(hit.point, 4);
            VailActorManager._instance.KillActorsInRadius(hit.point, 4);
            TreeManager.SetWorldObjectStateInRadius(hit.point, 4, WorldObjectState.Destroyed);

            Destroy(explosion, 10f);
        }

        // remove trees in path
        TreeManager.SetWorldObjectStateInRadius(transform.position, 4, WorldObjectState.Cleared);

        // update engine sound
        float pitch = rigidBody.velocity.magnitude;
        TankMod.EngineCh.setPitch(Mathf.Clamp(pitch, .7f, 4f));

        // turret H rotation
        turret.localRotation = LocalPlayer.Transform.localRotation;

        // muzzle up-down rotation
        float y = Input.GetAxis("Mouse Y") * Time.deltaTime * 2;
        muzzleRotation -= y;
        muzzleRotation = Mathf.Clamp(muzzleRotation, -15, 10);
        muzzle.localRotation = Quaternion.Euler(Mathf.Clamp(muzzle.localRotation.x + muzzleRotation, -15, 10), muzzle.localRotation.y, muzzle.localRotation.z);

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);

        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }

}
