using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        var newVoucher = new Voucher();
        newVoucher.id = "NEW ID";
        newVoucher.status = "Pending";
        newVoucher.department = m_Data.department;
        newVoucher.org = m_Data.org;
        newVoucher.expiry_date = m_Data.expiry_date;
        newVoucher.fundingType = m_Data.fundingType;

        var redeemingItems = new List<VoucherProduct>();
        var remainingItems = new Dictionary<string, int>();
        foreach (Transform item in m_ProductsContainer)
        {
            EVVoucherProductItemView itemView = item.GetComponent<EVVoucherProductItemView>();
            if (itemView != null)
            {
                remainingItems.Add(itemView.GetItemId(), itemView.GetItemRemaining());

                var redeemingItem = new VoucherProduct();
                redeemingItem.id = itemView.GetItemId();
                redeemingItem.name = itemView.GetItemName();
                redeemingItem.quantity = itemView.GetRedeemCount();
                redeemingItems.Add(redeemingItem);
            }
        }

        newVoucher.items = redeemingItems.ToArray();

        EVControl.Api.GenerateNewVoucherData(m_Data.id, newVoucher);
        EVControl.Api.UpdateVoucherData(m_Data.id, remainingItems);
    }
}
