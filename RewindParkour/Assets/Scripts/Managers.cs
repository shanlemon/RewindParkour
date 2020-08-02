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
