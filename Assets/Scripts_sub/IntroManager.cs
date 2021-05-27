using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using NetCommon;

public class IntroManager : MonoBehaviour, ILog
{
    public string _deviceID;
    public DeviceType _deviceType;
    public string _serverIP;
    public int _port;

    public string _nextSeneName;
    [SerializeField]
    private long _PID;
    // Start is called before the first frame update
    void Start()
    {
        _deviceID = UnityEngine.SystemInfo.deviceUniqueIdentifier;
        _deviceType = UnityEngine.SystemInfo.deviceType;

        NetAPI.m_ILog = this;

        ClientAgent.Instance.EventOnConnected += OnConnected;
        ClientAgent.Instance.EventOnDisconnected += OnDisconnected;
        ClientAgent.Instance.EventOnLoginResult += OnLoginResult;

        ClientAgent.Instance.ConnectToServer(_serverIP, _port);
    }

    private void OnConnected(bool success)
    {
        Debug.Log("OnConnected : " + success.ToString());
        if (success)
        {
            ClientAgent.Instance.SendToServer(CommandFactory.Instance.GetLoginCommand(_deviceID));
        }
    }

    private void OnDisconnected()
    {
        Debug.Log("OnDisconnected");
    }

    private void OnLoginResult(UserInfo userInfo)
    {
        Debug.Log(string.Format("OnLoginResult PID : {0}", userInfo.PID));
        _PID = userInfo.PID;
        if (_PID != 0)
        {
            Debug.Log("StartCoroutine");
            StartCoroutine(MoveScene(_nextSeneName));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveScene(string moveSceneName)
    {
        Debug.Log("MoveScene : " + moveSceneName);
        AsyncOperation async = SceneManager.LoadSceneAsync(moveSceneName);
        async.allowSceneActivation = false;

        float progress = async.progress;
        while (progress < 0.9f)
        {
            Debug.Log(string.Format("progress : {0}", progress));
            yield return null;
            progress = async.progress;
        }
        yield return new WaitForSeconds(1.5f);
        async.allowSceneActivation = true;
    }

    public void Log(string msg)
    {
        Debug.Log(msg);
    }

    public void Log(string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }

    private void OnApplicationQuit()
    {
        ClientAgent.Instance.Disconnect(true);
    }
}
