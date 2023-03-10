using System.Collections.Generic;

public class EVModel
{
    public static EVModel Api { get; set; } = new EVModel();

    public Dictionary<string, string> Users { get; set; } = new Dictionary<string, string>();

    public UserData CachedUserData { get; set; } = null;

    public Voucher CachedCurrentVoucher { get; set; } = null;

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
                    voucher.fundingType = CachedUserData.fundingType;
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
                    voucher.fundingType = CachedUserData.fundingType;
                    historyVouchers.Add(voucher);
                }
            }
        }

        return historyVouchers;
    }
}

public enum Organizations
{
    TTSH,
    WDL,
    NHGP
}

public enum VoucherStatus
{
    Expired = -1,
    Redeemed = 0,
    Active = 1,
    Partial_Redemption = 2,
    Pending = 3
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
    public VoucherProduct[] items;
    public string address;
    public string contactNo;
    public string email;
}

[System.Serializable]
public class VoucherProduct
{
    public string id;
    public string name;
    public int quantity;
    public int remaining;
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
    public string fundingType; 
    public int funding_type_code;
    public string status;
    public int status_code;
    public Voucher[] vouchers;
}

[System.Serializable]
public class PostVoucherData
{
    public string patiendId;
    public Voucher voucher;
}

[System.Serializable]
public class PatchVoucherData
{
    public string patiendId;
    public string voucherId;
    public List<VoucherProduct> items;
}
