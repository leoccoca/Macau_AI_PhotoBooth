using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Webcam : MonoBehaviour
{
    public static Webcam instance;

    [SerializeField] RawImage cameraImage;
    [SerializeField] RawImage captureImage;
    [SerializeField] RawImage finalPosterImage;

    [SerializeField] Camera captureCamera;
    [SerializeField] Camera liveCamera;
    [SerializeField] Camera outputCamera;
    [SerializeField] Camera finalPosterCamera;

    [SerializeField] List<Image> photoFrames;

    WebCamTexture webCamTexture;
    const int WEBCAM_RESOLUTION_WIDTH = 1920;
    const int WEBCAM_RESOLUTION_HEIGHT = 1080;

    public Texture LiveCameraTexture
    {
        get { return liveCamera.targetTexture; }
    }

    public RawImage FinalPosterImage { get => finalPosterImage; }

    public Transform CaptureImageTrans => captureImage.transform;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple singleton object created, gameobject: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AssignWebCam();
    }

    void AssignWebCam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        string deviceName = "";

        foreach (WebCamDevice device in devices)
        {
            Debug.Log("Webcam device name: " + device.name);

            if (device.name.Contains(ConfigManager.Instance.clientConfig.webCamName))
            {
                deviceName = device.name;
                break;
            }
        }

        if (deviceName == "")
        {
            Debug.LogError("Webcam device cannot be found!");
            DebugManager.Instance.IsCameraConnected = false;
            return;
        }

        Debug.Log("Using webcam: " + deviceName);
        DebugManager.Instance.IsCameraConnected = true;
        webCamTexture = new WebCamTexture(deviceName, WEBCAM_RESOLUTION_WIDTH, WEBCAM_RESOLUTION_HEIGHT, 60);
    }

    public void SetupWebcam()
    {
        if (webCamTexture != null)
        {
            cameraImage.texture = webCamTexture;
            OnPlay();
        }
    }

    void OnPlay()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Play();
        }
    }

    void OnPause()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Pause();
        }
    }

    public void OnStop()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }
    }

    void OnApplicationQuit()
    {
        OnStop();
    }

    public Texture2D CapturePoster()
    {
        Camera camera;
        int rtWidth;
        int rtHeight;

        camera = finalPosterCamera;
        rtWidth = 896;
        rtHeight = 1344;


        RenderTexture rt = new RenderTexture(rtWidth, rtHeight, 24);
        camera.targetTexture = rt;

        camera.Render();
        RenderTexture.active = rt;

        Texture2D image = new Texture2D(rtWidth, rtHeight, TextureFormat.ARGB32, false);
        image.ReadPixels(new Rect(0, 0, rtWidth, rtHeight), 0, 0);

        Color[] pixels = image.GetPixels();

        image.SetPixels(pixels);
        image.Apply();

        RenderTexture.active = null;

        camera.targetTexture.Release();
        camera.targetTexture = null;

        Destroy(rt);
        Capture(image);

        return image;
    }

    public Texture2D CapturePhoto(bool isFinalOutput = false)
    {
        Camera camera;
        int rtWidth;
        int rtHeight;

        if (isFinalOutput)
        {
            camera = outputCamera;
            rtWidth = 512;
            rtHeight = 768;
        } else
        {
            camera = captureCamera;
            rtWidth = 1920;
            rtHeight = 1080;
        }


        RenderTexture rt = new RenderTexture(rtWidth, rtHeight, 24);
        camera.targetTexture = rt;

        camera.Render();
        RenderTexture.active = rt;

        Texture2D image = new Texture2D(rtWidth, rtHeight, TextureFormat.ARGB32, false);
        image.ReadPixels(new Rect(0, 0, rtWidth, rtHeight), 0, 0);

        Color[] pixels = image.GetPixels();

        image.SetPixels(pixels);
        image.Apply();

        RenderTexture.active = null;

        camera.targetTexture.Release();
        camera.targetTexture = null;

        Destroy(rt);

        if (!isFinalOutput )
        {
            Capture(image);
        }
        return image;
    }

    void Capture(Texture2D texture)
    {
        captureImage.texture = texture;
    }

    public void SetFrame(int frameid)
    {
        FrameReset();
        var SelectedFrame = photoFrames.Find(x => x.name == frameid.ToString());
        SelectedFrame.gameObject.SetActive(true);
    }

    void FrameReset()
    {
        foreach (var frame in photoFrames)
        {
            frame.gameObject.SetActive(false);
        }
    }
}
