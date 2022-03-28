using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public static bool TopDownActive = false;

    public GameObject camera1;
    public GameObject camera2;

    //Simple switch bool statement contantly checked,
    //this will ensure that the player can easily and quickly switch cameras.
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

    //These two function are just inverted versions of eachother,
    //they will disable one cameta whilst instantly enabling the other camera.
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
