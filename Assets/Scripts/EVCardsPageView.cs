using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EVCardsPageView : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefCard;
    [SerializeField] private Transform m_CardsContainer;
    [SerializeField] private GameObject m_PrefHistory;
    [SerializeField] private Transform m_HistoryContainer;

    private bool m_isDataAcquired = false;

    private void OnEnable()
    {
        ClearItems();

        List<Voucher> activeVouchers = EVModel.Api.GetActiveVouchers();
        //Debug.Log($"<color=yellow>activeVouchers {JsonConvert.SerializeObject(activeVouchers)}</color>");
        if (activeVouchers != null && activeVouchers.Count > 0)
        {
            foreach (var voucherData in activeVouchers)
            {
                StartCoroutine(CreateActiveCards(voucherData));
            }
        }

        List<Voucher> historyVouchers = EVModel.Api.GetHistoryVouchers();
        historyVouchers.Reverse();
        //Debug.Log($"<color=yellow>historyVouchers {JsonConvert.SerializeObject(historyVouchers)}</color>");
        if (historyVouchers != null && historyVouchers.Count > 0)
        {
            foreach (var voucherData in historyVouchers)
            {
                StartCoroutine(CreateHistoryCards(voucherData));
            }
        }
    }

    private void ClearItems()
    {
        foreach (Transform card in m_CardsContainer)
        {
            Destroy(card.gameObject);
        }

        foreach (Transform card in m_HistoryContainer)
        {
            Destroy(card.gameObject);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (!m_isDataAcquired && EVModel.Api.CachedUserData != null)
        {
            m_isDataAcquired = true;
        }
    }

    private IEnumerator CreateActiveCards(Voucher voucherData)
    {
        var wait = new WaitForEndOfFrame();
        var card = Instantiate(m_PrefCard, m_CardsContainer).GetComponent<EVCardsPageItemView>();
        yield return wait;
        card.SetupCard(voucherData);
    }

    private IEnumerator CreateHistoryCards(Voucher voucherData)
    {
        var wait = new WaitForEndOfFrame();
        var card = Instantiate(m_PrefHistory, m_HistoryContainer).GetComponent<EVHistoryPageItemView>();
        yield return wait;
        card.SetupCard(voucherData);
    }
}
