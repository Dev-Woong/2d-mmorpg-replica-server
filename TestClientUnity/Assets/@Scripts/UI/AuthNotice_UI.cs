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
                _noticeText.text = "정말로 계정 생성을 멈추시고 로그인 화면으로\n돌아가시겠습니까?";
                _noticeText.color = Color.red;
                ShowCloseButton();
                break;

            case NoticeCode.CreateAccountSucess:
                _noticeText.text = "계정 생성이 완료되었습니다.\n체크 버튼을 누르시면 로그인 화면으로 돌아갑니다.";
                _noticeText.color = Color.white;
                break;

            case NoticeCode.CreateAccountFail:
                _noticeText.text = "계정 생성에 필요한 조건이 충족되지 않았습니다.\n다시 시도해주세요.";
                _noticeText.color = Color.red;
                break;
            case NoticeCode.LoginFailNullID:
                _noticeText.text = "아이디를 입력해주세요.";
                _noticeText.color = Color.red;
                break;

            case NoticeCode.LoginFailNullPW:
                _noticeText.text = "비밀번호를 입력해주세요";
                _noticeText.color = Color.red;
                break;

            case NoticeCode.DoLogin:
                _noticeText.text = "로그인을 진행중입니다. 잠시만 기다려주세요.";
                _noticeText.color = Color.white;
                break;

            case NoticeCode.LoginFailNullAccount:
                _noticeText.text = "존재하지 않는 계정입니다.\n회원가입을 진행하거나 로그인 정보를 다시 입력해주세요.";
                _noticeText.color = Color.red;
                break;

            case NoticeCode.LoginSuccess:
                _noticeText.text = "로그인에 성공하셨습니다.\n캐릭터 정보를 불러오고 있습니다.";
                _noticeText.color = Color.white;
                break;
            case NoticeCode.RecvCharacterListSuccess:
                _noticeText.text = "캐릭터 리스트를 성공적으로 불러왔습니다.";
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
