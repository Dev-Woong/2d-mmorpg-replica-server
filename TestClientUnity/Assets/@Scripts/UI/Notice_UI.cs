using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NoticeCode
{
    LoginFailNullID = 0,
    LoginFailNullPW,
    DoLogin,
    LoginFailNullAccount,
    LoginSuccess,
    CheckExitCreateAccountPanel,
    CreateAccountFail,
    CreateAccountSucess 
}

public class Notice_UI : MonoBehaviour
{
    [SerializeField] private TMP_Text _noticeText;
    [SerializeField] private Button _checkBtn;
    [SerializeField] private Button _closePanelBtn;
    [SerializeField] private GameObject _createAccountPanel;
    [SerializeField] private GameObject _authorizePanel;
    [SerializeField] private NoticeCode _noticeCode;

    public TMP_Text ChangeNoticeCode(NoticeCode notiCode)
    {
        _noticeCode = notiCode;
        switch (_noticeCode)
        {
            case NoticeCode.LoginFailNullID:
                _noticeText.text = "���̵� �Է����ּ���.";
                break;

            case NoticeCode.LoginFailNullPW:
                _noticeText.text = "��й�ȣ�� �Է����ּ���";
                break;

            case NoticeCode.DoLogin:
                _noticeText.text = "�α����� �������Դϴ�. ��ø� ��ٷ��ּ���.";
                break;

            case NoticeCode.LoginFailNullAccount:
                _noticeText.text = "�α��ο� �����߽��ϴ�\n�������� �ʴ� �����̰ų� �α��� ������ �ٽ� �Է����ּ���.";
                break;

            case NoticeCode.LoginSuccess:
                _noticeText.text = "�α��ο� �����ϼ̽��ϴ�! ĳ���� ������ �ҷ����� �ֽ��ϴ�.";
                break;

            case NoticeCode.CheckExitCreateAccountPanel:
                _noticeText.text = "������ ���� ������ ���߽ð� �α��� ȭ������\n���ư��ðڽ��ϱ�?";
                ShowCloseButton();
                break;

            case NoticeCode.CreateAccountSucess:
                _noticeText.text = "���� ������ �Ϸ�Ǿ����ϴ�.\nüũ ��ư�� �����ø� �α��� ȭ������ ���ư��ϴ�.";
                break;

            case NoticeCode.CreateAccountFail:
                _noticeText.text = "���� ������ �ʿ��� ������ �������� �ʾҽ��ϴ�.\n�ٽ� �õ����ּ���.";
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
    }
    private void ShowCloseButton()
    {
        _closePanelBtn.enabled = true;
        _closePanelBtn.image.color = Color.white;
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
        if (_checkBtn) _checkBtn.onClick.AddListener(OnClickCheck);
        if (_closePanelBtn) _closePanelBtn.onClick.AddListener(OnClickClose);
    }
    private void Start()
    {
        _noticeText.text = "";
        _closePanelBtn.enabled = false;
        _closePanelBtn.image.color = new Color(0,0,0,0);
        this.gameObject.SetActive(false);   
    }
}
