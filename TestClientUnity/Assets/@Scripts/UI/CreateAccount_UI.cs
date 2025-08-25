using Cysharp.Net.Http;
using Google.Protobuf.Protocol;
using Grpc.Core;
using Grpc.Net.Client;
using Mmorpg2d.Auth;
using Packet;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
public static class Authenticate 
{
    public static string Jwt = "";
    public static async Task<bool> DoCreateAccountAsync(Auth.AuthClient client, string id, string password)
    {
        try
        {
            var reply = await client.RegisterAsync(new RegisterRequest
            {
                Email = (id ?? "").Trim().ToLowerInvariant(),
                Password = password ?? ""
            });
            Debug.Log($"[���� ���] {reply.Success} / {reply.Detail}");
            return reply.Success;
        }
        catch (RpcException ex)
        {
            Debug.LogError($"[���� RPC ����] {ex.StatusCode} / {ex.Status.Detail}");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[���� ����] {ex.Message}");
            return false;
        }
    }
    public static async Task<(bool available, string detail)> CheckEmailAsync(Auth.AuthClient client, string email)
    {
        try
        {
            var reply = await client.CheckEmailAsync(
                new CheckEmailRequest { Email = (email ?? "").Trim().ToLowerInvariant() });

            return (reply.Available, reply.Detail);
        }
        catch (RpcException ex)
        {
            Debug.LogError($"[�ߺ�Ȯ�� RPC ����] {ex.StatusCode} / {ex.Status.Detail}");
            return (false, "���� ��� ����");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[�ߺ�Ȯ�� ����] {ex.Message}");
            return (false, "���� �߻�");
        }
    }
    public static async Task<(bool success, string detail, string jwt)> LoginAsync(Auth.AuthClient client, string id, string password)
    {
        try
        {
            var reply = await client.LoginAsync(new LoginRequest() 
            { 
                Email = id, 
                Password = password
            });
            Debug.Log("gRPC ����: " + reply.Success + "\ngRPC Detail" + reply.Detail + "\nJwt" + reply.Jwt);
              
            return (reply.Success,reply.Detail,reply.Jwt);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("gRPC ����: " + ex.Message);
            return (false,ex.Message,"");
        }
    }
}
    
public class CreateAccount_UI : MonoBehaviour
{
    private YetAnotherHttpHandler _handler;
    private GrpcChannel _channel;
    private Auth.AuthClient _client;
    private bool _checkID =false;
    #region InputFields
    [SerializeField] private TMP_InputField _idField;
    [SerializeField] private TMP_InputField _pwField;
    [SerializeField] private TMP_InputField _pwCheckField;
    #endregion
    #region Buttons
    [SerializeField] private Button _createAccountBtn;
    [SerializeField] private Button _ExitCreateAccountBtn;
    [SerializeField] private Button _idDuplicateCheckBtn;
    #endregion
    #region Panels
    [SerializeField] private GameObject _authorizePanel;
    #endregion
    #region TMP_Texts
    [SerializeField] private TMP_Text _idUseableText;
    [SerializeField] private TMP_Text _pwRecheckText;
    #endregion
    private void Awake()
    {

        if (_createAccountBtn) _createAccountBtn.onClick.AddListener(OnClickCreateAccount);
        if (_ExitCreateAccountBtn) _ExitCreateAccountBtn.onClick.AddListener(OnClickExitCreateAccount);
        if (_idDuplicateCheckBtn) _idDuplicateCheckBtn.onClick.AddListener(OnClickIdDuplicateCheck);
        if (_idField) _idField.onValueChanged.AddListener(OnIdChanged); // �Է� ��� �˻�
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
        InitializePanel();
        this.gameObject.SetActive(false);
    }
    private async void OnClickCreateAccount()
    {
        var id = _idField.text?.Trim();
        var pw = _pwField.text ?? "";
        var pw2 = _pwCheckField.text ?? "";

        // �Է� ����
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(pw2))
        {
            AuthNotice_UI.Instance.gameObject.SetActive(true);
            AuthNotice_UI.Instance.ShowNotice(NoticeCode.CreateAccountFail);
            return;
        }
        if (pw != pw2)
        {
            _pwRecheckText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            AuthNotice_UI.Instance.gameObject.SetActive(true);
            AuthNotice_UI.Instance.ShowNotice(NoticeCode.CreateAccountFail);
            return;
        }
        if (_checkID == false)
        {
            _idUseableText.text = "���̵� �ߺ� Ȯ���� �ʿ��մϴ�.";
            _idUseableText.color = Color.red;
            return;
        }
        _createAccountBtn.interactable = false;

