using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SpeechRecognizer : MonoBehaviour
{
    private GCSpeechRecognition _speechRecognition;

    public Button _startRecordButton,
                   _stopRecordButton;

    public TMP_Text _resultText;

    public GameObject ObjectToNotify;

    private TextSimilarityAnalyzer _similarityAnalyzer = new();

    private void Awake()
    {
        _speechRecognition = GCSpeechRecognition.Instance;

        _speechRecognition.SetMicrophoneDevice(_speechRecognition.GetMicrophoneDevices()[0]);

        _speechRecognition.RecognizeSuccessEvent += RecognizeSuccessEventHandler;
        _speechRecognition.RecognizeFailedEvent += RecognizeFailedEventHandler;

        _speechRecognition.FinishedRecordEvent += FinishedRecordEventHandler;
        _speechRecognition.StartedRecordEvent += StartedRecordEventHandler;
        _speechRecognition.RecordFailedEvent += RecordFailedEventHandler;

        _speechRecognition.BeginTalkigEvent += BeginTalkigEventHandler;
        _speechRecognition.EndTalkigEvent += EndTalkigEventHandler;

        _startRecordButton.onClick.AddListener(StartRecordButtonOnClickHandler);
        _stopRecordButton.onClick.AddListener(StopRecordButtonOnClickHandler);

        _startRecordButton.interactable = true;
        _stopRecordButton.interactable = false;
    }

    private void OnDestroy()
    {
        _speechRecognition.RecognizeSuccessEvent -= RecognizeSuccessEventHandler;
        _speechRecognition.RecognizeFailedEvent -= RecognizeFailedEventHandler;
        _speechRecognition.FinishedRecordEvent -= FinishedRecordEventHandler;
        _speechRecognition.StartedRecordEvent -= StartedRecordEventHandler;
        _speechRecognition.RecordFailedEvent -= RecordFailedEventHandler;
        _speechRecognition.BeginTalkigEvent -= BeginTalkigEventHandler;
        _speechRecognition.EndTalkigEvent -= EndTalkigEventHandler;
    }

    private void StartRecordButtonOnClickHandler()
    {
        _startRecordButton.interactable = false;
        _stopRecordButton.interactable = true;
        _resultText.text = "Waiting For Speech";

        _speechRecognition.StartRecord(false);
    }

    private void StopRecordButtonOnClickHandler()
    {
        _stopRecordButton.interactable = false;
        _startRecordButton.interactable = true;

        _speechRecognition.StopRecord();
    }

    private void StartedRecordEventHandler()
    {

    }

    private void RecordFailedEventHandler()
    {
        _resultText.text = "<color=red>Start record Failed. Please check microphone device and try again.</color>";

        _stopRecordButton.interactable = false;
        _startRecordButton.interactable = true;
    }

    private void BeginTalkigEventHandler()
    {
        _resultText.text = "<color=blue>Speech Began.</color>";
    }

    private void EndTalkigEventHandler(AudioClip clip, float[] raw)
    {
        _resultText.text += "\n<color=blue>Speech Ended.</color>";

        FinishedRecordEventHandler(clip, raw);
    }

    private void FinishedRecordEventHandler(AudioClip clip, float[] raw)
    {

        if (clip == null)
            return;

        RecognitionConfig config = RecognitionConfig.GetDefault();
        config.languageCode = ((Enumerators.LanguageCode)PlayerPrefs.GetInt("language")).Parse();

        config.audioChannelCount = clip.channels;
        // configure other parameters of the config if need

        GeneralRecognitionRequest recognitionRequest = new GeneralRecognitionRequest
        {
            audio = new RecognitionAudioContent() // for base64 data
            {
                content = raw.ToBase64(channels: clip.channels)
            },
            //recognitionRequest.audio = new RecognitionAudioUri() // for Google Cloud Storage object
            //{
            //	uri = "gs://bucketName/object_name"
            //};
            config = config
        };

        _speechRecognition.Recognize(recognitionRequest);

    }
    private void RecognizeFailedEventHandler(string error)
    {
        _resultText.text = "Recognize Failed: " + error;
    }

    private void RecognizeSuccessEventHandler(RecognitionResponse recognitionResponse)
    {
        InsertRecognitionResponseInfo(recognitionResponse);
    }

    private void InsertRecognitionResponseInfo(RecognitionResponse recognitionResponse)
    {
        if (recognitionResponse == null || recognitionResponse.results.Length == 0)
        {
            _resultText.text = "Words not detected.";
            return;
        }

        _resultText.text = recognitionResponse.results[0].alternatives[0].transcript;
        if(ObjectToNotify != null)
            ObjectToNotify.SendMessage("DisplaySpeech", recognitionResponse.results[0].alternatives[0].transcript);
        Invoke("ClearText", 3);
    }

    void ClearText()
    {
        _resultText.text = "Waiting For Speech";
    }
}
