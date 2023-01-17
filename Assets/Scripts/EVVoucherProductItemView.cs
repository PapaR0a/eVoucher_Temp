using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EVVoucherProductItemView : MonoBehaviour
{
    private Text m_TxtQuantity;
    private Text m_TxtProductName;
    private InputField m_IfRedeemCount;

    private VoucherProduct m_Data;
    private int m_Remaining;
    private int m_RedeemCount;

    private void OnDestroy()
    {
        if (m_IfRedeemCount != null)
        {
            m_IfRedeemCount.onEndEdit.RemoveAllListeners();
        }
    }

    public void Setup(VoucherProduct data)
    {
        m_Data = data;

        m_TxtQuantity = transform.Find("quantity").GetComponent<Text>();
        m_TxtProductName = transform.Find("name").GetComponent<Text>();
        m_IfRedeemCount = transform.Find("redeem").GetComponent<InputField>();

        m_IfRedeemCount.onEndEdit.AddListener(delegate { OnEditRedeem(); });

        m_TxtQuantity.text = data.remaining.ToString();
        m_TxtProductName.text = data.name;
    }

    private void OnEditRedeem()
    {
        if (float.TryParse(m_IfRedeemCount.text, out float output1))
        {
            m_IfRedeemCount.text = Mathf.Clamp(output1, 0, m_Data.remaining).ToString();
            m_RedeemCount = (int)output1;
            m_Remaining = m_Data.remaining - (int)output1;
            m_TxtQuantity.text = m_Remaining < 0 ? 0.ToString() : m_Remaining.ToString();
        }
    }

    public int GetItemRemaining()
    {
        return m_Remaining;
    }

    public string GetItemId()
    {
        return m_Data.id;
    }

    public int GetRedeemCount()
    {
        return m_RedeemCount;
    }

    public string GetItemName()
    {
        return m_Data.name;
    }

    public int GetItemDefaultQuantity()
    {
        return m_Data.quantity;
    }
}
