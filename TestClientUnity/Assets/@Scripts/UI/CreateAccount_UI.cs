using System.Collections;
using System.Net.Sockets;
using System.Security;
using TMPro;
using Packet;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf.Protocol;
using Mmorpg2d.Auth;

public class CreateAccount_UI : MonoBehaviour
{
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
        if (_idDuplicateCheckBtn) _idDuplicateCheckBtn.onClick.AddListener(OnClickIdDuplicateCheck);
    }
    void OnClickCreateAccount() // ���� ��������
    {
        // TODO >> ���� ���� ����
        
        // ���� ���� ������ �����ߴٸ�  >>
        if (_idField.text == "" || _pwField.text == "" || _pwCheckField.text == "" /* bool=> id�ߺ�Ȯ�� ���н�*/)
        {
            _noticePanel.SetActive(true);
            _noticePanel.GetComponent<Notice_UI>().ChangeNoticeCode(NoticeCode.CreateAccountFail);
        }
        else if(_idField.text == "" || _pwField.text == "" || _pwCheckField.text == ""/*bool=> id�ߺ�Ȯ�� ������*/)
        {
            _noticePanel.SetActive(true);
            _noticePanel.GetComponent<Notice_UI>().ChangeNoticeCode(NoticeCode.CreateAccountSucess);
        }

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
    void OnClickIdDuplicateCheck() //���̵� �ߺ�Ȯ�� ��ư �̺�Ʈ
    {
        if (_idField.text == "") // ���̵� �Է��ʵ尡 ���������
        {
            _idUseableText.text = "���̵� �Է����ּ���!";
            _idUseableText.color = Color.red;
        }
        else
        {
            string id = _idField.text.Trim();
            string password = _pwField.text;

            var pkt = new RegisterRequest
            {
                Email = id,
                Password = password
            };
            var sendBuffer = ServerPacketManager.MakeSendBuffer(pkt);
            NetworkManager.Instance.Send(sendBuffer);
        }
        // TODO >> �Է¹��� ���̵� ��밡�� ���ο� ���� ���̵� �ߺ� Ȯ�� �ؽ�Ʈ ����� ���� >> �������� ���۹޾ƾ��� ��������.
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
    void InitializePanel()
    {
        _pwRecheckText.text = "";
        _idUseableText.text = "";
        _idField.text = "";
        _pwField.text = "";
        _pwCheckField.text = "";
    }
    void Start()
    {
        InitializePanel();
        this.gameObject.SetActive(false);
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
