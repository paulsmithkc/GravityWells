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
    public float engineAccelTime = 0.15f;
    public float engineVolume = 1.0f;
    public float enginePitchMin = 1f;
    public float enginePitchMax = 6f;
    public float enginePitchMultiplier = 1f;
    public float highPitchMultiplier = 0.25f; 

    public AudioClip lowAccelClip;
    public AudioClip lowDecelClip;
    public AudioClip highAccelClip;
    public AudioClip highDecelClip;
    private AudioSource _lowAccelSource;
    private AudioSource _lowDecelSource;
    private AudioSource _highAccelSource;
    private AudioSource _highDecelSource;

    private const float maxAccel = 1000.0f;
    public Vector3 currentMoveVelocity = Vector3.zero;
    private Vector3 currentMoveAccel = Vector3.zero;
    public float currentTurnVelocity = 0;
    private float currentTurnAccel = 0;
    public Inputs inputs = new Inputs();

    [System.Serializable]
    public struct Inputs
    {
        public bool allowPlayerControl;
        public float Horizontal;
        public float Vertical;
        public float CameraYaw;
        public float CameraPitch;
    }

    // Use this for initialization
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        //planetGravity = FindObjectOfType<PlanetGravitySource>();
        currentMoveVelocity = Vector3.zero;
        currentMoveAccel = Vector3.zero;
        currentTurnVelocity = 0;
        currentTurnAccel = 0;

        _lowAccelSource = SetUpEngineAudioSource(lowAccelClip);
        _lowDecelSource = SetUpEngineAudioSource(lowDecelClip);
        _highAccelSource = SetUpEngineAudioSource(highAccelClip);
        _highDecelSource = SetUpEngineAudioSource(highDecelClip);
    }

    // Update is called once per frame
    void Update() {
        if (inputs.allowPlayerControl)
        {
            inputs.Horizontal = Input.GetAxis("Horizontal");
            inputs.Vertical = Input.GetAxis("Vertical");
            inputs.CameraYaw = Input.GetAxis("Camera Yaw");
            inputs.CameraPitch = Input.GetAxis("Camera Pitch");
        }

        float deltaTime = Time.deltaTime;
        float h = inputs.Horizontal;
        float v = inputs.Vertical;

        float targetTurnVelocity = inputs.CameraYaw * cameraYawSensitivity;
        currentTurnVelocity = Mathf.SmoothDamp(currentTurnVelocity, targetTurnVelocity, ref currentTurnAccel, 0.15f, maxAccel, deltaTime);

        Vector3 targetVelocity = new Vector3(h * horizonalSpeed, 0, v * (v >= 0 ? forwardSpeed : backwardSpeed));
        currentMoveVelocity = Vector3.SmoothDamp(currentMoveVelocity, targetVelocity, ref currentMoveAccel, engineAccelTime, maxAccel, deltaTime);

        UpdateEngineSound(v);
    }

    void UpdateEngineSound(float acceleratorInput)
    {
        // The pitch is interpolated between the min and max values, according to the car's revs.
        float revs = currentMoveAccel.magnitude / 100;
        float pitch = (1.0f - revs) * enginePitchMin + revs * enginePitchMax;
        // clamp to minimum pitch (note, not clamped to max for high revs while burning out)
        pitch = Mathf.Min(enginePitchMax, pitch);
        
        // adjust the pitches based on the multipliers
        _lowAccelSource.pitch = pitch * enginePitchMultiplier;
        _lowDecelSource.pitch = pitch * enginePitchMultiplier;
        _highAccelSource.pitch = pitch * enginePitchMultiplier * highPitchMultiplier;
        _highDecelSource.pitch = pitch * enginePitchMultiplier * highPitchMultiplier;

        // get values for fading the sounds based on the acceleration
        float accFade = Mathf.Abs(acceleratorInput);
        float decFade = 1 - accFade;

        // get the high fade value based on the cars revs
        float highFade = Mathf.InverseLerp(0.2f, 0.8f, revs);
        float lowFade = 1 - highFade;

        // adjust the values to be more realistic
        highFade = 1 - ((1 - highFade) * (1 - highFade));
        lowFade = 1 - ((1 - lowFade) * (1 - lowFade));
        accFade = 1 - ((1 - accFade) * (1 - accFade));
        decFade = 1 - ((1 - decFade) * (1 - decFade));

        // adjust the source volumes based on the fade values
        _lowAccelSource.volume = lowFade * accFade * engineVolume;
        _lowDecelSource.volume = lowFade * decFade * engineVolume;
        _highAccelSource.volume = highFade * accFade * engineVolume;
        _highDecelSource.volume = highFade * decFade * engineVolume;
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
        
        rigidbody.AddForce(transform.up * inputs.CameraPitch * jetpackSpeed, ForceMode.Acceleration);
    }

    // sets up and adds new audio source to the gane object
    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        // create the new audio source component on the game object and set up its properties
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;

        // start the clip from a random point
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = 50;
        source.dopplerLevel = 0;
        return source;
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
