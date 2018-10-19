using UnityEngine;
#if WINDOWS_UWP
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.ExtendedExecution; 
using Windows.UI.Core;
#endif

public class RunInBackgroundExtended : MonoBehaviour
{
#if WINDOWS_UWP
    private ExtendedExecutionSession session = null;
#endif

    public bool ExtendExecutionInBackground = true;

    // Use this for initialization
    void Start()
    {
        if (ExtendExecutionInBackground)
        {
#if WINDOWS_UWP
      BeginExtendedExecution();      
#endif
        }
    }

#if WINDOWS_UWP

    private async void BeginExtendedExecution()
    {
        // The previous Extended Execution must be closed before a new one can be requested.
        // This code is redundant here because the sample doesn't allow a new extended
        // execution to begin until the previous one ends, but we leave it here for illustration.
        ClearExtendedExecution();

        var newSession = new ExtendedExecutionSession();
        newSession.Reason = ExtendedExecutionReason.Unspecified;
        newSession.Description = "Raising periodic toasts";
        newSession.Revoked += SessionRevoked;
        ExtendedExecutionResult result = await newSession.RequestExtensionAsync();

        switch (result)
        {
            case ExtendedExecutionResult.Allowed:
                session = newSession;
                Debug.Log("Extended Execution started succesfully");        
                break;

            default:
            case ExtendedExecutionResult.Denied:
                newSession.Dispose();
                Debug.Log("Extended Execution denied!");        
                break;
        }
    }

    void ClearExtendedExecution()
    {
        if (session != null)
        {
            session.Revoked -= SessionRevoked;
            session.Dispose();
            session = null;
        }
    }

    private async void SessionRevoked(object sender, ExtendedExecutionRevokedEventArgs args)
    {
        Debug.Log("Extended Execution Ended: " + args.Reason);        
        EndExtendedExecution();
    }

    private void EndExtendedExecution()
    {
        ClearExtendedExecution();
    }
#endif

}
