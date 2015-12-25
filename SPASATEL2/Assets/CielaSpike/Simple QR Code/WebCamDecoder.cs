using UnityEngine;
using System.Collections;

#if NETFX_CORE
using Windows.Foundation;
using Windows.System.Threading;
#else
using System.Threading;
#endif

using CielaSpike.Unity.Barcode;

public class WebCamDecoder : MonoBehaviour
{
    private WebCamTexture _webCam;
    private IBarcodeDecoder _decoder;

    // set to true when web cam is playing.
    private volatile bool _needDecoding;

    private Cancel _cancel;

    private Color32[] _pixels;
    private int _width, _height;

    public DecodeResult Result { get; private set; }
    public DecodeOptions Options { get; set; }

    public System.Int32 DecodeIntervalMilliseconds = 100;

#if UNITY_EDITOR
    public bool ShowDebugMessage = false;
#endif

    public IEnumerator StartDecoding(WebCamTexture webCam)
    {
        _webCam = webCam;
        if (_cancel == null)
        {
            _cancel = new Cancel();
            _decoder = Barcode.GetDecoder(Options);
            _decoder.Options.TryHarder = true;

#if NETFX_CORE
            IAsyncAction asyncAction = ThreadPool.RunAsync(workItem =>
            {
                DecodingWorker(_cancel);
            });
#else
            ThreadPool.QueueUserWorkItem(DecodingWorker, _cancel);
#endif
        }

        while (!_webCam.isPlaying) yield return null;
    }

    public void StopDecoding()
    {
        if (_webCam != null)
        {
            _webCam.Stop();
        }

        _webCam = null;
        _decoder = null;

        if (_cancel != null)
        {
            _cancel.IsPending = true;
            _cancel = null;
        }
    }

    void Awake()
    {
        Result = DecodeResult.None;
        Options = new DecodeOptions();
    }

    void Update()
    {
        // no camera;
        if (_webCam == null) return;
        // or not playing;
        if (!_webCam.isPlaying) return;

        _pixels = _webCam.GetPixels32();
        _width = _webCam.width;
        _height = _webCam.height;

        _needDecoding = true;
    }

    void OnDestroy()
    {
        StopDecoding();
    }

    void DecodingWorker(object state)
    {
        var cancel = (Cancel)state;
        int errorCount = 0;
        while (!cancel.IsPending)
        {
            if (_needDecoding)
            {
                try
                {
                    Result = _decoder.Decode(_pixels, _width, _height);
#if UNITY_EDITOR
                    if (ShowDebugMessage)
                    {
                        if (Result.Success)
                        {
                            Debug.Log("Code: " + Result.Text);
                        }
                        else
                        {
                            Debug.Log("No Code Detected.");
                        }
                    }
#endif
                }
                catch (System.Exception ex)
                {
                    if (errorCount++ > 50)
                    {
                        Debug.LogException(ex);
                    }
                }
            }

#if NETFX_CORE
            using (var evt = new System.Threading.ManualResetEvent(false))
            { 
                evt.WaitOne(DecodeIntervalMilliseconds);
            }
#else
            Thread.Sleep(DecodeIntervalMilliseconds);
#endif
        }

#if UNITY_EDITOR
        if (ShowDebugMessage)
        {
            Debug.Log("Decoding Thread Exited.");
        }
#endif
    }

    private class Cancel
    {
        public volatile bool IsPending;
    }
}
