using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float walkSpeed = 8f;
    public float sprint = 14f;
    public float maxVelocityChange = 10.0f;
    public float jump = 5f;
    public float airControl = 0.5f;


    private bool sprinting;
    private bool grounded;
    private bool jumping;

    private Vector2 input;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Normalize(); 

        sprinting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");
    }

    private void OnTriggerStay(Collider other){
       
            grounded = true;
 
    }

    void FixedUpdate(){
        if (grounded){
           
            if (jumping){
                rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);

            }else if (input.magnitude > 0.5f){
                rb.AddForce(CalculateMovement(sprinting ? sprint : walkSpeed), ForceMode.VelocityChange); //if else shorthand method
            }
            else{
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f*Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }else{
            if (input.magnitude > 0.5f){
                rb.AddForce(CalculateMovement(sprinting ? sprint * airControl : walkSpeed*airControl), ForceMode.VelocityChange); //if else shorthand method
            }
            else{
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f*Time.fixedDeltaTime, velocity1.y, velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }
        
        grounded = false;
        
    }

    

    Vector3 CalculateMovement(float _speed){
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _speed;

        if (input.magnitude > 0.5f){
            Vector3 velocityChange = targetVelocity - rb.velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            return velocityChange;

        }else{
            return new Vector3();
        }

        
    }
}
