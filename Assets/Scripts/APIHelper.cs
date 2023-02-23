using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

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
            EVModel.Api.Users.Add(userName, userId);
        }
    }

    public static UserData GetUserData(string userId)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_USERDATA, userId));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        Debug.Log($"<color=yellow>GetUserData: {json}</color>");

        return JsonUtility.FromJson<UserData>(json);
    }

    public static void UpdateVoucher(PatchVoucherData updateVoucherData)
    {
        HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(EVConstants.URL_VOUCHER_UPDATE);
        updateRequest.Method = "POST";

        var putData = new JObject()
        {
            ["patientId"] = updateVoucherData.patiendId,
            ["voucherId"] = updateVoucherData.voucherId,
            ["items"] = JArray.FromObject(updateVoucherData.items)
        };

        Debug.Log($"<color=yellow>POST Json: {JsonConvert.SerializeObject(putData)}</color>");
        var encoded = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(putData));

        updateRequest.ContentType = "application/json";
        Stream dataStream = updateRequest.GetRequestStream();
        dataStream.Write(encoded, 0, encoded.Length);
        dataStream.Close();
        HttpWebResponse updateResponse = (HttpWebResponse)updateRequest.GetResponse();

        StreamReader updateReader = new StreamReader(updateResponse.GetResponseStream());
        string json = updateReader.ReadToEnd();

        updateReader.Close();
        updateResponse.Close();

        Debug.Log($"<color=yellow>UpdateVoucher Success</color>");
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
                ["items"] = JArray.FromObject(data.voucher.items),
                ["address"] = data.voucher.address,
                ["contactNo"] = data.voucher.contactNo,
                ["email"] = data.voucher.email,
                ["deliveryDate"] = data.voucher.deliveryDate,
                ["deliveryTime"] = data.voucher.deliveryTime
            }
        };

        Debug.Log($"<color=yellow>POST Json: {JsonConvert.SerializeObject(postData)}</color>");
        var encoded = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(postData));

        createRequest.ContentType = "application/json";
        Stream dataStream = createRequest.GetRequestStream();
        dataStream.Write(encoded, 0, encoded.Length);
        dataStream.Close();

        HttpWebResponse response = (HttpWebResponse)createRequest.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        reader.Close();
        response.Close();

        Debug.Log($"<color=yellow>Create Pending Voucher Success: {json}</color>");
    }
}
