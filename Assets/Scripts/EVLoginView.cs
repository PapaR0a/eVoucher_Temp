using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EVLoginView : MonoBehaviour
{
    [SerializeField] private Dropdown m_Dropdown;
    [SerializeField] private Button m_LoginButton;

    private void OnDestroy()
    {
        EVControl.Api.OnFetchUsers -= OnFetchUserList;
    }

    private void Start()
    {
        EVControl.Api.OnFetchUsers += OnFetchUserList;
    }

    private void OnFetchUserList()
    {
        var userNames = new List<string>();
        foreach (var userData in EVModel.Api.Users)
        {
            userNames.Add(userData.Key);
        }
        m_Dropdown.AddOptions(userNames);
    }

    public void OnLogin()
    {
        EVControl.Api.FetchUserData(EVModel.Api.Users[m_Dropdown.options[m_Dropdown.value].text]);
    }
}
