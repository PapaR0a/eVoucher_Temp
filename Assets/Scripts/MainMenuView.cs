using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Text m_UserIdDisplay;

    void Start()
    {
        EVControl.Api.OnUpdateUserIdDisplay += UpdateUserNameDisplay;
        //FetchUserData();
        EVControl.Api.FetchUsers();
    }

    private void OnDestroy()
    {
        EVControl.Api.OnUpdateUserIdDisplay -= UpdateUserNameDisplay;
    }

    private void FetchUserData()
    {
        EVControl.Api.FetchUserData("");
    }

    private void UpdateUserNameDisplay(string name)
    {
        m_UserIdDisplay.text = $"Hi {name}";
    }
}
