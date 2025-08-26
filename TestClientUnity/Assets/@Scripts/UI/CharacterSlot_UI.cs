using Google.Protobuf.Protocol;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot_UI : MonoBehaviour
{
    [SerializeField] private Image highlight; // ���� �� ���� �׵θ�/��׶��� (�ɼ�
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
                _userGender.text = "����";
                break;
            case EGender.GenderFemale:
                _userGender.text = "����";
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
