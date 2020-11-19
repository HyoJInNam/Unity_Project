using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform target;


    void Update()
    {
        transform.position = Offset(target.position);
    }
    
    private Vector3 Offset(Vector3 target)
    {
        target.y = transform.position.y;
        return target;
    }
}
