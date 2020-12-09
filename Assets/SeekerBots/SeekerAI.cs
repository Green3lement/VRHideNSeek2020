using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SeekerAI : MonoBehaviour
{

    bool playerVisible;
    bool inPursuit;
    bool gameStart;
    public bool investigate;
    bool botOn = true;
    private GameObject player;
    public float startPosTime = 30f;
    public float wanderRadius;
    public float searchRadius;
    public float wanderTimer;
    public float searchSpotTime;
    public float investigateAreaTime;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float searchTime = 20f;
    public float investigateTime = 15f;
    public float countDown = 30f;
    public float stunTime = 7f;
    public Transform LastSeenLocation;
    [SerializeField] Transform startZone;
    [SerializeField] AudioSource alertNoise;
    [SerializeField] AudioSource pursuitNoise;
    [SerializeField] AudioSource endNoise;

    //public GameObject[] seekers;
    public NavMeshAgent agent;
    private float timer;
    private float searchTimer;
    private float stunTimer;
    private float investigateTimer;
    float FOV = 120f;
    float visibilityDistance = 100f;
    float gameOverDistance = 3f;
    float hearingDistance = 25f;
    bool playerFound = false;
    private Transform Glasses;
    public PlayerTimer playerTimer;

    // Use this for initialization
    void Start()
    {
        Glasses = GetComponentInChildren<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        playerTimer = GetComponent<PlayerTimer>();
        //seekers = GameObject.FindGameObjectsWithTag("Bots");
    }

    // Update is called once per frame
    void Update()
    {
        stunTimer += Time.deltaTime;
        searchTimer += Time.deltaTime;
        timer += Time.deltaTime;
        investigateTimer += Time.deltaTime;

        if (botOn) { 
            if(timer >= countDown  && !gameStart)
            {
                gameStart = true;
                agent.SetDestination(startZone.position);
                timer = 0;
            }

            if (gameStart)
            {
                playerVisible = CanSeePlayer();
                
                if (playerVisible)
                {
                    playerFound = PlayerFound();

                    if (alertNoise != null)
                    {
                        alertNoise.Play();
                    }
                    Chasing();

                    LastSeenLocation = player.transform;
                    inPursuit = true;
                    timer = 0;
                    searchTimer = 0;

                    if (playerFound)
                    {
                        if (endNoise != null)
                        {
                            endNoise.Play();
                        }
                        GetComponent<Renderer>().material.color = new Color(0, 0, 0);
                        StartCoroutine(WaitFiveCoroutine());
                        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                    }
                }
                else if (investigate)
                {
                    
                    if (timer >= investigateAreaTime)
                    {
                        Investigating();
                        timer = 0;
                    }

                    if (investigateTimer >= investigateTime)
                    {
                        investigate = false;
                    }
                }
                else if (inPursuit && !playerVisible)
                {
                    if (timer >= searchSpotTime)
                    {
                        Searching();
                        timer = 0;
                    }

                    if (searchTimer >= searchTime)
                    {
                        inPursuit = false;
                    }
                }
                else if (timer >= wanderTimer && !inPursuit && !playerVisible && (timer >= startPosTime || transform.position == startZone.position))
                {
                    startPosTime = -1;
                    Wandering();
                    timer = 0;
                }
            }   
        }
        else if (stunTimer >= stunTime)
        {
            agent.isStopped = false;
            botOn = true;
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
        if ((Vector3.Angle(rayDirection, Glasses.forward)) <= FOV * 0.5f)
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
    public void Chasing()
    {
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        agent.speed = chaseSpeed;
        agent.SetDestination(player.transform.position);
    }

    void Investigating()
    {
        GetComponent<Renderer>().material.color = Color.white;
        agent.speed = chaseSpeed;
        Vector3 investigatePos = RandomNavSphere(LastSeenLocation.position, searchRadius, -1);
        agent.SetDestination(investigatePos);
    }
    void Wandering()
    {
        GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        agent.speed = patrolSpeed;
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
    }
    void Searching()
    {
        GetComponent<Renderer>().material.color = new Color(0, 0, 255);
        agent.speed = chaseSpeed;
        Vector3 searchPos = RandomNavSphere(transform.position, searchRadius, -1);
        agent.SetDestination(searchPos);
    }
    void Stun()
    {
        GetComponent<Renderer>().material.color = new Color(255, 255, 0);
        agent.isStopped = true;
        botOn = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {            
            stunTimer = 0;
            Stun();
            NoiseRadius(collision.transform.position, hearingDistance);
            StartCoroutine(WaitFiveCoroutine());
            Destroy(collision.gameObject);
        }
    }

    private bool PlayerFound()
    {
        // The ray we'll try to cast later is the difference vector between the player and this enemy.
        var rayDirection = player.transform.position - transform.position;

        // Comparing sqr magnitudes will always be faster than performing a sqrt operation.
        if (rayDirection.sqrMagnitude > gameOverDistance * gameOverDistance)
        {
            return false;
        }
        if ((Vector3.Angle(rayDirection, Glasses.forward)) <= FOV * 0.5f)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, rayDirection, Color.blue);

            if (Physics.Raycast(transform.position, rayDirection, out hit, gameOverDistance))
            {
                return true;
            }
        }
        return false;

    }

  /*  private bool PlayerHeard(Transform noiseLocation)
    {
        // The ray we'll try to cast later is the difference vector between the player and this enemy.
        var rayDirection = noiseLocation.transform.position - transform.position;

        // Comparing sqr magnitudes will always be faster than performing a sqrt operation.
        if (rayDirection.sqrMagnitude > hearingDistance * hearingDistance)
        {
            return false;
        }
        
        RaycastHit hit;
        Debug.DrawRay(transform.position, rayDirection, Color.yellow);

        if (Physics.Raycast(transform.position, rayDirection, out hit, hearingDistance))
        {
            return true;
        }
        return false;

    }*/

    IEnumerator WaitFiveCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    void GoInvestigate(Vector3 noiseLocation)
    {
        LastSeenLocation.position = noiseLocation;
        investigate = true;
        investigateTimer = 0;
        if (pursuitNoise != null)
        {
            pursuitNoise.Play();
        }
    }

    void NoiseRadius(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        foreach (Collider hitCollider in hitColliders)
        {
            //SeekerAI seeker = hitCollider.gameObject.GetComponent<SeekerAI>();
            //seeker.GoInvestigate(center);
            hitCollider.SendMessage("GoInvestigate", center);
        }
    }
}
