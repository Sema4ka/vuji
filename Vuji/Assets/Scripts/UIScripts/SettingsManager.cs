using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region Fields
    public static Action<bool> keybindMovementToggled;
    [SerializeField] GameObject Keybind;
    [SerializeField] DataBase dataBase;
    #region PanelsFields
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject videoSettingsPanel;
    [SerializeField] GameObject midSettingsPanel;
    [SerializeField] GameObject keybindsPanel;
    [SerializeField] RectTransform movementKeys;
    [SerializeField] RectTransform abilityKeys;
    [SerializeField] RectTransform UIKeys;
    [SerializeField] RectTransform movementBlocker;
    #endregion
    #region VideoSettinsFields
    private bool fullscreen = false;
    private int resolutionX = 800;
    private int resolutionY = 600;
    [SerializeField] Text maxFpsText;
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Toggle vSyncToggle;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider fpsSlider;
    #endregion

    #region KeybindsFields
    #endregion
    #endregion

    private void OnDestroy()
    {
        KeyHandler.keyPressed -= OnKeyPressed;
        KeybindManager.keyChanged -= OnKeyChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        var settingsList = dataBase.GetSettings();
        foreach (Setting setting in settingsList)
        {
            switch (setting.name)
            {
                case "FPS":
                    fpsSlider.value = Convert.ToInt32(setting.value);
                    ChangeFps(fpsSlider);
                    break;
                case "VSync":
                    vSyncToggle.isOn = Convert.ToBoolean(setting.value);
                    ChangeVsync(vSyncToggle);
                    break;
                case "Resolution":
                    resolutionDropdown.value = Convert.ToInt32(setting.value);
                    ChangeResolution(resolutionDropdown);
                    break;
                case "Fullscreen":
                    fullscreenToggle.isOn = Convert.ToBoolean(setting.value);
                    ChangeFullscreen(fullscreenToggle);
                    break;

            }
        }


        KeyHandler.keyPressed += OnKeyPressed;
        KeybindManager.keyChanged += OnKeyChanged;
        int movY = -50, ablY = -50, UIY = -50;

        movementKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(190, KeyHandler.movementKeys.Length * 50 + 25);
        abilityKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(190, KeyHandler.abilityKeys.Length * 50 + 25);
        UIKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(190, KeyHandler.uiKeys.Length * 50 + 25);

        foreach (KeyValuePair<string, KeyCode> pair in KeyHandler.instance.GetKeybinds())
        {
            if (pair.Key == "EscapeMenu") continue;
            GameObject keybind = Instantiate(Keybind, new Vector3(0, 100, 0), Quaternion.identity);
            if (KeyHandler.movementKeys.Contains(pair.Key)){
                keybind.transform.SetParent(movementKeys, false);
                keybind.transform.localPosition = new Vector2(0, movY);
                movY -= 50;
            }
            else if (KeyHandler.abilityKeys.Contains(pair.Key)){
                
                keybind.transform.SetParent(abilityKeys, false);
                keybind.transform.localPosition = new Vector2(0, ablY);
                ablY -= 50;
            }
            else if (KeyHandler.uiKeys.Contains(pair.Key))
            {
                keybind.transform.SetParent(UIKeys, false);
                keybind.transform.localPosition = new Vector2(0, UIY);
                UIY -= 50;
            }
            
            keybind.GetComponent<KeybindManager>().SetKey(pair.Value);
            keybind.GetComponent<KeybindManager>().SetName(pair.Key);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnKeybindMovementToggle(Toggle keybindMvmToggle)
    {
        movementBlocker.gameObject.SetActive(!keybindMvmToggle.isOn);
        keybindMovementToggled?.Invoke(keybindMvmToggle.isOn);
    }

    void OnKeyPressed(string name, KeyCode keys)
    {
        if (!settingsPanel) return;
        if (name == "EscapeMenu")
        {
            if (!settingsPanel.activeSelf) return;
            settingsPanel.SetActive(false);
            KeyHandler.instance.Pause(false);
        }
    }
    void OnKeyChanged(KeybindManager keybind, string name, KeyCode key)
    {
        if (!KeyHandler.instance.SetKeybind(name, key))
        {
            keybind.ResetKeybind();
        }
    }
    #region VideoSettingsManager
    public void ChangeFullscreen(Toggle fullscreenToggle)
    {
        fullscreen = fullscreenToggle.isOn;
        dataBase.SetSetting("Fullscreen", fullscreen.ToString());
        SetScreenResolution();
    }

    public void ChangeResolution(Dropdown dropdown)
    {
        dropdown.Hide();
        string[] resolution = dropdown.captionText.text.ToString().Split('x'); ;
        Debug.Log(string.Join(" ", resolution));
        int.TryParse(resolution[0], out resolutionX);
        int.TryParse(resolution[1], out resolutionY);
        dataBase.SetSetting("Resolution", dropdown.value.ToString());
        SetScreenResolution();
    }
    public void ChangeFps(Slider fpsSlider)
    {
        Application.targetFrameRate = int.Parse(fpsSlider.value.ToString());
        dataBase.SetSetting("FPS", fpsSlider.value.ToString());
        maxFpsText.text = fpsSlider.value.ToString();
    }
    
    public void ChangeVsync(Toggle vsyncToggle)
    {
        QualitySettings.vSyncCount = Convert.ToInt32(vsyncToggle.isOn);
        dataBase.SetSetting("VSync", vsyncToggle.isOn.ToString());
    }

    private void SetScreenResolution()
    {
        Screen.SetResolution(resolutionX, resolutionY, fullscreen);
    }
    #endregion
    #region PanelsManager
    public void SwitchPanels(GameObject toPanel)
    {
        videoSettingsPanel.SetActive(false);
        midSettingsPanel.SetActive(false);
        keybindsPanel.SetActive(false);
        toPanel.SetActive(true);
    }
    #endregion
}
