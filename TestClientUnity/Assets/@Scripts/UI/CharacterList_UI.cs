using Google.Protobuf.Protocol;
using Packet;
using ServerCore;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CharacterList_UI : MonoBehaviour
{
    public static CharacterList_UI Instance { get; private set; }
    [SerializeField] private Transform _contentParent;         // 리스트 부모(Grid)
    [SerializeField] private CharacterSlot_UI _slotPrefab;  // 캐릭터 슬롯 프리팹
    [SerializeField] private TMP_Text _EmptyListText;
    [SerializeField] private Button _createBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _startBtn;
    [SerializeField] private GameObject _createCharacterPanel;

    private readonly List<CharacterSlot_UI> _slots = new();
    private int _selectedIndex = -1;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (_createBtn) _createBtn.onClick.AddListener(OnClickCreateCharacter);
        if (_exitBtn) _exitBtn.onClick.AddListener(OnClickExitGame);
        if (_startBtn) _startBtn.onClick.AddListener(OnClickStartGame);
    }
    void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        RequestList();
    }
    void RequestList()
    {
        var req = new C_CharacterListRequest();
        var send = ServerPacketManager.MakeSendBuffer(req);
        NetworkManager.Instance.Send(send);
        UnityEngine.Debug.Log($"[UI] 캐릭터 리스트 전송 요청: playerIndex={0}, len={send.Count}");
    }
    public void SetCharacterList(IList<CharacterSummaryInfo> list)
    {
        if (list.Count == 0)
        {
            EmptyListTextEnable(true);
        }
        else
        {
            EmptyListTextEnable(false);
        }
        foreach (Transform c in _contentParent) Destroy(c.gameObject);
        _slots.Clear();
        _selectedIndex = -1;
        _startBtn.interactable = false;
        for (int i = 0; i < _slots.Count; i++)
        {
            var slot = Instantiate(_slotPrefab, _contentParent);
            slot.SetupSlot(list[i], i, OnSlotSelected);
            _slots.Add(slot);
        }
    }
    private void OnSlotSelected(int index)
    {
        _selectedIndex = index;
        _startBtn.interactable = true;
        UpdateHighlights();
    }
    private void UpdateHighlights()
    {
        for (int i = 0; i < _slots.Count; i++)
            _slots[i].SetSelected(i == _selectedIndex);
    }
    private void OnClickStartGame()
    {
        if (_selectedIndex < 0) return;

        // 여기서만 입장 패킷 전송
        var req = new C_EnterGame { PlayerIndex = _selectedIndex };
        var send = ServerPacketManager.MakeSendBuffer(req);
        NetworkManager.Instance.Send(send);

        // 선택: 로딩 표시
        _startBtn.interactable = false;
        // ShowLoading(true);
    }

    void OnClickCreateCharacter()
    {
        _createCharacterPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    void OnClickExitGame()
    {
        Application.Quit();
    }
    void EmptyListTextEnable(bool enable)
    {
        _EmptyListText.enabled = enable;
    }
    












    void Update()
    {
        
    }
}
