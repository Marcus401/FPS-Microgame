using UnityEngine;

public class SurvivalManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject hoverBotPrefab;
    public GameObject turretBossPrefab;

    [Header("Spawn Locations")]
    public Transform[] spawnPoints;
    public Transform bossSpawnPoint;

    [Header("Timer Settings")]
    public float timeBetweenSpawns = 5f; 
    public float timeToSurvive = 180f;

    private float timer = 0f;
    private float nextSpawnTime = 0f;
    private bool bossSpawned = false;
    private bool gameWon = false;
    
    // This is our new tracker variable
    private GameObject activeBoss; 

    void Update()
    {
        if (gameWon) return;

        // Phase 2: The Boss Fight
        if (bossSpawned)
        {
            // If the boss was spawned but is now gone (destroyed by your gun), YOU WIN!
            if (activeBoss == null) 
            {
                WinGame();
            }
            return; // Stop spawning regular bots during the boss fight
        }

        // Phase 1: Survival Timer
        timer += Time.deltaTime;

        if (timer >= timeToSurvive)
        {
            SpawnBoss();
        }
        else if (timer >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = timer + timeBetweenSpawns;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(hoverBotPrefab, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
    }

    void SpawnBoss()
    {
        // We save the spawned boss into 'activeBoss' so the Update loop can keep an eye on it
        activeBoss = Instantiate(turretBossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        bossSpawned = true;
    }

    void WinGame()
    {
        gameWon = true;
        // Slow motion effect for a dramatic finish!
        Time.timeScale = 0.2f; 
    }

    // This built-in Unity trick draws text directly on the screen without needing a complex UI Canvas!
    void OnGUI()
    {
        if (gameWon)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 60;
            style.normal.textColor = Color.cyan;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            
            // Draw a giant holographic message right in the middle of the screen
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "OBJECTIVE CLEARED\nBOSS DEFEATED", style);
        }
    }
}