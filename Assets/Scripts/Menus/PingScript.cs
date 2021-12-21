using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PingScript : MonoBehaviour
{
    public Text txt;
    public string text;
    private string holdValue;

    public int avgFrameRate;
    [SerializeField] private float _hudRefreshRate = 1f;
    private float _timer;

    // Start is called before the first frame update
    void Awake()
    {
        txt = GetComponent<Text>();
        text = txt.text;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            txt.text = "FPS: " + fps;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
        /*
        holdValue = PhotonNetwork.GetPing().ToString();
        if (CompareText(holdValue))
        {
            txt.text = text;
        }
        */
    }

    bool CompareText(string other)
    {
        if (!other.Equals(text))
        {
            text = other;
            return true;
        }
        return false;
    }
}
