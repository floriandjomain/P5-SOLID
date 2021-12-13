using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SaveStruct
{
    string name;
    TransformStruct transformStruct;

    public SaveStruct(GameObject gameObject)
    {
        name = gameObject.name;
        transformStruct = new TransformStruct(gameObject.transform);
    }
}

struct TransformStruct
{
    float x;
    float y;
    float z;

    public TransformStruct(Transform transform)
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
    }
}