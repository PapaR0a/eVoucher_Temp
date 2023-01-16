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

    public static void UpdateVoucher(string userId, string voucherId, Dictionary<string, int> items)
    {
        HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_VOUCHER_UPDATE));
        //request.ContentType = "application/json";
        updateRequest.Method = "PATCH";

        using (var streamWriter = new StreamWriter(updateRequest.GetRequestStream()))
        {
            JObject postJson = new JObject()
            {
                ["patientId"] = userId,
                ["voucherId"] = voucherId,
                ["items"] = JsonConvert.SerializeObject(items)
            };
            Debug.Log($"<color=yellow>PATCH Json: {JsonConvert.SerializeObject(postJson).Replace(@"\", "") }</color>");
            streamWriter.Write(JsonConvert.SerializeObject( postJson ).Replace(@"\", ""));
        }

        //HttpWebResponse updateResponse = (HttpWebResponse)updateRequest.GetResponse();

        //StreamReader updateReader = new StreamReader(updateResponse.GetResponseStream());
        //string json = updateReader.ReadToEnd();

        //Debug.Log($"<color=yellow>received json: {json}</color>");
    }

    public static void CreateVoucher(string userId, Voucher data)
    {
        HttpWebRequest createRequest = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_VOUCHER_CREATE));
        //request.ContentType = "application/json";
        createRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(createRequest.GetRequestStream()))
        {
            string voucherJson = JsonConvert.SerializeObject(data);

            JObject postJson = new JObject()
            {
                ["patientId"] = userId,
                ["voucher"] = voucherJson
            };

            Debug.Log($"<color=yellow>POST Json: {JsonConvert.SerializeObject(postJson).Replace(@"\","")}</color>");
            streamWriter.Write(JsonConvert.SerializeObject(postJson).Replace(@"\", ""));
        }

        //HttpWebResponse response = (HttpWebResponse)createRequest.GetResponse();

        //StreamReader reader = new StreamReader(response.GetResponseStream());
        //string json = reader.ReadToEnd();

        //Debug.Log($"<color=yellow>received json: {json}</color>");
    }
}
