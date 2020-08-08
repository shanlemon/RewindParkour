using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A singleton class that provides singleton support for anything that needs to be a singleton.
 * All singletons can be referenced from afar here.
 */
public class Managers : MonoBehaviour
{
    private static Managers instance;
    public static Managers Instance => instance;

    [SerializeField] private AudioManager audioManager = default;
    public static AudioManager AudioManager => instance?.audioManager;

    [SerializeField] private Timer timeManager = default;
    public static Timer TimeManager => instance?.timeManager;

    [SerializeField] private PauseMenu pauseMenu = default;
    public static PauseMenu PauseMenu => instance?.pauseMenu;

    [SerializeField] private GameManager gameManager = default;
    public static GameManager GameManager => instance?.gameManager;

    [SerializeField] private GameObject playerManager = default;
    public static GameObject Player => instance?.playerManager;

    [SerializeField] private GameObject cameraManager = default;
    public static GameObject Camera => instance?.cameraManager;



    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
