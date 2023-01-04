using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ButtonInput : MonoBehaviour
{
    public bool AButtonDown { get; private set; } = false;
    public bool XButtonDown { get; private set; } = false;

    public bool BButtonDown { get; private set; } = false;

    public bool YButtonDown { get; private set; } = false;

    private bool BDown = false;
    private bool ADown = false;
    private bool XDown = false;
    private bool YDown = false;

    private InputDevice RightDevice;
    private InputDevice LeftDevice;

    public static ButtonInput Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ButtonInput>();
                if (_instance == null)
                {
                    _instance = new GameObject("InputBridge").AddComponent<ButtonInput>();
                }
            }
            return _instance;
        }
    }

    private static ButtonInput _instance;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        List<InputDevice> devices2 = new List<InputDevice>();
        InputDeviceCharacteristics rightcharacteristic = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDeviceCharacteristics leftcharactersitic = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightcharacteristic, devices);
        InputDevices.GetDevicesWithCharacteristics(leftcharactersitic, devices2);

        if(devices.Count > 0)
        {
            RightDevice = devices[0];
        }

        if(devices2.Count > 0)
        {
            LeftDevice = devices2[0];
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(RightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool abutton))
        {
            if (ADown && abutton) { AButtonDown = false; return; }

            if (abutton)
            {
                

                if (!AButtonDown)
                {
                    AButtonDown = true;
                    ADown = true;
                }

            }
            else
            {
                ADown = false;
                
            }

        }

        if(LeftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool xbutton))
        {
            if (XDown && xbutton) { XButtonDown = false; return; }

            if (xbutton)
            {
                if (!XButtonDown)
                {
                    XButtonDown = true;
                    XDown = true;
                }
            }
            else
            {
                XDown = false;
            }

        }

        if (RightDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool bbutton))
        {
            if(BDown && bbutton) { BButtonDown = false; return; }

            if (bbutton)
            {
                if (!BButtonDown)
                {
                    BButtonDown = true;
                    BDown = true;
                    Debug.Log("Pressed");
                }
                
            }
            else
            {
                BDown = false;
                
            }

        }

        if (LeftDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool ybutton))
        {
            if (YDown && ybutton) { YButtonDown = false; return; }
            
            if (ybutton)
            {
                if (!YButtonDown)
                {
                    YButtonDown = true;
                    YDown = true;
                }
            }
            else
            {
                YDown = false;
            }

        }




    }


    
}
