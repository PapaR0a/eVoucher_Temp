using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using UnityEngine;

public class EVControl
{
    #region API
    private static EVControl api;

    public static EVControl Api
    {
        get
        {
            if (api == null)
            {
                api = new EVControl();
            }
            return api;
        }
    }
    #endregion

    public void Init()
    {
        // Instantiate other stuff here if needed
    }

    public void FetchUserData(string userId)
    {
        EVModel.Api.CachedUserData = APIHelper.GetUserData(userId);
    }
}
