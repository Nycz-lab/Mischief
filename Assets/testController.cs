using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class testController : MonoBehaviour
{

    private Rigidbody rb;
    private PlayerInputActions playerActions;
    public float speed = 1.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerActions = new PlayerInputActions();
        playerActions.Player.Enable();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector2 inputVec = playerActions.Player.Movement.ReadValue<Vector2>();
        rb.AddForce(new Vector3(inputVec.x, 0, inputVec.y) * speed, ForceMode.Impulse);
    }

}
