using UnityEngine;
using System.Collections;

using CielaSpike.Unity.Barcode;
using System.Threading;

public class ExampleBarcodeController : MonoBehaviour
{
    WebCamTexture cameraTexture;

    Material cameraMat;
    GameObject plane;

    WebCamDecoder decoder;

    IBarcodeEncoder qrEncoder, pdf417Encoder;

    GUIContent qrImage = new GUIContent();
    GUIContent pdf417Image = new GUIContent();

    GUIContent resultString = new GUIContent();

    Vector2 scroll = Vector2.zero;

    IEnumerator Start()
    {
        // get render target;
        plane = GameObject.Find("Plane");
        cameraMat = plane.GetComponent<MeshRenderer>().material;

        // get a reference to web cam decoder component;
        decoder = GetComponent<WebCamDecoder>();

        // get encoders;
        qrEncoder = Barcode.GetEncoder(BarcodeType.QrCode, new QrCodeEncodeOptions()
            {
                ECLevel = QrCodeErrorCorrectionLevel.H
            });

        pdf417Encoder = Barcode.GetEncoder(BarcodeType.Pdf417);

        qrEncoder.Options.Margin = 1;
        pdf417Encoder.Options.Margin = 2;

        // init web cam;
        if (Application.platform == RuntimePlatform.OSXWebPlayer ||
            Application.platform == RuntimePlatform.WindowsWebPlayer)
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }

        var devices = WebCamTexture.devices;
        var deviceName = devices[0].name;
        cameraTexture = new WebCamTexture(deviceName, 1920, 1080);
        cameraTexture.Play();

        // start decoding;
        yield return StartCoroutine(decoder.StartDecoding(cameraTexture));

        cameraMat.mainTexture = cameraTexture;

        // adjust texture orientation;
        plane.transform.rotation = plane.transform.rotation * 
            Quaternion.AngleAxis(cameraTexture.videoRotationAngle, Vector3.up);
    }

    void OnGUI()
    {
        // draw web cam on screen;
        var width = Screen.width - 20;

        // get decode result;
        var result = decoder.Result;
        bool decoded = false;
        if (result.Success && resultString.text != result.Text)
        {
            resultString.text = result.Text;
            decoded = true;
            Debug.Log(string.Format(
				"Decoded: [{0}]{1}", result.BarcodeType, result.Text));
        }

        // draw decoded content (can edit);
        var inputRect = new Rect(
            10,
            Screen.height * 0.6f,
            width,
            GUI.skin.label.CalcHeight(resultString, width));

        var input = GUI.TextArea(inputRect, resultString.text);

        // encode content above into textures;
        if (resultString.text != input || decoded)
        {
            resultString.text = input;

            if (!string.IsNullOrEmpty(input))
            {
                var qrResult = qrEncoder.Encode(input);
                var pdf417Result = pdf417Encoder.Encode(input);

                if (qrResult.Success)
                {
                    qrImage.image = qrResult.GetTexture();
                }
                else
                {
                    Debug.LogError(qrResult.ErrorMessage);
                }

                if (pdf417Result.Success)
                {
                    pdf417Image.image = pdf417Result.GetTexture();
                }
                else
                {
                    Debug.LogError(pdf417Result.ErrorMessage);
                }
            }
        }

        // draw textures;

        GUILayout.BeginArea(new Rect(
            10,
            inputRect.yMax + 10,
            inputRect.width,
            Screen.height - (inputRect.yMax + 20)));

        scroll = GUILayout.BeginScrollView(scroll, GUILayout.Width(width), GUILayout.ExpandHeight(true));

        GUILayout.Label(qrImage, GUILayout.MaxWidth(width - 20));
        GUILayout.Space(10);
        GUILayout.Label(pdf417Image, GUILayout.MaxWidth(width - 20));

        GUILayout.EndScrollView();

        GUILayout.EndArea();
    }
}
