using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ObjectsData : UpdatableData {

    //public Objects[] objects;
    public Objects obj;

    public GameObject instantiate(float heightMultiplier, Vector3 position, Transform parent)
    {

        float minValue = obj.minHeight * heightMultiplier;
        float maxValue = obj.maxHeight * heightMultiplier;
        float value = position.y;
        //if (minValue < value && maxValue > value)
        {
            GameObject go = GameObject.Instantiate(obj.gameObejct, position, Quaternion.identity, parent);
            go.name = obj.name;
            go.transform.localScale = new Vector3(
                parent.localScale.x * obj.localScale.x,
                parent.localScale.y * obj.localScale.y,
                parent.localScale.z * obj.localScale.z);

            return go;
        }
    }

    //public List<GameObject> AppliedObjectsList(float height, float heightMultiplier)
    //{
    //    List<GameObject> go = new List<GameObject>();
    //    foreach (Objects obj in objects)
    //    {
    //        float minValue = obj.minHeight * heightMultiplier;
    //        float maxValue = obj.maxHeight * heightMultiplier;
    //        float value = height * heightMultiplier;

    //        //if (minValue < value && maxValue > value)
    //        {
    //            go.Add(obj.gameObejct);
    //        }
    //    }
    //    return go;
    //}

    [System.Serializable]
    public class Objects
    {
        public string name = "game object";
        public GameObject gameObejct;
        public Vector3 localScale = Vector3.one;

        public float minHeight;
        public float maxHeight;

        [Range(0, 1)]
        public const int numSupportedLODs = 1;
    }

}