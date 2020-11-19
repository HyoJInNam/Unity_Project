using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap : MonoBehaviour
{
    CubeState cubeState;
    public Transform[] face = new Transform[6];
    private void Awake()
    {
        face[0] = transform.Find("up");
        for (int i = 0; i < 4; ++i)
            face[i + 1] = transform.Find("side").GetChild(i);
        face[5] = transform.Find("down");
    }

    public void Set()
    {
        cubeState = FindObjectOfType<CubeState>();
        for (int i = 0; i < face.Length; ++i)
            UpdateMap(cubeState.faces[i], face[i]);
    }
    void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;
        foreach(Transform map in side)
        {
            if (face[i].tag.Equals("Empty"))
                map.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            if (face[i].tag.Equals("Lake"))
                map.GetComponent<Image>().color = new Color(0, 0, 1, 1);
            i++;
        }
    }
}
