using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class car_movement : MonoBehaviour
{

// Public

    public Rigidbody rb;
    public float mspeed = 3500f;
    public float breakforce = 1000f;
    public float speedkmph;
    public bool hbrake_on_off;
    public float steeringAngle = 60f;

// Private

    float speedmps;
    string speedkmph_string;
    float moveinput;
    float steerinput;
    bool isrev;
    string dir;
    bool brakesinput;
    float dotP;

// Classes

    public WheelColliders colliders;
    public WheelMeshes wheel_meshes;
    public TMProText TMPro_text;


    // Start is called before the first frame update
    void Start()
    {

    }

// Gets keyboard inputs
    void getinputs()
    {
        moveinput = Input.GetAxis("Vertical");
        steerinput = Input.GetAxis("Horizontal");


    }

// Logic for wheel rotation
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;

    }

// Makes Wheels rotate
    void UpdateWheelMesh()
    {
        UpdateWheel(colliders.FR, wheel_meshes.FR);
        UpdateWheel(colliders.FL, wheel_meshes.FL);
        UpdateWheel(colliders.RR, wheel_meshes.RR);
        UpdateWheel(colliders.RL, wheel_meshes.RL);
        
    }

//Make car move by adding torque to the wheels
    void move()
    {
        colliders.RL.motorTorque = moveinput * mspeed;
        colliders.RR.motorTorque = moveinput * mspeed; 
    }

// Car steering
    void steering()
    {

        
        colliders.FL.steerAngle = steeringAngle * steerinput;
        colliders.FR.steerAngle = steeringAngle * steerinput;

    }

// Check if H has been pressed
   void HandleHandBrake()
    {
        if (Input.GetKeyDown("h"))
        {
            hbrake_on_off = !hbrake_on_off;

        }
    }

// Apply the handbrake to the car
    void ApplyHandBrake()
    {
        if (hbrake_on_off)
        {
            colliders.RL.brakeTorque = breakforce * 100000000f;
            colliders.RR.brakeTorque = breakforce * 100000000f;
        }
        
    }


// Checks what direction the car is moving to know when to apply brakes
    void dir_check()
    {
        dotP = Vector3.Dot(transform.forward.normalized, rb.velocity.normalized);
        if(dotP > 0.5f)
        {
            if (Input.GetKey("s"))
            {
                brakesinput = true;
            }
            else if (Input.GetKey("w"))
            {
                brakesinput = false;
            }
            else
            {
                brakesinput = false;
            }
            
            dir = "forward";
        }
        else if(dotP < -0.5f)
        {
            if (Input.GetKey("w"))
            {
                brakesinput = true;
            }
            else if (Input.GetKey("s"))
            {
                brakesinput = false;
            }
            else
            {
                brakesinput = false;
            }
            isrev = true;
            dir = "reverse";
        }
        else
        {
            isrev = false;
            brakesinput = false;
            dir = "sliding sideways";
        }
    }

// Applys brakes if trying to stop the car
    void Brakes()
    {
        if (brakesinput)
        {
            colliders.RL.brakeTorque = breakforce * mspeed;
            colliders.RR.brakeTorque = breakforce * mspeed;
            colliders.FL.brakeTorque = breakforce * mspeed;
            colliders.FR.brakeTorque = breakforce * mspeed;
        }
        
        if (brakesinput == false)
        {
            if (hbrake_on_off == false)
            {
                colliders.RL.brakeTorque = 0;
                colliders.RR.brakeTorque = 0;
                colliders.FL.brakeTorque = 0;
                colliders.FR.brakeTorque = 0;
            }
            
             
        }
    }

// Gets the speed in km
    void SpeedKm()
    {
        Vector3 velocity = rb.velocity;
        speedmps = velocity.magnitude;
        speedkmph = speedmps * 3.6f;
        speedkmph_string = speedkmph.ToString("0");

    }

// Limits the speed when reversing
    void limter()
    {
        if (isrev)
        {
            if (speedkmph >= 20f)
            {
                colliders.RL.brakeTorque = 100 * mspeed;
                colliders.RR.brakeTorque = 100 * mspeed;
            }
        }
    }

// Displays speed and handbrake status to screen
    void UIText()
    {
        TMPro_text.Speed.text = $"Speed:{speedkmph_string}";
        
        if (hbrake_on_off)
        {
            TMPro_text.Handbrake.text = "HandBrake: ON";
        }
        else
        {
            TMPro_text.Handbrake.text = "HandBrake: OFF";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        getinputs();
        move();
        steering();
        UpdateWheelMesh();
        HandleHandBrake();
        ApplyHandBrake();
        SpeedKm();
        UIText();
        Brakes();
        limter();
        
    }

// For physic related updates
    void FixedUpdate()
    {
        dir_check();
    }


// Classes for getting the wheel colliders, wheel meshes and TMP text

    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider RL;
        public WheelCollider RR;
        public WheelCollider FL;
        public WheelCollider FR;
    }

    [System.Serializable]
    public class WheelMeshes
    {
        public MeshRenderer RL;
        public MeshRenderer RR;
        public MeshRenderer FL;
        public MeshRenderer FR;
    }
    
    [System.Serializable]
    public class TMProText
    {
        public TMP_Text Speed;
        public TMP_Text Handbrake;
        
    }

}

