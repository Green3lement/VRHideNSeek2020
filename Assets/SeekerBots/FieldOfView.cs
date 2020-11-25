using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    //private Animator anim;
    float FOV = 90f;
    float visibilityDistance = 100f;
    private GameObject player;
    private float timer;
    // Start is called before the first frame update
    private void Start()
    {
        //anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        CanSeePlayer();
    }
    private bool CanSeePlayer()
    {
        // The ray we'll try to cast later is the difference vector between the player and this enemy.
        var rayDirection = player.transform.position - transform.position;

        // Comparing sqr magnitudes will always be faster than performing a sqrt operation.
        if (rayDirection.sqrMagnitude > visibilityDistance * visibilityDistance)
        {
            //anim.SetBool("playerVisible", false);
            return false;
        }

        // Is the angle between the difference vector and the eyes' forward vector within the field of view?
        // Make sure to have the Transform of the eyes or head saved so you don't cast the ray from the feet or wherever your pivot is later.
        if ((Vector3.Angle(rayDirection, transform.forward)) <= FOV * 0.5f)
        {
            RaycastHit hit;

            // Return true if player is within the field of view, false if obscured by something.
            if (Physics.Raycast(transform.forward, rayDirection, out hit, visibilityDistance))
            {
                // Put here whatever would identify a player from other GameObjects the ray would hit
                //anim.SetBool("playerVisible", true);
                return (hit.transform.root.CompareTag("Player"));
            }
        }
        //anim.SetBool("playerVisible", false);
        return false;
    }
}
