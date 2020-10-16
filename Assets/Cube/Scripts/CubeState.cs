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

    [SerializeField]
    public List<GameObject> [] faces = new List<GameObject> [6];

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
