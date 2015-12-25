using UnityEngine;
using System.Collections;

public class GetScreen : MonoBehaviour
{

    Color[] colors;
    //public Transform[] _probes;
    public Vector3[] ProbePosition;

    private static GetScreen _instance;
    public static GetScreen Instance
    {
        get { return _instance; }
    }

    public Texture2D ScreenTexture
    {
        get
        {
            if (texturePrepared)
                return _texture;
            else
                return null;
        }
    }
    Texture2D _texture = null;

    [HideInInspector]
    public bool needRenderScreen
    {
        set { _need_render_screen = value; if (value)texturePrepared = false; }
        private get { return _need_render_screen; }
    }
    bool _need_render_screen;

    [HideInInspector]
    public bool texturePrepared
    {
        get { return _texture_prepared; }
        private set { _texture_prepared = value; }
    }
    bool _texture_prepared = false;

    void Awake()
    {
        _instance = this;
    }



    public void OnPostRender()
    {
        if (needRenderScreen)
        {
            /*
            ProbePosition = new Vector3[_probes.Length];

            for (int i = 0; i < _probes.Length; i++)
            {
                ProbePosition[i] = this.GetComponent<Camera>().WorldToViewportPoint(_probes[i].position);
                ProbePosition[i].x *= Screen.width;
                ProbePosition[i].y = Screen.height - Screen.height * ProbePosition[i].y;
                //ProbePosition[i].z=0;

                if (ProbePosition[i].x < 0 || ProbePosition[i].x > Screen.width || ProbePosition[i].y < 0 || ProbePosition[i].y > Screen.height)
                {
                    needRenderScreen = true;
                    return;
                }
            }
            */

            needRenderScreen = false;
            _texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            _texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
            _texture.Apply();
            StartCoroutine(executeReading());
        }
    }

    IEnumerator executeReading()
    {


        colors = new Color[ProbePosition.Length / 3];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = ColorRecognizer.GetBlurColor(_texture, new Vector3[] { ProbePosition[i * 3], ProbePosition[i * 3 + 1], ProbePosition[i * 3 + 2], }, Screen.height);
            Debug.Log("" + colors[i]);
        }
        texturePrepared = true;

        yield return null;
    }

    public Color[] GetColors()
    {
        if (texturePrepared)
        {
            return colors;
        }
        else return null;

    }

}
