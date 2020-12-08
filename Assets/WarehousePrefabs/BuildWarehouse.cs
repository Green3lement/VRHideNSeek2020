using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildWarehouse : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        navMeshSurface.BuildNavMesh();
    }
}
