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

    public Action<Voucher> OnShowVoucherDetails { get; set; }

    public void Init()
    {
        // Instantiate other stuff here if needed
    }

    public void FetchUserData(string userId)
    {
        EVModel.Api.CachedUserData = APIHelper.GetUserData(userId);
    }

    public void UpdateVoucherData(string voucherId, Dictionary<string, int> items)
    {
        var userData = EVModel.Api.CachedUserData;

        APIHelper.UpdateVoucher(userData.id, voucherId, items);
    }

    public void GenerateNewVoucherData(string userId, Voucher newVoucherData)
    {
        APIHelper.CreateVoucher(userId, newVoucherData);
    }

    public void ShowVoucherDetails(Voucher voucher)
    {
        OnShowVoucherDetails?.Invoke(voucher);
    }
}
