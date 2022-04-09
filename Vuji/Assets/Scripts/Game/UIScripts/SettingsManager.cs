using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region Fields
    [SerializeField] KeyHandler keyHandler;
    [SerializeField] GameObject Keybind;
    #region PanelsFields
    [SerializeField] RectTransform settingsPanel;
    [SerializeField] RectTransform videoSettingsPanel;
    [SerializeField] RectTransform midSettingsPanel;
    [SerializeField] RectTransform keybindsPanel;
    [SerializeField] RectTransform movementKeys;
    [SerializeField] RectTransform abilityKeys;
    [SerializeField] RectTransform UIKeys;
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
        int movY = 100, ablY = 100, UIY = 100;

        movementKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(360, KeyHandler.movementKeys.Length * 100 + 50);
        abilityKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(360, KeyHandler.abilityKeys.Length * 100 + 50);
        UIKeys.GetComponent<RectTransform>().sizeDelta = new Vector2(360, KeyHandler.uiKeys.Length * 100 + 50);

        foreach (KeyValuePair<string, KeyCode[]> pair in keyHandler.GetKeybinds())
        {
            if (pair.Key == "EscapeMenu") continue;
            GameObject keybind = Instantiate(Keybind, new Vector3(0, 100, 0), Quaternion.identity);
            if (KeyHandler.movementKeys.Contains(pair.Key)){
                keybind.transform.SetParent(movementKeys, true);
                keybind.transform.position = new Vector3(keybind.transform.position.x - 360, keybind.transform.position.y + movY, keybind.transform.position.z);
                movY -= 100;
            }
            else if (KeyHandler.abilityKeys.Contains(pair.Key)){
                keybind.transform.SetParent(abilityKeys, true);
                keybind.transform.position = new Vector3(keybind.transform.position.x, keybind.transform.position.y + ablY, keybind.transform.position.z);
                ablY -= 100;
            }
            else if (KeyHandler.uiKeys.Contains(pair.Key))
            {
                keybind.transform.SetParent(UIKeys, true);
                keybind.transform.position = new Vector3(keybind.transform.position.x + 360, keybind.transform.position.y + UIY, keybind.transform.position.z);
                UIY -= 100;
            }
            
            keybind.GetComponent<KeybindManager>().SetKeys(pair.Value);
            keybind.GetComponent<KeybindManager>().SetName(pair.Key);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnKeyPressed(KeyHandler keyHandler, string name, KeyCode[] keys)
    {
        if (name == "EscapeMenu")
        {
            settingsPanel.gameObject.SetActive(false);
            keyHandler.Pause(false);
        }
    }
    void OnKeyChanged(KeybindManager keybind, string name, KeyCode[] keys)
    {
        if (!keyHandler.SetKeybind(name, keys))
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
