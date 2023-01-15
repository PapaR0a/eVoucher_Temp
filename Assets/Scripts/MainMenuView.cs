using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuView : MonoBehaviour
{
    void Start()
    {
        FetchUserData();
    }

    private void OnDestroy()
    {

    }

    private void FetchUserData()
    {
        EVControl.Api.FetchUserData("");
    }
}
