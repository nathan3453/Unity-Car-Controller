using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchsCams : MonoBehaviour
{

    public Camera firstPersonCamera;
    public Camera overheadCamera;


    // Start is called before the first frame update
    void Start()
    {
        overheadCamera.enabled = true;
        firstPersonCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            if (overheadCamera.enabled == true)
            {
                overheadCamera.enabled = false;
                firstPersonCamera.enabled = true;
            }
            else 
            {
                overheadCamera.enabled = true;
                firstPersonCamera.enabled = false;
            }
        }
    }
}
