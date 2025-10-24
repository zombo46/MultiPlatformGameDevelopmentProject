using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCamera : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Camera2;

    public GameObject startCanvas;
    public GameObject deathCanvas;
    public GameObject winCanvas;

    public PlayerMovement playerMovement;
    public PlayerVitality playerVitality;

    // Canvases on display 1 to hide during intro
    public Canvas[] display1Canvases;

    // Allow exactly one switch from intro -> main
    private bool hasSwitchedToMain = false;

    // Store main camera's original culling mask so we can restore it
    private int mainOriginalCullingMask = -1;

    private bool playerDead;
    

    void Start()
    {
        playerDead = false;

        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerVitality = FindFirstObjectByType<PlayerVitality>();

        if (MainCamera != null)
        {
            var cam = MainCamera.GetComponent<Camera>();
            if (cam != null)
            {
                mainOriginalCullingMask = cam.cullingMask;
                cam.targetDisplay = 0;
            }
        }

        if (Camera2 != null)
        {
            var cam2 = Camera2.GetComponent<Camera>();
            if (cam2 != null)
                cam2.targetDisplay = (Display.displays.Length > 1) ? 1 : 0;
        }

        for (int i = 1; i < Display.displays.Length; i++)
            Display.displays[i].Activate();

        ShowIntro();
    }

    void Update()
    {
        if (hasSwitchedToMain) return;

        // Use the new Input System unconditionally for input checks.
        // (This avoids the InvalidOperationException raised by calling UnityEngine.Input
        // when the project uses the Input System package exclusively.)
        // We still guard against null Keyboard/Gamepad instances to avoid NREs.
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            SwitchToMain();
            return;
        }

        if (Gamepad.current != null && (Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.startButton.wasPressedThisFrame))
        {
            SwitchToMain();
            return;
        }

        // If you still need legacy Input support, reintroduce a conditional fallback here.
    }

    void ShowIntro()
    {
        if (playerMovement != null)
            playerMovement.SetCanMove(false);
        else
            Debug.LogWarning("ShowIntro: PlayerMovement is null; cannot disable movement for intro.");

        // Enable intro camera
        if (Camera2 != null)
        {
            Camera2.SetActive(true);
            var cam2 = Camera2.GetComponent<Camera>();
            if (cam2 != null) cam2.enabled = true;

            startCanvas.SetActive(true);
            deathCanvas.SetActive(false);
            winCanvas.SetActive(false);
        }
    }

    void ShowDeath()
    {
        hasSwitchedToMain = false;

        playerDead = true;

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
            Debug.Log("Game Over!");
        }

        else
        {
            Debug.LogWarning("ShowDeath: PlayerMovement is null; cannot disable movement for death.");
        }

        if (Camera2 != null)
        {
            Camera2.SetActive(true);
            var cam2 = Camera2.GetComponent<Camera>();
            if (cam2 != null) cam2.enabled = true;

            startCanvas.SetActive(false);
            deathCanvas.SetActive(true);
            winCanvas.SetActive(false);
        }
    }

    void ShowWin()
    {
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
            Debug.Log("Game Over!");
        }

        else
        {
            Debug.LogWarning("ShowWin: PlayerMovement is null; cannot disable movement for win.");
        }

        if (Camera2 != null)
        {
            Camera2.SetActive(true);
            var cam2 = Camera2.GetComponent<Camera>();
            if (cam2 != null) cam2.enabled = true;

            startCanvas.SetActive(false);
            deathCanvas.SetActive(false);
            winCanvas.SetActive(true);
        }
    }

    public void SwitchToMain()
    {
        if (playerDead)
        {
            this.SendMessage("revive");
        }

        if (hasSwitchedToMain) return;
        hasSwitchedToMain = true;

        // Disable intro camera (component then GameObject)
        if (Camera2 != null)
        {
            var cam2 = Camera2.GetComponent<Camera>();
            if (cam2 != null) cam2.enabled = false;
            Camera2.SetActive(false);
        }

        // Restore main camera to render scene/UI
        if (MainCamera != null)
        {
            var cam = MainCamera.GetComponent<Camera>();
            if (cam != null)
            {
                cam.enabled = true;
                cam.cullingMask = (mainOriginalCullingMask >= 0) ? mainOriginalCullingMask : -1;
                cam.targetDisplay = 0;
            }
        }

        // Restore canvases
        if (display1Canvases != null)
        {
            foreach (var c in display1Canvases)
            {
                if (c == null) continue;
                if (c.renderMode == RenderMode.ScreenSpaceOverlay)
                    c.gameObject.SetActive(true);
                else
                    c.enabled = true;
            }
        }

        // Enable player control + unpause oxygen (safe null checks)
        if (playerMovement != null)
            playerMovement.SetCanMove(true);
        else
            Debug.LogWarning("SwitchToMain: PlayerMovement is null; cannot enable movement.");

        if (playerVitality != null)
            playerVitality.SetOxygenPaused(false);
        else
            Debug.LogWarning("SwitchToMain: PlayerVitality is null; cannot unpause oxygen.");
    }
}
