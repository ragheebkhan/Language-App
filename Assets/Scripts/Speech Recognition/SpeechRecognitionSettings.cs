using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecognitionSettings : MonoBehaviour
{
    private GCSpeechRecognition m_speechRecognition;
    public Dropdown LanguageDropdown;
    public Dropdown MicrophoneDeviceDropdown;
    public Button RefreshMicsButton;

    private void Start()
    {
        m_speechRecognition = GCSpeechRecognition.Instance;

        RefreshMicsButton.onClick.AddListener(RefreshMicsButtonOnClickHandler);

        RefreshMicsButtonOnClickHandler();

        if (LanguageDropdown == null)
        {
            return;
        }

        LanguageDropdown.onValueChanged.AddListener(x => PlayerPrefs.SetInt("language", x));
        if(!PlayerPrefs.HasKey("language"))
        {
            PlayerPrefs.SetInt("language", 51);
        }

        LanguageDropdown.ClearOptions();

        for (int i = 0; i < Enum.GetNames(typeof(Enumerators.LanguageCode)).Length; i++)
        {
            LanguageDropdown.options.Add(new Dropdown.OptionData(((Enumerators.LanguageCode)i).Parse()));
        }

        LanguageDropdown.value = PlayerPrefs.GetInt("language");

    }

    private void RefreshMicsButtonOnClickHandler()
    {
        m_speechRecognition.RequestMicrophonePermission(null);

        MicrophoneDeviceDropdown.ClearOptions();
        MicrophoneDeviceDropdown.AddOptions(m_speechRecognition.GetMicrophoneDevices().ToList());

        MicrophoneDevicesDropdownOnValueChangedEventHandler(0);
    }

    private void MicrophoneDevicesDropdownOnValueChangedEventHandler(int value)
    {
        if (!m_speechRecognition.HasConnectedMicrophoneDevices())
            return;
        m_speechRecognition.SetMicrophoneDevice(m_speechRecognition.GetMicrophoneDevices()[value]);
    }
}
