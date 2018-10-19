using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class DoNotSleep : MonoBehaviour
{
    public bool PreventDeviceFromSleeping;

#if ENABLE_WINMD_SUPPORT
    private Windows.System.Display.DisplayRequest _displayRequest;
#endif

    // Use this for initialization
    void Start()
    {
        if (PreventDeviceFromSleeping)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

#if ENABLE_WINMD_SUPPORT
            //create the request instance if needed
            if (_displayRequest == null)
                _displayRequest = new Windows.System.Display.DisplayRequest();
 
            //make request to put in active state
            _displayRequest.RequestActive();
#endif
        }
    }
}
