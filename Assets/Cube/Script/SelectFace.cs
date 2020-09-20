using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    CubeState cubeState;
    ReadCube readCube;

    private int layerMask = 1 << 8;
    //private int layerMask_cube = 1 << 9;

    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            readCube.ReadState();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                GameObject face = hit.collider.gameObject;
                List<GameObject>[] cubeSides = cubeState.faces;

                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        cubeState.PickUp(cubeSide);
                    }
                }
            }


            //if (Physics.Raycast(ray, out hit, 100.0f, layerMask_cube))
            //{
            //    GameObject cube = hit.collider.gameObject;
            //    cubeState.PickUp(cube.transform);
            //}
        }
    }
}
