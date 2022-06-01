using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Модуль управления настройками
/// </summary>
public class SettingsManager : MonoBehaviour
{
    #region Fields
    public static Action<bool> keybindMovementToggled;
    [SerializeField, Tooltip("Префаб для настроек управления")] GameObject Keybind;
    [SerializeField] DataBase dataBase;
    #region PanelsFields
    [SerializeField, Tooltip("Объект панели настроек")] GameObject settingsPanel;
    [SerializeField, Tooltip("Объект панели настроек видео")] GameObject videoSettingsPanel;
    [SerializeField, Tooltip("Объект панели настроек звука")] GameObject midSettingsPanel;
    [SerializeField, Tooltip("Объект панели настроек управления")] GameObject keybindsPanel;
    [SerializeField, Tooltip("Панель списка настроек управления движением")] RectTransform movementKeys;
    [SerializeField, Tooltip("Панель списка настроек управления способностями")] RectTransform abilityKeys;
    [SerializeField, Tooltip("Панель списка настроек управления UI")] RectTransform UIKeys;
    [SerializeField, Tooltip("Панель блокировки настроек движения при выключенном движении по кнопкам")] RectTransform movementBlocker;
    #endregion
    #region VideoSettinsFields
    private bool fullscreen = false;
    private int resolutionX = 800;
    private int resolutionY = 600;
    [SerializeField, Tooltip("Текстовое поле для значения целевых ФПС")] Text maxFpsText;
    [SerializeField, Tooltip("Выпадающий список для разрешений экрана")] Dropdown resolutionDropdown;
    [SerializeField, Tooltip("Переключатель вертикальной синхронизации")] Toggle vSyncToggle;
    [SerializeField, Tooltip("Переключатель статуса полнного экрана для окна игры")] Toggle fullscreenToggle;
    [SerializeField, Tooltip("Слайдер для изменения целевых ФПС")] Slider fpsSlider;
    #endregion

    #region KeybindsFields
    #endregion
    #endregion

    private void OnDestroy()
    {
        KeyHandler.keyPressed -= OnKeyPressed;
        KeybindManager.keyChanged -= OnKeyChanged;
    }

    /// <summary>
    /// Загрузка всех настроек
    /// </summary>
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
            if (KeyHandler.movementKeys.Contains(pair.Key))
            {
                keybind.transform.SetParent(movementKeys, false);
                keybind.transform.localPosition = new Vector2(0, movY);
                movY -= 50;
            }
            else if (KeyHandler.abilityKeys.Contains(pair.Key))
            {

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

    /// <summary>
    /// Фукнция для переключения движения по заданным кнопка (или заданным в движке юнити)
    /// </summary>
    /// <param name="keybindMvmToggle"></param>
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
    /// <summary>
    /// Функция длл события изменения заданной настройки управления
    /// </summary>
    /// <param name="keybind">Модуль управления настройкой</param>
    /// <param name="name">Название действия</param>
    /// <param name="key">новое значение ключа действия</param>
    void OnKeyChanged(KeybindManager keybind, string name, KeyCode key)
    {
        if (!KeyHandler.instance.SetKeybind(name, key))
        {
            keybind.ResetKeybind();
        }
    }
    #region VideoSettingsManager
    /// <summary>
    /// Функция для переключателя режима полного экрана окна игры в настройках видео
    /// </summary>
    /// <param name="fullscreenToggle">Целевой переключатель</param>
    public void ChangeFullscreen(Toggle fullscreenToggle)
    {
        fullscreen = fullscreenToggle.isOn;
        dataBase.SetSetting("Fullscreen", fullscreen.ToString());
        SetScreenResolution();
    }

    /// <summary>
    /// Фунция для выпадающего списка установки размеров окна игры в настройках видео
    /// </summary>
    /// <param name="dropdown">Целевой выпадающий список</param>
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
    /// <summary>
    /// Функция для слайдера целевых ФПС в настройках видео
    /// </summary>
    /// <param name="fpsSlider">Целевой слайдер</param>
    public void ChangeFps(Slider fpsSlider)
    {
        Application.targetFrameRate = int.Parse(fpsSlider.value.ToString());
        dataBase.SetSetting("FPS", fpsSlider.value.ToString());
        maxFpsText.text = fpsSlider.value.ToString();
    }
    /// <summary>
    /// Фукнция для переключателя Вертикальной синхронизации в настройках видео
    /// </summary>
    /// <param name="vsyncToggle">Переключатель настройки</param>
    public void ChangeVsync(Toggle vsyncToggle)
    {
        QualitySettings.vSyncCount = Convert.ToInt32(vsyncToggle.isOn);
        dataBase.SetSetting("VSync", vsyncToggle.isOn.ToString());
    }

    /// <summary>
    /// Установить разрешение экрана (на полный ли экран) по сохраненным значениям
    /// </summary>
    private void SetScreenResolution()
    {
        Screen.SetResolution(resolutionX, resolutionY, fullscreen);
    }
    #endregion
    #region PanelsManager
    /// <summary>
    /// Функция переключения панелей настроек
    /// </summary>
    /// <param name="toPanel">Панель, на которую необходимо переключиться</param>
    public void SwitchPanels(GameObject toPanel)
    {
        videoSettingsPanel.SetActive(false);
        midSettingsPanel.SetActive(false);
        keybindsPanel.SetActive(false);
        toPanel.SetActive(true);
    }
    #endregion
}
