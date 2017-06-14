using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int initialEnemyCount = 200;
    public GameObject player;
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public MeteorShower meteorShower;
    public TextTyping textTyping;
    public RocketLauncher[] guns;

    public new Camera camera;
    public FadeImageEffect cameraFadeEffect;
    public PixelateImageEffect pixelateImageEffect;
    
	void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponent<PlayerHealth>();
        meteorShower = GameObject.FindObjectOfType<MeteorShower>();
        textTyping = GameObject.FindObjectOfType<TextTyping>();
        guns = player.GetComponents<RocketLauncher>();

        camera = Camera.main;
        cameraFadeEffect = camera.GetComponent<FadeImageEffect>();
        pixelateImageEffect = camera.GetComponent<PixelateImageEffect>();

        StartCoroutine(PlayStory());
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public Vector3 playerPosition
    {
        get { return player.transform.position; }
    }

    public void DamagePlayer(float damage)
    {
        playerHealth.DamagePlayer(damage);
    }

    void Update()
    {
        pixelateImageEffect.downsample = Mathf.FloorToInt(
            (playerHealth.maxHealth - playerHealth.currentHealth) / playerHealth.maxHealth * 60
        );
    }

    IEnumerator PlayStory()
    {
        // Initialization
        cameraFadeEffect.Fade(Color.black, Color.black, 0);
        textTyping.Clear();
        playerMovement.inputs = new PlayerMovement.Inputs
        {
            allowPlayerControl = false
        };
        for (var i = 0; i < guns.Length; i++)
        {
            guns[i].allowPlayerControl = false;
        }

        // Fade in from black
        yield return StartCoroutine(DisplayMessage("Activating visual array"));
        float fadeTime = 4;
        cameraFadeEffect.Fade(Color.black, new Color(0, 0, 0, 0), fadeTime);
        yield return new WaitForSeconds(fadeTime);

        // Eject from dropship
        yield return StartCoroutine(DisplayMessage("Deploying asset"));
        playerMovement.inputs = new PlayerMovement.Inputs
        {
            allowPlayerControl = false,
            Vertical = 1
        };
        yield return new WaitForSeconds(1);
        playerMovement.inputs = new PlayerMovement.Inputs
        {
            allowPlayerControl = true
        };
        for (var i = 0; i < guns.Length; i++)
        {
            guns[i].allowPlayerControl = true;
        }

        yield return StartCoroutine(
            DisplayMessage("ENEMY PRESENCE AT 100%", true)
        );
        float percentEnemies = 100.0f;
        for (int i = 9; i >= 0; --i)
        {
            while (percentEnemies > 10.0f * i)
            {
                yield return new WaitForSeconds(1);
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");
                percentEnemies = Mathf.Clamp(100.0f * enemies.Length / initialEnemyCount, 0.0f, 100.0f);
            }
            yield return StartCoroutine(
                DisplayMessage(string.Format("ENEMY PRESENCE AT {0}%", 10 * i), true)
            );
        }
    }

    IEnumerator DisplayMessage(string message, bool keepOnScreen = false)
    {
        bool messageDisplayed = false;
        textTyping.Enqueue(message, keepOnScreen, m => { messageDisplayed = true; });
        while (!messageDisplayed)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnPlayerDeath(PlayerHealth p)
    {
        StopAllCoroutines();
        textTyping.Clear();
        StartCoroutine(OnPlayerDeath_Coroutine());
    }

    IEnumerator OnPlayerDeath_Coroutine()
    {
        playerMovement.inputs = new PlayerMovement.Inputs
        {
            allowPlayerControl = false
        };
        for (var i = 0; i < guns.Length; i++)
        {
            guns[i].allowPlayerControl = false;
        }

        yield return StartCoroutine(DisplayMessage("Mission Failed"));
        yield return new WaitForSeconds(3);
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogErrorFormat("LoadScene({0}): scene name not specified", sceneName);
        }
        else if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogErrorFormat("LoadScene({0}): scene {0} not found", sceneName);
        }
        else
        {
            Debug.LogFormat("LoadScene({0})", sceneName);
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(sceneName);
        }
    }
}
