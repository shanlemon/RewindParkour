using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private KeyCode pauseButton;
    [SerializeField] private GameObject settingsMenuPanel;

    
	[SerializeField] private UIEntrancer menuEntrancer = default;
	[SerializeField] private UIEntrancer backEntrancer = default;
    [SerializeField] private Slider mouseSensitivySlider;

    public bool SettingsOpen {get; private set;}
    private bool playerWasEnabled;
    private PlayerLook playerLook;

    private void Start() {
        playerLook = Managers.Player.GetComponent<PlayerLook>();
        
        float value = PlayerPrefs.GetFloat("MouseSensitiviy", .5f);
        SetMouseSensitivity(value);
    }

    public void SetMouseSensitivity(float amount)
    {
        PlayerPrefs.SetFloat("MouseSensitiviy", amount);
        mouseSensitivySlider.value = amount;
        playerLook.scaler = amount;
    }

    private void Update() {
        if (Input.GetKeyDown(pauseButton))
		{
			if (SettingsOpen)
			{
				ExitSettings();
			}
			else
			{
				EnterSettings();
			}
		}
    }

    public void EnterSettings() {
		settingsMenuPanel.SetActive(true);

		playerWasEnabled = playerLook.enabled;
		if (playerWasEnabled) {
			playerLook.enabled = false;
		}
        UnlockMouse();

		menuEntrancer.Enter();
		backEntrancer.Enter();
		SettingsOpen = true;

		Time.timeScale = 0;
	}

	public void ExitSettings() 
	{
		if (playerWasEnabled) {
			playerLook.enabled = true;
		}
        LockMouse();
		menuEntrancer.Exit();
		backEntrancer.Exit();
		SettingsOpen = false;

		Time.timeScale = 1;
	}

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable() {
		Time.timeScale = 1;
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu() 
	{
		Debug.Log("GO TO MAIN MEUN");
	}
    
}
