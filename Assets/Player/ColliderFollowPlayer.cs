using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollowPlayer : MonoBehaviour
{
    public Camera camera;
    public CharacterController collider;
    //private Vector3 tempPos;

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
        collider.height = camera.transform.position.y;
        //tempPos = new Vector3(camera.transform.localPosition.x, 0, camera.transform.localPosition.z); turning is way too sensitive
        //collider.center = tempPos;
    }
}
