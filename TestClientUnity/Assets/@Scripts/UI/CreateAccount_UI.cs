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
public static class Authenticate
{
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
    [SerializeField] private GameObject _noticePanel; 
    #endregion
    #region TMP_Texts
    [SerializeField] private TMP_Text _idUseableText;
    [SerializeField] private TMP_Text _pwRecheckText;
    #endregion
    private void Awake()
    {

        if (_createAccountBtn) _createAccountBtn.onClick.AddListener(OnClickCreateAccount);
        if (_ExitCreateAccountBtn) _ExitCreateAccountBtn.onClick.AddListener(OnClickExitCreateAccount);
        //if (_idDuplicateCheckBtn) _idDuplicateCheckBtn.onClick.AddListener(OnClickIdDuplicateCheck);
    }
    void Start()
    {
        _handler = new YetAnotherHttpHandler { Http2Only = true };
        _channel = GrpcChannel.ForAddress("http://182.231.5.187:8080",
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
            ShowNotice(NoticeCode.CreateAccountFail);
            return;
        }
        if (pw != pw2)
        {
            _pwRecheckText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            ShowNotice(NoticeCode.CreateAccountFail);
            return;
        }
        if (_checkID == false)
        {
            _idUseableText.text = "���̵� �ߺ� Ȯ���� �ʿ��մϴ�.";
        }
        _createAccountBtn.interactable = false;

        // ���� ���� ȣ��
        var ok = await Authenticate.DoCreateAccountAsync(_client, id, pw);

        // UI ������Ʈ
        ShowNotice(ok ? NoticeCode.CreateAccountSucess : NoticeCode.CreateAccountFail);

        _createAccountBtn.interactable = true;
    }
    private void ShowNotice(NoticeCode code)
    {
        _noticePanel.SetActive(true);
        _noticePanel.GetComponent<Notice_UI>()?.ChangeNoticeCode(code);
    }
    void OnClickExitCreateAccount() // ȸ������ �г� ������
    {
        // ex_1 >> ���� â�� �����ųİ� ����� �ݱ�
        // ex_2 >> �ٷ� �ݱ�

        // �ϴ� �ٷ� �ݰ� �ٽ� �α��� �г� ���� �ɷ� ������.
        InitializePanel();
        _authorizePanel.SetActive(true);
        this.gameObject.SetActive(false); 
    }
    //private async void OnClickIdDuplicateCheck() //���̵� �ߺ�Ȯ�� ��ư �̺�Ʈ
    //{
    //}
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
    void InitializePanel()
    {
        _pwRecheckText.text = "";
        _idUseableText.text = "";
        _idField.text = "";
        _pwField.text = "";
        _pwCheckField.text = "";
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
