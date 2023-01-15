using System.Collections.Generic;

public class EVModel
{
    public static EVModel Api { get; set; } = new EVModel();

    public UserData CachedUserData { get; set; } = null;

    public string UserId { get; set; }

    public UserData GetCachedUserData()
    {
        if (CachedUserData == null)
        {
            CachedUserData = APIHelper.GetUserData(UserId);
        }

        return CachedUserData;
    }

    public List<Voucher> GetActiveVouchers()
    {
        List<Voucher> activeVouchers = new List<Voucher>();

        if (CachedUserData != null)
        {
            foreach(var voucher in CachedUserData.vouchers)
            {
                if (voucher.status == "active")
                {
                    voucher.fundingType = CachedUserData.funding_type;
                    activeVouchers.Add(voucher);
                }
            }
        }

        return activeVouchers;
    }

    public List<Voucher> GetHistoryVouchers()
    {
        List<Voucher> historyVouchers = new List<Voucher>();

        if (CachedUserData != null)
        {
            foreach (var voucher in CachedUserData.vouchers)
            {
                if (voucher.status != "active")
                {
                    voucher.fundingType = CachedUserData.funding_type;
                    historyVouchers.Add(voucher);
                }
            }
        }

        return historyVouchers;
    }
}

[System.Serializable]
public class Voucher
{
    public string id;
    public string org;
    public string department;
    public string status;
    public string expiry_date;
    public string fundingType;
}

[System.Serializable]
public class UserData
{
    public string id;
    public string name;
    public string phone;
    public string email;
    public int funding;
    public int subsidy;
    public string duration;
    public string funding_type; 
    public int funding_type_code;
    public string status;
    public int status_code;
    public Voucher[] vouchers;
}
