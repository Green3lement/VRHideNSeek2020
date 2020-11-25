using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollowPlayer : MonoBehaviour
{
    public Camera camera;
    CharacterController collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = camera.transform.position - this.transform.position;
        temp.y = collider.transform.position.y;
        collider.height = temp.y;
        
    }
}
