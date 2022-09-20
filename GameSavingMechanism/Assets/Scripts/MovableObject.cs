using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovableObject : MonoBehaviour
{

    private string obj_id;
    void Start()
    {
        obj_id = this.gameObject.name;
    }

    public string Obj_ID
    {
        get
        {
            return obj_id;
        }
    }
}

[System.Serializable]
public class MovableObjectData
{
    public string obj_id;
    public float[] obj_position;

    public MovableObjectData(string id, float[] position)
    {
        obj_id = id;
        obj_position = position;
    }
}