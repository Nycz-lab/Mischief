using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camScript : MonoBehaviour
{
    private Animator characterAnim;
    public float smoothing = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
        characterAnim = transform.parent.GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, characterAnim.GetBoneTransform(HumanBodyBones.Neck).position, smoothing);


        // TODO track rotation
        //Quaternion targetRot = Quaternion.Lerp(transform.rotation, characterAnim.GetBoneTransform(HumanBodyBones.Neck).rotation, smoothing);
        //Debug.Log("target " + targetRot.eulerAngles);
        //Debug.Log("Cam " + transform.eulerAngles);
        //transform.rotation = Quaternion.Euler(targetRot.eulerAngles.x, targetRot.eulerAngles.y + 90, targetRot.eulerAngles.z + 90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
