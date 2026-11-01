using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private TextMeshProUGUI enemyListDisplay;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private KeyCode spawnKey = KeyCode.E;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private string[] enemyTypes = { "Blue Enemy", "Red Enemy", "Green Enemy" };

    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            SpawnRandomEnemy();
        }
    }

    public void SpawnRandomEnemy()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
        GameObject newEnemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        string randomType = enemyTypes[Random.Range(0, enemyTypes.Length)];
        newEnemy.name = randomType;

        Color targetColor = GetColorFromName(randomType);

        if (newEnemy.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.color = targetColor;
        }
        else if (newEnemy.TryGetComponent<Renderer>(out var meshRenderer))
        {
            meshRenderer.material.color = targetColor;
        }

        ClickableEnemy clickable = newEnemy.GetComponent<ClickableEnemy>();
        if (clickable == null)
        {
            clickable = newEnemy.AddComponent<ClickableEnemy>();
        }
        clickable.Initialize(this);

        activeEnemies.Add(newEnemy);
        UpdateUI();
    }

    private Color GetColorFromName(string enemyName)
    {
        switch (enemyName)
        {
            case "Blue Enemy":
                return Color.blue;
            case "Red Enemy":
                return Color.red;
            case "Green Enemy":
                return Color.green;
            default:
                return Color.white;
        }
    }

    public void DefeatEnemy(GameObject enemy)
    {
        if (enemy != null && activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            Destroy(enemy);
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        string displayString = "Active Enemies:\n";
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] != null)
            {
                string enemyName = activeEnemies[i].name;

                displayString += $"{enemyName}\n";
            }
        }

        if (enemyListDisplay != null)
        {
            enemyListDisplay.text = displayString;
        }

        if (enemyCountText != null)
        {
            enemyCountText.text = $"{activeEnemies.Count}";
        }
    }

    public void ClearWave()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] != null)
            {
                Destroy(activeEnemies[i]);
            }
        }

        activeEnemies.Clear();
        UpdateUI();
    }
}