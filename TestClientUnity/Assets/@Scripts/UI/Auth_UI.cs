using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Auth_UI : MonoBehaviour
{
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

    void Start()
    {
        if (_createAccountBtn) _createAccountBtn.onClick.AddListener(OnClickCreateAccount);
        if (_loginBtn) _loginBtn.onClick.AddListener(OnClickLogin);
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
    void OnClickLogin()
    {
        if (_idField.text == "") // ���̵� �Է��ʵ尡 �����������
        {
            _noticePanel.SetActive(true);
            _noticePanel.GetComponent<Notice_UI>().ChangeNoticeCode(NoticeCode.LoginFailNullID);
        }
        else if (_pwField.text == "") // �н����� �Է��ʵ尡 �����������
        {
            _noticePanel.SetActive(true);
            _noticePanel.GetComponent<Notice_UI>().ChangeNoticeCode(NoticeCode.LoginFailNullPW);
        }
        //TODO >> �б� �߰� 
        // 1. �߸��� �α��� ����
        // _noticePanel.SetActive(true);
        // _noticePanel.GetComponent<Notice_UI>().ChangeNoticeCode(NoticeCode.LoginFailNullAccount)
        // 2. �α��� ����
        // _noticePanel.SetActive(true);
        // _noticePanel.GetComponent<Notice_UI>().ChangeNoticeCode(NoticeCode.LoginSuccess)
    }
}
