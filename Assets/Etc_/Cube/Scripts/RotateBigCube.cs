using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBigCube : MonoBehaviour
{
    //Drag
    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    //Swipe
    Vector2 mDownPos = Vector3.zero;
    Vector2 mUpPos = Vector3.zero;
    Vector2 mGapPos = Vector3.zero;

    GameObject target;
    ReadCube readCube;

    float speed = 200.0f;

    void Start()
    {
        target = transform.parent.Find("target").gameObject;
        readCube = FindObjectOfType<ReadCube>();
    }

    void Update()
    {
        Swipe();
        Drag();
    }
    private void Drag()
    {
        if (Input.GetMouseButton(1))
        {
            mPosDelta = Input.mousePosition - mPrevPos;
            mPosDelta *= 0.1f;
            transform.rotation = Quaternion.Euler(mPosDelta.y, -mPosDelta.x, 0) * transform.rotation;
        }
        else if (transform.rotation != target.transform.rotation)
        {
            var step = speed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
        }
        mPrevPos = Input.mousePosition;
    }

     void Swipe()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mDownPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            mUpPos = Input.mousePosition;
            mGapPos = mDownPos - mUpPos;
            mGapPos.Normalize();
            if (Left())
            {
                target.transform.Rotate(0, -90, 0, Space.World);
            }
            else if (Right())
            {
                target.transform.Rotate(0, 90, 0, Space.World);
            }
            else if (UpLeft())
            {
                target.transform.Rotate(0, 0, 90, Space.World);
            }
            else if (DownLeft())
            {
                target.transform.Rotate(-90, 0, 0, Space.World);
            }
            else if (UpRight())
            {
                target.transform.Rotate(0, 0, -90, Space.World);
            }
            else if (DownRight())
            {
                target.transform.Rotate(90, 0, 0, Space.World);
            }
        }
    }

    bool Left()
    {
        return mGapPos.x < 0 && mGapPos.y > -0.5f && mGapPos.y < 0.5;
    }
    bool Right()
    {
        return mGapPos.x > 0 && mGapPos.y > -0.5f && mGapPos.y < 0.5;
    }
    bool UpLeft()
    {
        return mDownPos.x < (Screen.width / 2) && mGapPos.y > 0f;
    }
    bool UpRight()
    {
        return mDownPos.x > (Screen.width / 2) && mGapPos.y > 0f;
    }
    bool DownLeft()
    {
        return mDownPos.x < (Screen.width / 2) && mGapPos.y < 0f;
    }
    bool DownRight()
    {
        return mDownPos.x > (Screen.width / 2) && mGapPos.y < 0f;
    }
}
