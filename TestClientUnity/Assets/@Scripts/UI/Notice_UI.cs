using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum NoticeCode
{
    LoginFailNullID = 0,
    LoginFailNullPW,
    LoginFailNullAccount,
    LoginSuccess,
    OpenCreateAccountPanel ,
    CreateAccountFail  ,
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
            case NoticeCode.LoginFailNullAccount:
                _noticeText.text = "�α��ο� �����߽��ϴ� " +
                    "�������� �ʴ� �����̰ų� �α��� ������ �ٽ� �Է����ּ���.";
                break;
            case NoticeCode.LoginSuccess:
                _noticeText.text = "�α����� �������Դϴ�. ��ø� ��ٷ��ּ���.";
                break;

            case NoticeCode.OpenCreateAccountPanel:
                _noticeText.text = "ȸ�������� �����Ͻðڽ��ϱ�?";
                _closePanelBtn.enabled = true;
                _closePanelBtn.image.color = new Color(1, 1, 1, 1);
                break;

            case NoticeCode.CreateAccountSucess:
                _noticeText.text = "���� ������ �Ϸ�Ǿ����ϴ�. üũ ��ư�� �����ø� �α��� ȭ������ ���ư��ϴ�.";
                break;
            case NoticeCode.CreateAccountFail:
                _noticeText.text = "���� ������ �ʿ��� ������ �������� �ʾҽ��ϴ�. �ٽ� �õ����ּ���.";
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
        else if (_noticeCode == NoticeCode.OpenCreateAccountPanel)
        {
            _createAccountPanel.SetActive(true);
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.CreateAccountSucess)
        {
            _createAccountPanel.SetActive(false);
            _authorizePanel.SetActive(true);
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
        else if (_noticeCode == NoticeCode.CreateAccountFail)
        {
            _noticeText.text = "";
            this.gameObject.SetActive(false);
        }
    }
    private void OnClickClose()
    {
        _noticeText.text = "";
        _authorizePanel.SetActive(true);
        _closePanelBtn.enabled = false;
        _closePanelBtn.image.color = new Color(1, 1, 1, 0);
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
        _closePanelBtn.image.color = new Color(1,1,1,0);
        this.gameObject.SetActive(false);   
    }
}