        // ���� ���� ȣ��
        var ok = await Authenticate.DoCreateAccountAsync(_client, id, pw);

        // UI ������Ʈ
        AuthNotice_UI.Instance.gameObject.SetActive(true);
        AuthNotice_UI.Instance.ShowNotice(ok ? NoticeCode.CreateAccountSucess : NoticeCode.CreateAccountFail);

        _createAccountBtn.interactable = true;
    }
    void OnClickExitCreateAccount() // ȸ������ �г� ������
    {
        // ex_1 >> ���� â�� �����ųİ� ����� �ݱ�
        // ex_2 >> �ٷ� �ݱ�

        // �ϴ� �ٷ� �ݰ� �ٽ� �α��� �г� ���� �ɷ� ������.
        AuthNotice_UI.Instance.gameObject.SetActive(true);
        AuthNotice_UI.Instance.ShowNotice(NoticeCode.CheckExitCreateAccountPanel);
    }
    private bool IsValidEmail(string s)
    {
        try
        {
            var _ = new System.Net.Mail.MailAddress(s);
            return true;
        }
        catch { return false; }
    }
    private void OnIdChanged(string value)
    {
        var email = value?.Trim() ?? "";
        _checkID = IsValidEmail(email);

        // ���̵� �ٲ�� �ߺ�Ȯ�� ����� ��ȿȭ
        _checkID = false;
        if (_checkID == false)
        {
            _idUseableText.text = "ID�ߺ�Ȯ���� �ʿ��մϴ�.";
            _idUseableText.color= Color.red;
        }
    }
    private async void OnClickIdDuplicateCheck() //���̵� �ߺ�Ȯ�� ��ư �̺�Ʈ
    {
        var id = _idField.text?.Trim();
        if (string.IsNullOrEmpty(id))
        {
            _idUseableText.text = "���̵� �Է����ּ���!";
            _idUseableText.color = Color.red;
            _checkID = false;
            return;
        }
        else if (!IsValidEmail(id))
        {
            _idUseableText.text = "Email�������� �Է����ּ���!";
            _idUseableText.color = Color.red;
            _checkID = false;
        }
        else
        {
            var ok = await Authenticate.CheckEmailAsync(_client, _idField.text);
            if (ok.available)
            {
                _idUseableText.text = "��밡���� ID�Դϴ�!";
                _idUseableText.color = Color.green;
                _checkID = true;
            }
            else
            {
                _idUseableText.text = "�ߺ��� ID�Դϴ�!";
                _idUseableText.color = Color.red;
                _checkID = false;
            }
        }
        
    }
    void CheckPassword()
    {
        if (_pwField.text == _pwCheckField.text && _pwField.text != "") // ��й�ȣ�� ��й�ȣ ��Ȯ�� �ʵ��� �ؽ�Ʈ�� ��� ���� ������� �ʴٸ�
        {
            _pwRecheckText.text = "��й�ȣ�� ��ġ�մϴ�!";
            _pwRecheckText.color = Color.green;
        }
        else // ��й�ȣ�� ��й�ȣ ��Ȯ�� �ʵ��� ���� �ٸ���
        {
            _pwRecheckText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�."; 
            _pwRecheckText.color = Color.red;
        }
    }
    public void InitializePanel()
    {
        _pwRecheckText.text = "";
        _idUseableText.text = "";
        _idField.text = "";
        _pwField.text = "";
        _pwCheckField.text = "";
        _checkID = false;
    }
    
    private void Update()
    {
        if (_pwCheckField.text == "") // ��й�ȣüũ �Է��ʵ尡 ����ִٸ�
        {
            if (_pwField.text == "")
            {
                _pwRecheckText.text = "��й�ȣ�� �Է����ּ���.";
                _pwRecheckText.color = Color.white;
            }
            else
            {
                _pwRecheckText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
                _pwRecheckText.color = Color.red;
            }
        }
        else
        {
            if (_pwCheckField.text != "")
                CheckPassword();
        }
    }
}
