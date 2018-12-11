using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GazeGestureManager : MonoBehaviour {

    public static GazeGestureManager Instance { get; private set; }
    public GameObject FocusedObject { get; private set; }
    public GameObject TextViewPrefab;
    public AudioClip captureAudioClip;
    public AudioClip failedAudioClip;

    UnityEngine.XR.WSA.Input.GestureRecognizer gestureRecognizer;
    PhotoInput photoInput;
    QrDecoder qrDecoder;
    AudioSource captureAudioSource;
    AudioSource failedAudioSource;

    void Awake () {
        Instance = this;
        photoInput = GetComponent<PhotoInput>();
        gestureRecognizer = new UnityEngine.XR.WSA.Input.GestureRecognizer();
        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;
        gestureRecognizer.StartCapturingGestures();
        qrDecoder = gameObject.AddComponent<QrDecoder>();
	}

    void Start() {
        captureAudioSource = gameObject.AddComponent<AudioSource>();
        captureAudioSource.clip = captureAudioClip;
        captureAudioSource.playOnAwake = false;
        failedAudioSource = gameObject.AddComponent<AudioSource>();
        failedAudioSource.clip = failedAudioClip;
        failedAudioSource.playOnAwake = false;
    }

    private void Update() {
    }

    void GestureRecognizer_TappedEvent(UnityEngine.XR.WSA.Input.InteractionSourceKind source, int tapCount, Ray headRay) {
        photoInput.CapturePhotoAsync(onPhotoCaptured);
    }

    void onPhotoCaptured(List<byte> image, int width, int height) {
        string val = qrDecoder.Decode(image.ToArray(), width, height);
        Debug.Log(val);
        if (val != null) {
            showText(val);
            captureAudioSource.Play();
        } else {
            failedAudioSource.Play();
        }
    }

    void showText(string text) {
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;
        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
            var obj = Instantiate(TextViewPrefab, hitInfo.point, Quaternion.identity);
            var textMesh = obj.GetComponent<TextMesh>();

            //Set your actions here after getting your values
            if (text == "1" || text == "HelloHolo")
            {
                textMesh.color = Color.red;
            }
            else {
                textMesh.color = Color.green;
            }
            textMesh.text = text;
        }
    }
}
