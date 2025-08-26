using Google.Protobuf.Protocol;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot_UI : MonoBehaviour
{
    [SerializeField] private Image highlight; // 선택 시 켜줄 테두리/백그라운드 (옵션
    [SerializeField] private TMP_Text _userName;
    [SerializeField] private TMP_Text _userGender;
    [SerializeField] private TMP_Text _userLevel;
    [SerializeField] private Button _selectBtn;
    [SerializeField] private int _index;

    public void SetupSlot(CharacterSummaryInfo client, int index,Action<int> onSelect)
    {
        _index = index;
        _userName.text = client.Username;
        switch (client.Gender)
        {
            case EGender.GenderMale:
                _userGender.text = "남자";
                break;
            case EGender.GenderFemale:
                _userGender.text = "여자";
                break;
        }
        _userLevel.text = client.Level.ToString();
        _selectBtn.onClick.RemoveAllListeners();
        _selectBtn.onClick.AddListener(() => onSelect?.Invoke(_index));
        SetSelected(false);
    }
    public void SetSelected(bool selected)
    {
        if (highlight) highlight.enabled = selected;
    }
}
