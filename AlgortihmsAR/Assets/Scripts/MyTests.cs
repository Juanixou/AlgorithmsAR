using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class MyTests : MonoBehaviour
{

    public Camera normalCamera;
    public CameraDevice vuforiaCamera;
    public GameObject img;
    WebCamTexture camText;

    private bool mAccessCameraImage = true;
    private bool mFormatRegistered = false;
    private PIXEL_FORMAT mPixelFormat = PIXEL_FORMAT.UNKNOWN_FORMAT;

    private void Start()
    {
        if (camText == null) camText = new WebCamTexture();

        img.GetComponent<Renderer>().material.mainTexture = camText;

#if UNITY_EDITOR
        mPixelFormat = PIXEL_FORMAT.GRAYSCALE; // Need Grayscale for Editor
#else
        mPixelFormat = PIXEL_FORMAT.RGB888; // Use RGB888 for mobile
#endif

        //camText.Play();
    }


    public void ButtonFunction()
    {
        //Texture2D tex = GetImage();
        
        img.GetComponent<Renderer>().material.mainTexture = GetImage();

    }

    public Texture2D GetImage()
    {
        
        Texture2D texture = VuforiaRTImage(CameraDevice.Instance);
        return texture;

    }

    // Take a "screenshot" of a camera's Render Texture.
    Texture2D RTImage(Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        //image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }

    Texture2D VuforiaRTImage(CameraDevice camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.

        // Render the camera's view.
        Vuforia.Image im = CameraDevice.Instance.GetCameraImage(mPixelFormat);
        if (im == null) return null;

        RectTransform cubeRec = img.GetComponent<RectTransform>();
        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(im.Width, im.Height, TextureFormat.RGB24, false);
        Vector2 scale = TransformImageToPixel();

        //image.ReadPixels(new Rect(0,0,cubeRec.rect.size.x, cubeRec.rect.size.y), 0, 0);
        image.ReadPixels(new Rect(0,0,im.Width, im.Height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        return image;
    }


    Vector2 TransformImageToPixel()
    {
        Vector2 sizeDelta = img.GetComponent<RectTransform>().sizeDelta;
        GameObject canvas = img.transform.parent.gameObject;
        Vector2 canvasScale = new Vector2(canvas.transform.localScale.x, canvas.transform.localScale.y);

        Vector2 finalScale = new Vector2(sizeDelta.x * canvasScale.x, sizeDelta.y * canvasScale.y);
        return finalScale;

    }

    }
