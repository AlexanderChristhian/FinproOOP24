using UnityEngine;

public class MoneyComponent : MonoBehaviour
{
    public int money = 0; // Properti uang pemain

    // Fungsi untuk menambahkan uang
    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log($"Money added: {amount}. Total money: {money}");
    }

    // Fungsi untuk mengurangi uang
    public void SubtractMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            Debug.Log($"Money subtracted: {amount}. Total money: {money}");
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}
