using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    public MoneyComponent moneyComponent; // Referensi ke MoneyComponent
    public TextMeshProUGUI moneyText;     // Referensi ke TextMeshPro UI

    void Start()
    {
        if (moneyComponent == null)
        {
            Debug.LogError("MoneyComponent is not assigned!");
        }

        if (moneyText == null)
        {
            Debug.LogError("TextMeshProUGUI is not assigned!");
        }

        // Inisialisasi teks dengan nilai awal uang
        UpdateMoneyText();
    }

    void Update()
    {
        // Perbarui teks jika nilai money berubah
        UpdateMoneyText();
    }

    void UpdateMoneyText()
    {
        moneyText.text = $"Money: {moneyComponent.money}";
    }
}
