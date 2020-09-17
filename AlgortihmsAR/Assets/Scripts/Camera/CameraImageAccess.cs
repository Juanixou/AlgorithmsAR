using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraImageAccess : MonoBehaviour
{
    #region PRIVATE_MEMBERS
    private PIXEL_FORMAT mPixelFormat = PIXEL_FORMAT.UNKNOWN_FORMAT;
    private bool mAccessCameraImage = true;
    private bool mFormatRegistered = false;
    #endregion // PRIVATE_MEMBERS

    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        #if UNITY_EDITOR
        mPixelFormat = PIXEL_FORMAT.GRAYSCALE; // Need Grayscale for Editor
        #else
        mPixelFormat = PIXEL_FORMAT.RGB888; // Use RGB888 for mobile
        #endif

        // Register Vuforia life-cycle callbacks:
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnPause);
    }
    #endregion // MONOBEHAVIOUR_METHODS

    #region PRIVATE_METHODS
    private void OnVuforiaStarted()
    {
        // Vuforia has started, now register camera image format  
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            //Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());
            mFormatRegistered = true;
        }
        else
        {
            //Debug.LogError(
            //  "Failed to register pixel format " + mPixelFormat.ToString() +
            //  "\n the format may be unsupported by your device;" +
            //  "\n consider using a different pixel format.");

            mFormatRegistered = false;
        }
    }


    /// 
    /// Called each time the Vuforia state is updated
    /// 
    void OnTrackablesUpdated()
{
    if (mFormatRegistered)
    {
        if (mAccessCameraImage)
        {
            Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
            if (image != null)
            {
                //Debug.Log(
                //    "\nImage Format: " + image.PixelFormat +
                //    "\nImage Size:   " + image.Width + "x" + image.Height +
                //    "\nBuffer Size:  " + image.BufferWidth + "x" + image.BufferHeight +
                //    "\nImage Stride: " + image.Stride + "\n"
                //);
                byte[] pixels = image.Pixels;
                if (pixels != null && pixels.Length > 0)
                {
                    //Debug.Log(
                    //    "\nImage pixels: " +
                    //    pixels[0] + ", " +
                    //    pixels[1] + ", " +
                    //    pixels[2] + ", ...\n"
                    //);
                }
            }
        }
    }
}
/// 
/// Called when app is paused / resumed
/// 
void OnPause(bool paused)
{
    if (paused)
    {
        Debug.Log("App was paused");
        UnregisterFormat();
    }
    else
    {
        Debug.Log("App was resumed");
        RegisterFormat();
    }
}
/// 
/// Register the camera pixel format
/// 
void RegisterFormat()
{
    if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
    {
        //Debug.Log("Successfully registered camera pixel format " + mPixelFormat.ToString());
        mFormatRegistered = true;
    }
    else
    {
        //Debug.LogError("Failed to register camera pixel format " + mPixelFormat.ToString());
        mFormatRegistered = false;
    }
}
/// 
/// Unregister the camera pixel format (e.g. call this when app is paused)
/// 
void UnregisterFormat()
{
    //Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
    CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
    mFormatRegistered = false;
}  
      #endregion //PRIVATE_METHODS
}
