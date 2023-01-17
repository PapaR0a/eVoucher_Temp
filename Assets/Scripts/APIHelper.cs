using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

public static class APIHelper
{
    public static void GetListUsers()
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EVConstants.URL_USERLIST);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        Debug.Log($"<color=yellow>GetListUsers: {json}</color>");

        JArray userList = JsonConvert.DeserializeObject<JArray>(json);

        foreach(JObject userData in userList)
        {
            string userId = userData.Value<string>("id");
            string userName = userData.Value<string>("name");
            EVModel.Api.Users.Add(userId, userName);
        }
    }

    public static UserData GetUserData(string userId)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_USERDATA_TEST, userId));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        Debug.Log($"<color=yellow>GetUserData: {json}</color>");

        return JsonUtility.FromJson<UserData>(json);
    }

    public static void UpdateVoucher(PatchVoucherData updateVoucherData)
    {
        HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_VOUCHER_UPDATE));
        updateRequest.Method = "PATCH";

        var postData = new JObject()
        {
            ["patientId"] = updateVoucherData.patiendId,
            ["voucherId"] = updateVoucherData.voucherId,
            ["items"] = JsonConvert.SerializeObject(updateVoucherData.items)
        };

        var encoded = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(postData));
        Debug.Log($"<color=yellow>PATCH Json: {JsonConvert.SerializeObject(encoded)}</color>");

        updateRequest.ContentType = "application/json";
        Stream dataStream = updateRequest.GetRequestStream();
        dataStream.Write(encoded, 0, encoded.Length);
        dataStream.Close();
        HttpWebResponse updateResponse = (HttpWebResponse)updateRequest.GetResponse();

        StreamReader updateReader = new StreamReader(updateResponse.GetResponseStream());
        string json = updateReader.ReadToEnd();

        Debug.Log($"<color=yellow>UpdateVoucher: {json}</color>");
    }

    public static void CreateVoucher(PostVoucherData data)
    {
        HttpWebRequest createRequest = (HttpWebRequest)WebRequest.Create(EVConstants.URL_VOUCHER_CREATE);
        createRequest.Method = "POST";

        var postData = new JObject()
        {
            ["patientId"] = data.patiendId,
            ["voucher"] = new JObject()
            {
                ["id"] = data.voucher.id,
                ["org"] = data.voucher.org,
                ["department"] = data.voucher.department,
                ["status"] = data.voucher.status,
                ["expiry_date"] = data.voucher.expiry_date,
                ["fundingType"] = EVModel.Api.CachedUserData.fundingType,
                ["items"] = JsonConvert.SerializeObject( data.voucher.items )
            }
        };

        var encoded = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(postData));
        Debug.Log($"<color=yellow>POST Json: {JsonConvert.SerializeObject(encoded)}</color>");

        createRequest.ContentType = "application/json";
        Stream dataStream = createRequest.GetRequestStream();
        dataStream.Write(encoded, 0, encoded.Length);
        dataStream.Close();

        HttpWebResponse response = (HttpWebResponse)createRequest.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        reader.Close();
        response.Close();

        Debug.Log($"<color=yellow>CreateVoucher: {json}</color>");
    }
}
