using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverWheelIK : MonoBehaviour {

    public float maxWheelAscent = 0.2f;
    public float maxWheelDrop = 1.0f;
    public Wheel[] wheels = new Wheel[0];
    
    [System.Serializable]
    public class Wheel
    {
        public Transform transform;
        [HideInInspector]
        public Vector3 currentVelocity;
    }

	// Use this for initialization
	void Start() {
        Vector3 bodyPos = transform.position;
        foreach (var w in wheels)
        {
            w.currentVelocity = Vector3.zero;
        }
	}
	
	// Update is called once per frame
	void Update() {
        float deltaTime = Time.deltaTime;
        Vector3 bodyPos = transform.position;
        Vector3 up = transform.up;
        int planetMask = (1 << LayerMask.NameToLayer("Planet")) | (1 << LayerMask.NameToLayer("Default"));

        foreach (var w in wheels)
        {
            Vector3 wheelPos = w.transform.position;
            Vector3 wheelCenter = wheelPos - Vector3.Project(wheelPos, up) + Vector3.Project(bodyPos, up);
            Vector3 wheelAscent = wheelCenter + up * maxWheelAscent;
            Vector3 wheelTarget;
            RaycastHit hit;
            if (Physics.Raycast(wheelAscent, -up, out hit, (maxWheelAscent + maxWheelDrop), planetMask, QueryTriggerInteraction.Ignore))
            {
                wheelTarget = hit.point;
                //Debug.LogFormat("{0} {1}", wheelPos, wheelTarget);
            }
            else
            {
                wheelTarget = wheelCenter - up * maxWheelDrop;
            }
            wheelPos = Vector3.SmoothDamp(wheelPos, wheelTarget, ref w.currentVelocity, 0.10f, 100, deltaTime);
            w.transform.position = wheelPos;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 bodyPos = transform.position;
        Vector3 up = transform.up;

        Gizmos.color = Color.red;
        foreach (var w in wheels)
        {
            if (w.transform != null)
            {
                Vector3 wheelPos = w.transform.position;
                Vector3 wheelCenter = wheelPos - Vector3.Project(wheelPos, up) + Vector3.Project(bodyPos, up);
                Gizmos.DrawLine(wheelCenter + up * maxWheelAscent, wheelCenter - up * maxWheelDrop);
            }
        }
    }
}
