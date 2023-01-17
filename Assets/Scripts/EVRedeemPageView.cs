using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using zxing;

public class EVRedeemPageView : MonoBehaviour
{
    private Text m_TxtFundingType;
    private Text m_TxtOrganization;
    private Text m_TxtDepartment;
    private Text m_TxtExpiryDate;
    private Text m_TxtId;
    private Button m_BtnRedeem;

    private Voucher m_Data;

    [SerializeField] private GameObject m_PrefVoucherProduct;
    [SerializeField] private Transform m_ProductsContainer;

    [SerializeField] private List<Sprite> m_OrgLogos;
    [SerializeField] private Image m_ImageOrgLogo;

    [SerializeField] private List<Sprite> m_FrontCardImages;
    [SerializeField] private Image m_ImageCardFront;

    [SerializeField] private RawImage m_QRCode;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnDestroy()
    {
        EVControl.Api.OnShowVoucherDetails -= UpdateDetailsView;
        m_BtnRedeem.onClick.RemoveAllListeners();
    }

    private void OnEnable()
    {
        m_TxtFundingType = transform.Find("ScrollView/Viewport/Content/card/front/FundingType").GetComponent<Text>();
        m_TxtOrganization = transform.Find("ScrollView/Viewport/Content/card/front/Organization").GetComponent<Text>();
        m_TxtDepartment = transform.Find("ScrollView/Viewport/Content/card/front/Department").GetComponent<Text>();
        m_TxtExpiryDate = transform.Find("ScrollView/Viewport/Content/card/front/ExpiryDate").GetComponent<Text>();
        m_TxtId = transform.Find("ScrollView/Viewport/Content/ID").GetComponent<Text>();
        m_BtnRedeem = transform.Find("ScrollView/Viewport/Content/CreateQRCode").GetComponent<Button>();

        m_BtnRedeem.onClick.AddListener(OnGenerateQR);

        EVControl.Api.OnShowVoucherDetails += UpdateDetailsView;
    }

    private void UpdateDetailsView(Voucher voucherData)
    {
        m_Data = voucherData;

        m_TxtFundingType.text = $"Funding Type: {voucherData.fundingType}";
        m_TxtOrganization.text = $"Organization: {voucherData.org}";
        m_TxtDepartment.text = $"Department: {voucherData.department}";
        m_TxtExpiryDate.text = $"Expiry Date: {voucherData.expiry_date}";
        m_TxtId.text = voucherData.id;

        m_ImageOrgLogo.sprite = GetOrgSprite(m_Data.org);
        m_ImageCardFront.sprite = GetFrontCardSprite(m_Data.org);

        ClearItems();

        StartCoroutine( CreateItems(voucherData.items) );
    }

    private IEnumerator CreateItems(VoucherProduct[] products)
    {
        var wait = new WaitForEndOfFrame();

        foreach (var product in products)
        {
            var productView = Instantiate(m_PrefVoucherProduct, m_ProductsContainer).GetComponent<EVVoucherProductItemView>();
            yield return wait;

            if (productView != null && product != null)
                productView.Setup(product);
        }
    }

    private void ClearItems()
    {
        foreach (Transform item in m_ProductsContainer)
        {
            Destroy(item.gameObject);
        }
    }

    private void OnGenerateQR()
    {
        //m_QRCode

        var newVoucher = new PostVoucherData();
        newVoucher.patiendId = EVModel.Api.CachedUserData.id;

        newVoucher.voucher = new Voucher();
        newVoucher.voucher.id = "NEW ID";
        newVoucher.voucher.status = "Pending";
        newVoucher.voucher.department = m_Data.department;
        newVoucher.voucher.org = m_Data.org;
        newVoucher.voucher.expiry_date = m_Data.expiry_date;
        newVoucher.voucher.fundingType = m_Data.fundingType;

        var redeemingItems = new List<VoucherProduct>();
        var remainingItems = new PatchVoucherData();
        remainingItems.patiendId = EVModel.Api.CachedUserData.id;
        remainingItems.voucherId = m_Data.id;
        remainingItems.items = new List<VoucherProduct>();
        foreach (Transform item in m_ProductsContainer)
        {
            EVVoucherProductItemView itemView = item.GetComponent<EVVoucherProductItemView>();
            if (itemView != null)
            {
                var remainingItem = new VoucherProduct();
                remainingItem.id = itemView.GetItemId();
                remainingItem.name = itemView.GetItemName();
                remainingItem.quantity = itemView.GetItemDefaultQuantity();
                remainingItem.remaining = itemView.GetItemRemaining();
                remainingItems.items.Add(remainingItem);

                var redeemingItem = new VoucherProduct();
                redeemingItem.id = itemView.GetItemId();
                redeemingItem.name = itemView.GetItemName();
                redeemingItem.quantity = itemView.GetRedeemCount();
                redeemingItems.Add(redeemingItem);
            }
        }

        newVoucher.voucher.items = redeemingItems.ToArray();

        EVControl.Api.UpdateVoucherData(remainingItems);
        EVControl.Api.GenerateNewVoucherData(newVoucher);
    }

    //private Color32[] CreateQR()
    //{

    //}

    private Sprite GetOrgSprite(string org)
    {
        switch (org)
        {
            case "TTSH":
                return m_OrgLogos[(int)Organizations.TTSH];

            case "WDL":
                return m_OrgLogos[(int)Organizations.WDL];

            case "NHGP":
                return m_OrgLogos[(int)Organizations.NHGP];

            default:
                return m_OrgLogos[(int)Organizations.TTSH];
        }
    }

    private Sprite GetFrontCardSprite(string org)
    {
        switch (org)
        {
            case "TTSH":
                return m_FrontCardImages[(int)Organizations.TTSH];

            case "WDL":
                return m_FrontCardImages[(int)Organizations.WDL];

            case "NHGP":
                return m_FrontCardImages[(int)Organizations.NHGP];

            default:
                return m_FrontCardImages[(int)Organizations.TTSH];
        }
    }
}
