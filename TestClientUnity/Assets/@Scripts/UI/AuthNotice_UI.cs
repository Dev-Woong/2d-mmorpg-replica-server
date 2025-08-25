using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum NoticeCode
{
    
    CheckExitCreateAccountPanel=0,
    CreateAccountFail,
    CreateAccountSucess, 
    LoginFailNullID ,
    LoginFailNullPW,
    LoginFailNullAccount,
    DoLogin,
    LoginSuccess,
    RecvCharacterListSuccess,
}

public class AuthNotice_UI : MonoBehaviour
{
    public static AuthNotice_UI Instance;
    [SerializeField] private TMP_Text _noticeText;
    [SerializeField] private Button _checkBtn;
    [SerializeField] private Button _closePanelBtn;
    [SerializeField] private GameObject _createAccountPanel;
    [SerializeField] private GameObject _authorizePanel;
    [SerializeField] private NoticeCode _noticeCode;
    private Coroutine _coNoticeActiveTrue;
    private WaitForSeconds _interval = new WaitForSeconds(1f);
    public TMP_Text ChangeNoticeCode(NoticeCode notiCode)
    {
        _noticeCode = notiCode;
        switch (_noticeCode)
        {
            case NoticeCode.CheckExitCreateAccountPanel:
                _noticeText.text = "������ ���� ������ ���߽ð� �α��� ȭ������\n���ư��ðڽ��ϱ�?";
                _noticeText.color = Color.red;
                ShowCloseButton();
                break;

            case NoticeCode.CreateAccountSucess:
                _noticeText.text = "���� ������ �Ϸ�Ǿ����ϴ�.\nüũ ��ư�� �����ø� �α��� ȭ������ ���ư��ϴ�.";
                _noticeText.color = Color.white;
                break;

            case NoticeCode.CreateAccountFail:
                _noticeText.text = "���� ������ �ʿ��� ������ �������� �ʾҽ��ϴ�.\n�ٽ� �õ����ּ���.";
                _noticeText.color = Color.red;
                break;
            case NoticeCode.LoginFailNullID:
                _noticeText.text = "���̵� �Է����ּ���.";
                _noticeText.color = Color.red;
                break;

            case NoticeCode.LoginFailNullPW:
                _noticeText.text = "��й�ȣ�� �Է����ּ���";
                _noticeText.color = Color.red;
                break;

            case NoticeCode.DoLogin:
                _noticeText.text = "�α����� �������Դϴ�. ��ø� ��ٷ��ּ���.";
                _noticeText.color = Color.white;
                break;

            case NoticeCode.LoginFailNullAccount:
                _noticeText.text = "�������� �ʴ� �����Դϴ�.\nȸ�������� �����ϰų� �α��� ������ �ٽ� �Է����ּ���.";
                _noticeText.color = Color.red;
                break;

            case NoticeCode.LoginSuccess:
                _noticeText.text = "�α��ο� �����ϼ̽��ϴ�.\nĳ���� ������ �ҷ����� �ֽ��ϴ�.";
                _noticeText.color = Color.white;
                break;
            case NoticeCode.RecvCharacterListSuccess:
                _noticeText.text = "ĳ���� ����Ʈ�� ���������� �ҷ��Խ��ϴ�.";
                _noticeText.color = Color.white;
                break;
        }
        return _noticeText;
    }
    private void OnClickCheck()
    {
        if (_noticeCode == NoticeCode.LoginFailNullID)
        {
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.LoginFailNullPW)
        {
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.LoginFailNullAccount)
        {
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.LoginSuccess)
        {
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.CheckExitCreateAccountPanel)
        {
            _authorizePanel.SetActive(true);
            _createAccountPanel.SetActive(false);
            _createAccountPanel.GetComponent<CreateAccount_UI>().InitializePanel();
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.CreateAccountSucess)
        {
            _createAccountPanel.SetActive(false);
            _authorizePanel.SetActive(true);
            _noticeText.text = "";
            _createAccountPanel.GetComponent<CreateAccount_UI>().InitializePanel();
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.CreateAccountFail)
        {
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.RecvCharacterListSuccess)
        {
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
    }
    IEnumerator PanelSetActiveTrue(NoticeCode code)
    {
        yield return _interval;
        this.gameObject.SetActive(true);
        ChangeNoticeCode(code);
    }
    private void ShowCloseButton()
    {
        _closePanelBtn.enabled = true;
        _closePanelBtn.image.color = Color.white;
    }
    public void ShowNotice(NoticeCode code)
    {
        _coNoticeActiveTrue = StartCoroutine(PanelSetActiveTrue(code));
    }
    private void OnClickClose()
    {
        _noticeText.text = "";
        _authorizePanel.SetActive(true);
        _closePanelBtn.enabled = false;
        _closePanelBtn.image.color = new Color(0, 0, 0, 0);
        this.gameObject.SetActive(false);
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        if (_checkBtn) _checkBtn.onClick.AddListener(OnClickCheck);
        if (_closePanelBtn) _closePanelBtn.onClick.AddListener(OnClickClose);
        _noticeText.text = "";
        _closePanelBtn.enabled = false;
        _closePanelBtn.image.color = new Color(0,0,0,0);
        this.gameObject.SetActive(false);   
    }
}
