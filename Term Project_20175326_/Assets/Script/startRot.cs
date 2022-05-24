using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startRot : MonoBehaviour
{
    public float rotSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(rotSpeed * Time.deltaTime / 0.01f, 0, 0));
    }

}
