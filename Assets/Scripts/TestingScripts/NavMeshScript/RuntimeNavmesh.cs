using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
public class RuntimeNavmesh : MonoBehaviour
{
    public NavMeshSurface navmesh;
    void Start()
    {
        navmesh.BuildNavMesh();
    }

    
}
