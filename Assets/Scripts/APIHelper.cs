using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public static class APIHelper
{
    public static UserData GetUserData(string userId)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_USERDATA_TEST, userId));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        Debug.Log($"<color=yellow>received json: {json}</color>");

        return JsonUtility.FromJson<UserData>(json);
    }

    public static void UpdateVoucher(PatchVoucherData updateVoucherData)
    {
        HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_VOUCHER_UPDATE));
        //request.ContentType = "application/json";
        updateRequest.Method = "PATCH";

        using (var streamWriter = new StreamWriter(updateRequest.GetRequestStream()))
        {
            string voucherJson = JsonConvert.SerializeObject(updateVoucherData);

            Debug.Log($"<color=yellow>PATCH Json: {voucherJson}</color>");
            streamWriter.Write(voucherJson);
        }

        HttpWebResponse updateResponse = (HttpWebResponse)updateRequest.GetResponse();

        StreamReader updateReader = new StreamReader(updateResponse.GetResponseStream());
        string json = updateReader.ReadToEnd();

        Debug.Log($"<color=yellow>received json: {json}</color>");
    }

    public static void CreateVoucher(PostVoucherData data)
    {
        HttpWebRequest createRequest = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_VOUCHER_CREATE));
        //request.ContentType = "application/json";
        createRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(createRequest.GetRequestStream()))
        {
            string voucherJson = JsonConvert.SerializeObject(data);

            Debug.Log($"<color=yellow>POST Json: {voucherJson}</color>");
            streamWriter.Write(voucherJson);
        }

        HttpWebResponse response = (HttpWebResponse)createRequest.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        Debug.Log($"<color=yellow>received json: {json}</color>");
    }
}
