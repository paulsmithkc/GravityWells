using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public new Rigidbody rigidbody = null;
    //public PlanetGravitySource planetGravity = null;
    public float cameraYawSensitivity = 60;
    public float forwardSpeed = 0.0f;
    public float horizonalSpeed = 0.0f;
    public float backwardSpeed = 0.0f;
    public float jetpackSpeed = 0.0f;

    private const float maxAccel = 1000.0f;
    public Vector3 currentMoveVelocity = Vector3.zero;
    private Vector3 currentMoveAccel = Vector3.zero;
    public float currentTurnVelocity = 0;
    private float currentTurnAccel = 0;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        //planetGravity = FindObjectOfType<PlanetGravitySource>();
        currentMoveVelocity = Vector3.zero;
        currentMoveAccel = Vector3.zero;
        currentTurnVelocity = 0;
        currentTurnAccel = 0;
    }
	
	// Update is called once per frame
	void Update () {
        float deltaTime = Time.deltaTime;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float cameraYaw = Input.GetAxis("Camera Yaw");
        
        float targetTurnVelocity = cameraYaw * cameraYawSensitivity;
        currentTurnVelocity = Mathf.SmoothDamp(currentTurnVelocity, targetTurnVelocity, ref currentTurnAccel, 0.15f, maxAccel, deltaTime);

        Vector3 targetVelocity = new Vector3(h * horizonalSpeed, 0, v * (v >= 0 ? forwardSpeed : backwardSpeed));
        currentMoveVelocity = Vector3.SmoothDamp(currentMoveVelocity, targetVelocity, ref currentMoveAccel, 0.15f, maxAccel, deltaTime);
    }

    void FixedUpdate() {
        float deltaTime = Time.fixedDeltaTime;
        //if (planetGravity != null)
        //{
        //    Vector3 planetUp = (this.transform.position - planetGravity.transform.position).normalized;
        //    rigidbody.rotation = Quaternion.FromToRotation(transform.up, planetUp) * rigidbody.rotation;
        //}
        rigidbody.MoveRotation(Quaternion.AngleAxis(currentTurnVelocity * deltaTime, transform.up) * rigidbody.rotation);
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(currentMoveVelocity) * deltaTime);

        float cameraPitch = Input.GetAxis("Camera Pitch");
        rigidbody.AddForce(transform.up * cameraPitch * jetpackSpeed, ForceMode.Acceleration);
    }
    
    void OnDrawGizmos()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * h);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.up * v);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}
