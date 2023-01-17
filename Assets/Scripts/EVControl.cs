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
    public Action<string> OnUpdateUserIdDisplay { get; set; }

    public void Init()
    {
        // Instantiate other stuff here if needed
    }

    public void FetchUserData(string userId)
    {
        EVModel.Api.CachedUserData = APIHelper.GetUserData(userId);
        OnUpdateUserIdDisplay?.Invoke(EVModel.Api.CachedUserData.name);
    }

    public void UpdateVoucherData(PatchVoucherData updateVoucherData)
    {
        APIHelper.UpdateVoucher(updateVoucherData);
    }

    public void GenerateNewVoucherData(PostVoucherData newVoucherData)
    {
        APIHelper.CreateVoucher(newVoucherData);
    }

    public void ShowVoucherDetails(Voucher voucher)
    {
        OnShowVoucherDetails?.Invoke(voucher);
    }
}
