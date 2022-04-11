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
    #region PanelsFields
    [SerializeField] RectTransform settingsPanel;
    [SerializeField] RectTransform videoSettingsPanel;
    [SerializeField] RectTransform midSettingsPanel;
    [SerializeField] RectTransform keybindsPanel;
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
    #endregion

    #region KeybindsFields
    #endregion
  #endregion

    // Start is called before the first frame update
    void Start()
    {
        KeyHandler.keyPressed += OnKeyPressed;
        KeybindManager.keyChanged += OnKeyChanged;
        int movY = -50, ablY = -50, UIY = -50;

        movementKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(330, KeyHandler.movementKeys.Length * 100 + 50);
        abilityKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(330, KeyHandler.abilityKeys.Length * 100 + 50);
        UIKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(330, KeyHandler.uiKeys.Length * 100 + 50);

        foreach (KeyValuePair<string, KeyCode> pair in KeyHandler.instance.GetKeybinds())
        {
            if (pair.Key == "EscapeMenu") continue;
            GameObject keybind = Instantiate(Keybind, new Vector3(0, 100, 0), Quaternion.identity);
            if (KeyHandler.movementKeys.Contains(pair.Key)){
                keybind.transform.SetParent(movementKeys, false);
                keybind.transform.localPosition = new Vector2(0, movY);
                movY -= 100;
            }
            else if (KeyHandler.abilityKeys.Contains(pair.Key)){
                
                keybind.transform.SetParent(abilityKeys, false);
                keybind.transform.localPosition = new Vector2(0, ablY);
                ablY -= 100;
            }
            else if (KeyHandler.uiKeys.Contains(pair.Key))
            {
                keybind.transform.SetParent(UIKeys, false);
                keybind.transform.localPosition = new Vector2(0, UIY);
                UIY -= 100;
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
        if (name == "EscapeMenu")
        {
            if (!settingsPanel.gameObject.activeSelf) return;
            settingsPanel.gameObject.SetActive(false);
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
    public void OnScroll(Vector2 vector)
    {
        Debug.Log(vector.ToString());
    }
    #region VideoSettingsManager
    public void ChangeFullscreen(Toggle fullscreenToggle)
    {
        fullscreen = fullscreenToggle.isOn;
        SetScreenResolution();
    }

    public void ChangeResolution(Text dropdownItemText)
    {
        string[] resolution = dropdownItemText.text.ToString().Split('x'); ;
        Debug.Log(string.Join(" ", resolution));
        int.TryParse(resolution[0], out resolutionX);
        int.TryParse(resolution[1], out resolutionY);
        SetScreenResolution();
    }
    public void ChangeFps(Slider fpsSlider)
    {
        Application.targetFrameRate = int.Parse(fpsSlider.value.ToString());
        maxFpsText.text = "Max FPS: " + fpsSlider.value.ToString();
    }
    
    public void ChangeVsync(Toggle vsyncToggle)
    {
        QualitySettings.vSyncCount = Convert.ToInt32(vsyncToggle.isOn);
    }

    private void SetScreenResolution()
    {
        Screen.SetResolution(resolutionX, resolutionY, fullscreen);
    }
    #endregion
    #region PanelsManager
    public void SwitchPanels(RectTransform toPanel)
    {
        videoSettingsPanel.gameObject.SetActive(false);
        midSettingsPanel.gameObject.SetActive(false);
        keybindsPanel.gameObject.SetActive(false);
        toPanel.gameObject.SetActive(true);
    }
    #endregion
}
