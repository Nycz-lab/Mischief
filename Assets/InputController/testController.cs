using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class testController : MonoBehaviour
{

    private Rigidbody rb;
    private PlayerInputActions playerActions;
    private Animator characterAnim;
    public float speed = 1.0f;
    public float smoothing = 5.0f;
    public float maxAngle = 50.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerActions = new PlayerInputActions();
        playerActions.Player.Enable();
        characterAnim = GetComponentInChildren<Animator>();
        //Debug.Log(characterAnim.GetBoneTransform(HumanBodyBones.Neck));

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // look around

        Vector2 viewVec = playerActions.Player.Look.ReadValue<Vector2>();
        viewVec.y *= -1;


        Vector3 currentRotation = transform.rotation.eulerAngles;

        // Set the new X rotation angle (in degrees)
        float newYRotation = currentRotation.y + viewVec.x;

        // Create a new Vector3 object to store the new rotation
        Vector3 newRotation = Vector3.Lerp(currentRotation, new Vector3(currentRotation.x, newYRotation, currentRotation.z), smoothing);

        // Set the transform's rotation to the new Euler angles
        transform.rotation = Quaternion.Euler(newRotation);

        Transform cam = Camera.main.transform;
        Debug.Log(cam.rotation.eulerAngles.x + viewVec.y - 180 + " " + Mathf.Clamp(cam.rotation.eulerAngles.x + viewVec.y - 180, -50, 50));
        //Debug.Log(Mathf.Clamp(cam.rotation.eulerAngles.x + viewVec.y, -50.0f, 50.0f));
        Quaternion targetRot = Quaternion.Lerp(cam.rotation, Quaternion.Euler(new Vector3(cam.rotation.eulerAngles.x + viewVec.y, cam.rotation.eulerAngles.y, cam.rotation.eulerAngles.z)), smoothing);
        cam.rotation = targetRot;

        Vector3 camRot = cam.rotation.eulerAngles;


        Vector2 moveVec = playerActions.Player.Movement.ReadValue<Vector2>();
        characterAnim.SetBool("isMoving", moveVec.magnitude > 0);                          // transition to walking/running animation

        rb.AddForce(cam.rotation * new Vector3(moveVec.x, 0, moveVec.y) * speed * Time.deltaTime, ForceMode.VelocityChange);     // move character


    }

}
