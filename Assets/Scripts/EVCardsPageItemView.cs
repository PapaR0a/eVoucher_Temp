using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EVCardsPageItemView : MonoBehaviour
{
    private Text m_TxtTitle;
    private Text m_TxtFundingType;
    private Text m_TxtOrganization;
    private Text m_TxtDepartment;
    private Text m_TxtExpiryDate;

    private Text m_TxtId;

    public void SetupCard(Voucher data)
    {
        m_TxtTitle = transform.Find("front/Text").GetComponent<Text>();
        m_TxtFundingType = transform.Find("front/FundingType").GetComponent<Text>();
        m_TxtOrganization = transform.Find("front/Organization").GetComponent<Text>();
        m_TxtDepartment = transform.Find("front/Department").GetComponent<Text>();
        m_TxtExpiryDate = transform.Find("front/ExpiryDate").GetComponent<Text>();

        m_TxtId = transform.Find("back/Text").GetComponent<Text>();

        m_TxtFundingType.text = $"Funding Type: {data.fundingType}";
        m_TxtOrganization.text = $"Organization: {data.org}";
        m_TxtDepartment.text = $"Department: {data.department}";
        m_TxtExpiryDate.text = $"Expiry Date: {data.expiry_date}";

        m_TxtId.text = data.id;
    }
}
