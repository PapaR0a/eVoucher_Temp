using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EVVoucherProductItemView : MonoBehaviour
{
    private Text m_TxtQuantity;
    private Text m_TxtProductName;
    private InputField m_IfRemaining;

    private VoucherProduct m_Data;

    private void OnDestroy()
    {
        m_IfRemaining.onEndEdit.RemoveAllListeners();
    }

    public void Setup(VoucherProduct data)
    {
        m_Data = data;

        m_TxtQuantity = transform.Find("quantity").GetComponent<Text>();
        m_TxtProductName = transform.Find("name").GetComponent<Text>();
        m_IfRemaining = transform.Find("redeem").GetComponent<InputField>();

        m_IfRemaining.onEndEdit.AddListener(delegate { OnEditRedeem(); });

        m_TxtQuantity.text = data.remaining.ToString();
        m_TxtProductName.text = data.name;
    }

    private void OnEditRedeem()
    {
        if (float.TryParse(m_IfRemaining.text, out float output1))
        {
            m_IfRemaining.text = Mathf.Clamp(output1, 0, m_Data.remaining).ToString();

            m_TxtQuantity.text = (m_Data.remaining - (int)output1).ToString();
        }
    }
}
