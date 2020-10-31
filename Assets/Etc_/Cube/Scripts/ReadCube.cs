using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ReadCube : MonoBehaviour
{
    private GameObject emptyGO;
    private GameObject faceGO;
    public Transform[] faces = new Transform[6];
    List<GameObject>[] rays = new List<GameObject>[6];

    private int layerMask = 1 << 8;
    CubeState cubeState;
    CubeMap cubeMap;
    
    void Start()
    {
        SetFaceGameObecjts();
        SetRayTransforms();
        ReadState();
    }

    public void ReadState()
    {
        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();
        for (int i = 0; i < 6; i++) {
            cubeState.faces[i] = ReadFace(rays[i], faces[i]);
        }
        cubeMap.Set();
    }
    void SetFaceGameObecjts()
    {
        faceGO = transform.parent.Find("Ray").gameObject;

        for (int i = 0; i < faceGO.transform.childCount; i++){
            faces[i] = faceGO.transform.GetChild(i);
        }
    }

    void SetRayTransforms()
    {
        emptyGO = transform.parent.Find("RayStart").gameObject;

        Vector3[] VECTOR_DIR = new Vector3[] {
             new Vector3(90, 0, 0),
             new Vector3(0, 90, 0),
             new Vector3(0, 0, 0),
             new Vector3(0, -90, 0),
             new Vector3(0, 180, 0),
             new Vector3(-90, 0, 0)
         };
        
        for (int i = 0; i < faces.Length; i++){
            rays[i] = BuildRays(faces[i], VECTOR_DIR[i]);
        }
    }

    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();
        for (int y = -1; y <= 1; y++){
            for (int x = -1; x <= 1; x++){
                Vector3 startPos = new Vector3(
                                        rayTransform.localPosition.x + x,
                                        rayTransform.localPosition.y + y,
                                        rayTransform.localPosition.z + rayTransform.localPosition.normalized.z * x);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = "(" + x + ", " + y + ")";
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();
        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }
        return facesHit;
    }
}
