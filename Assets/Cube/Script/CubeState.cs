using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    enum CUBEDIRECTION {
        UP = 0,
        LEFT,
        FRONT,
        RIGHT,
        BACK,
        DOWN,
        COUNT
    }

    //private List<Transform> cubes = new List<Transform> ();
    //private List<Transform> cubeX = new List<Transform>();
    //private List<Transform> cubeY = new List<Transform>();
    //private List<Transform> cubeZ = new List<Transform>();


    public List<GameObject> [] faces = new List<GameObject> [6];

    //private void Start()
    //{
    //    for(int i = 0; i < transform.childCount; i++)
    //    {
    //        cubes.Add(transform.GetChild(i));
    //    }
    //}

    //public void PickUp(Transform cube)
    //{
    //    foreach (Transform c in cubes)
    //    {
    //        if (c.position.x == cube.position.x) cubeX.Add(c);
    //        if (c.position.y == cube.position.y) cubeY.Add(c);
    //        if (c.position.z == cube.position.z) cubeZ.Add(c);
    //    }
    //    cube.GetComponent<PivotRotation>().Rotate();
    //}

    public void PickUp(List<GameObject> cubeSide)
    {
        foreach (GameObject faces in cubeSide)
        {
            if (faces != cubeSide[4])
            {
                faces.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }
        }

        cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
    }
    
    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach(GameObject littleCube in littleCubes)
        {
            if(littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }
}
