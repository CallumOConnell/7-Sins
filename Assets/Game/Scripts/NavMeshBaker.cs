using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface[] _navMeshSurfaces;

    private void Awake()
    {
        _navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();

        foreach (var surface in _navMeshSurfaces)
        {
            surface.BuildNavMesh();
        }
    }
}