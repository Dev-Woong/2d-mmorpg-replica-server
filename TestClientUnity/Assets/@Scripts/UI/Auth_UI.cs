using Cysharp.Net.Http;
using Grpc.Net.Client;
using Mmorpg2d.Auth;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.UI;
public class Auth_UI : MonoBehaviour
{
    private YetAnotherHttpHandler _handler;
    private GrpcChannel _channel;
    private Auth.AuthClient _client;
    public static string Jwt = "";
    #region InputFields
    [SerializeField] private TMP_InputField _idField;
    [SerializeField] private TMP_InputField _pwField;
    #endregion

    #region Buttons
    [SerializeField] private Button _createAccountBtn;
    [SerializeField] private Button _loginBtn;
    #endregion

    #region Panels 
    [SerializeField] private GameObject _createAccountPanel;
    [SerializeField] private GameObject _noticePanel;
    #endregion
    private void Awake()
    {
        if (_createAccountBtn) _createAccountBtn.onClick.AddListener(OnClickCreateAccount);
        if (_loginBtn) _loginBtn.onClick.AddListener(OnClickLogin);
    }
    void Start()
    {
        _handler = new YetAnotherHttpHandler { Http2Only = true };
        _channel = GrpcChannel.ForAddress("http://127.0.0.1:8080",
            new GrpcChannelOptions
            {
                HttpHandler = _handler,
                DisposeHttpClient = true
            });
        _client = new Auth.AuthClient(_channel);
    }
    void InitializeInputField()
    {
        _idField.text = "";
        _pwField.text = "";
    }
    void OnClickCreateAccount()
    {
        InitializeInputField();
        _createAccountPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void ShowNotice(NoticeCode code)
    {
        _noticePanel.SetActive(true);
        _noticePanel.GetComponent<Notice_UI>()?.ChangeNoticeCode(code);
    }
    private async void OnClickLogin()
    {
        var id = _idField.text?.Trim();
        var password = _pwField.text ?? "";
        if (string.IsNullOrEmpty(id)) // 아이디 입력필드가 비워져있을때
        {
            ShowNotice(NoticeCode.LoginFailNullID);
            return;
        }
        if (string.IsNullOrEmpty(password)) // 패스워드 입력필드가 비워져있을때
        {
            ShowNotice(NoticeCode.LoginFailNullPW);
            return;
        }
        ShowNotice(NoticeCode.DoLogin);
        var loginReply = await Authenticate.LoginAsync(_client, id, password);
        if (loginReply.success)
        {
            Jwt = loginReply.jwt;
            ShowNotice(NoticeCode.LoginSuccess);
        }
        else
        {
            ShowNotice(NoticeCode.LoginFailNullAccount); 
        }

    }
}
