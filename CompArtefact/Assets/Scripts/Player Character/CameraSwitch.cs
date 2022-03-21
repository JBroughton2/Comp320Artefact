using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public static bool TopDownActive = false;

    public GameObject camera1;
    public GameObject camera2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!TopDownActive)
            {
                SwitchToTop();
            }
            else
            {
                SwitchToFP();
            }
        }
    }

    private void SwitchToTop()
    {
        camera1.SetActive(false);
        camera2.SetActive(true);
        TopDownActive = true;
    }

    private void SwitchToFP()
    {
        camera1.SetActive(true);
        camera2.SetActive(false);
        TopDownActive = false;
    }
}
