using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SeekerAI : MonoBehaviour
{
    
    bool playerVisible;
    bool inPursuit;
    private GameObject player;
    public float wanderRadius;
    public float searchRadius;
    public float wanderTimer;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float searchTimer = 50f;
    private Transform LastSeenLocation;

    
    private Transform target;
    public NavMeshAgent agent;
    private float timer;
    float FOV = 180f;
    float visibilityDistance = 100f;
 
    // Use this for initialization
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerVisible = CanSeePlayer();
        timer += Time.deltaTime;
        if(playerVisible)
        {
            Chasing();
            GetComponent<Renderer>().material.color = new Color(255, 0, 0);
            LastSeenLocation.position = player.transform.position;
            inPursuit = true;
            timer = 0;
        }
        else if(!playerVisible && inPursuit)
        {
            Searching();
            GetComponent<Renderer>().material.color = new Color(0, 0, 255);
            if (timer >= searchTimer)
            {
                inPursuit = false;
            }
        }       
        else if (timer >= wanderTimer)
        {
            GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            Wandering();
            timer = 0;
        }
    }

    /// 
    /// 
    /// 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)//, Animator anim)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        

        return navHit.position;
    }

    private bool CanSeePlayer()
    {
        // The ray we'll try to cast later is the difference vector between the player and this enemy.
        var rayDirection = player.transform.position - transform.position;

        // Comparing sqr magnitudes will always be faster than performing a sqrt operation.
        if (rayDirection.sqrMagnitude > visibilityDistance * visibilityDistance)
        {
            return false;
        }

        // Is the angle between the difference vector and the eyes' forward vector within the field of view?
        // Make sure to have the Transform of the eyes or head saved so you don't cast the ray from the feet or wherever your pivot is later.
        if ((Vector3.Angle(rayDirection, transform.forward)) <= FOV * 0.5f)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, rayDirection, Color.red);
            
            // Return true if player is within the field of view, false if obscured by something.
            if (Physics.Raycast(transform.position, rayDirection, out hit, visibilityDistance))
            {
                Debug.Log("What it hit: " + hit.transform.position);
                Debug.Log("HIT! " + hit.transform.root.CompareTag("Player"));
                // Put here whatever would identify a player from other GameObjects the ray would hit
                return (hit.transform.root.CompareTag("Player"));
                
            }
        }       
        return false;
    }
    void Chasing()
    {
        agent.speed = chaseSpeed;
        Debug.Log("Chasing: " + agent);
        agent.SetDestination(player.transform.position);
        Debug.Log("Destination: " + player.transform.position);
    }
    void Wandering()
    {
        agent.speed = patrolSpeed;
        Debug.Log("Wandering: " + agent);
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
        Debug.Log("Destination: " + newPos);
    }
    void Searching()
    {
        Debug.Log("Searching: " + agent);
        Vector3 newPos = RandomNavSphere(LastSeenLocation.position, searchRadius, -1);
        agent.SetDestination(newPos);
        Debug.Log("Destination: " + newPos);
    }

}
