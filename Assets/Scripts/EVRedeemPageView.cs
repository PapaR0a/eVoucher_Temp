using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVRedeemPageView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EVControl.Api.OnShowVoucherDetails += UpdateDetailsView;
    }

    void OnDestroy()
    {
        EVControl.Api.OnShowVoucherDetails -= UpdateDetailsView;
    }

    private void UpdateDetailsView(Voucher voucherData)
    {

    }
}
