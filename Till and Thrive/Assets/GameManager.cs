using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text timerText;      // Untuk menampilkan waktu tersisa
    public TMP_Text roundText;      // Untuk menampilkan round saat ini
    public TMP_Text questMoneyText; // Untuk menampilkan QuestMoney

    [Header("Game Settings")]
    public float roundDuration = 210f;  // Durasi tiap round (dalam detik)
    private float currentTimer;        // Waktu tersisa untuk round saat ini

    public int currentRound = 1;       // Round saat ini
    public int questMoney = 0;         // Uang yang dimiliki pemain
    public int questTarget = 100;      // Target uang untuk menyelesaikan round
    public int targetIncrement = 50;   // Penambahan target uang setiap round baru

    private int targetleft;

    private bool isGameRunning = true; // Apakah permainan sedang berlangsung
    public MoneyComponent playerMoney;

    [Header("Inventory Settings")]
    public GameObject inventoryPanel; // Objek yang mewakili inventory

    void Start()
    {
        targetleft = questTarget;
        StartRound(); // Memulai permainan dari round pertama
        inventoryPanel.SetActive(false); // Pastikan inventory dimulai dalam keadaan tersembunyi
    }

    void Update()
    {
        if (!isGameRunning) return;

        // Update timer setiap frame
        currentTimer -= Time.deltaTime;
        UpdateTimerUI();

        // Jika waktu habis dan target tidak tercapai, round gagal
        if (currentTimer <= 0)
        {
            EndRound(false);
        }

        // Cek jika tombol P ditekan untuk toggle inventory
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleInventory();
        }
    }

    // Memulai round baru
    void StartRound()
    {
        Debug.Log($"Round {currentRound} Started!");
        currentTimer = roundDuration; // Reset timer
        UpdateTimerUI();
        UpdateRoundUI();
        UpdateQuestMoneyUI();
    }

    // Mengakhiri round
    void EndRound(bool success)
    {
        isGameRunning = false;

        if (success)
        {
            Debug.Log($"Round {currentRound} Completed!");
            currentRound++;
            questTarget += targetIncrement; // Tambah target uang
            questMoney = 0; // Reset QuestMoney
            StartNextRound();
        }
        else
        {
            Debug.Log($"Round {currentRound} Failed!");
            GameOver();
        }
    }

    // Menambah uang pemain
    public void UpdateQuestMoney()
    {
        if (playerMoney != null)
        {
            int availableMoney = playerMoney.money; // Uang pemain saat ini
            int neededMoney = questTarget - questMoney; // Uang yang dibutuhkan untuk mencapai target

            // Hitung jumlah uang yang akan ditambahkan ke questMoney
            int moneyToAdd = Mathf.Min(availableMoney, neededMoney);

            // Perbarui questMoney dan kurangi uang pemain
            questMoney += moneyToAdd;
            playerMoney.SubtractMoney(moneyToAdd);

            Debug.Log($"QuestMoney diperbarui menjadi {questMoney}");

            UpdateQuestMoneyUI();

            // Cek apakah questMoney telah mencapai target
            if (questMoney >= questTarget)
            {
                EndRound(true);
            }
        }
        else
        {
            Debug.LogError("PlayerMoney tidak ditemukan!");
        }
    }

    // Memulai round berikutnya
    void StartNextRound()
    {
        isGameRunning = true;
        StartRound();
    }

    // Update tampilan timer di UI
    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTimer / 60);
        int seconds = Mathf.FloorToInt(currentTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Update tampilan round di UI
    void UpdateRoundUI()
    {
        roundText.text = $"Round: {currentRound}";
    }

    // Update tampilan QuestMoney di UI
    void UpdateQuestMoneyUI()
    {
        questMoneyText.text = $"Target: {questMoney} / {questTarget}";
    }

    // Mengatur logika untuk Game Over
    void GameOver()
    {
        Debug.Log("Game Over! Try Again.");
        isGameRunning = false;
        // Tambahkan logika untuk kembali ke menu atau restart game
    }

    // Fungsi untuk toggle visibilitas Inventory
    void ToggleInventory()
    {
        bool isActive = inventoryPanel.activeSelf; // Cek apakah inventory sedang aktif
        inventoryPanel.SetActive(!isActive); // Toggle aktifitas
    }
}
