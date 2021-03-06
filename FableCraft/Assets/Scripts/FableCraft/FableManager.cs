﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FableCraft
{
    public class FableManager : MonoBehaviour, IFableManager
    {
        [SerializeField] FableData _fableData;
        [SerializeField] TextMeshProUGUI _storyTextContainer;
        [SerializeField] GameObject _optionButtonsContainer, _optionButtonPrefab;

        Scene _currentScene;
        TextEffect _textEffect;
        GameObject _textEffectGO = null;      
        string _currentCheckpoint = "Checkpoint 1";
        int _selectedOptionPath = 0;      

        [HideInInspector] public Action<string> _checkpoint;
        [HideInInspector] public Action<string, string> _triggerDialogue;
        public bool _loadingGame = false;
        public bool _optionSelected = false;
        public string CurrentCheckpoint => _currentCheckpoint;
        public FableData FableData => _fableData;
        public static FableManager Instance { get; private set; }

        // Start is called before the first frame update
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
            
            #if UNITY_EDITOR
                        
            #else
                _fableData.LoadFable();
            #endif
        }

        void Start()
        {
            _loadingGame = true;
        }

        public void LoadCheckpoint()
        {
            if (_fableData._checkpointEvent != null)
                _checkpoint = _fableData._checkpointEvent;     
            _currentCheckpoint = _fableData.CurrentCheckpointName;
            StopAllCoroutines();         
            LoadBeat(_fableData.CurrentSceneAsset);
            _checkpoint?.Invoke(_currentCheckpoint);           
        }

        public void Checkpoint(string checkpointName, int option)
        {
            _currentCheckpoint = checkpointName;
            _fableData._checkpointEvent = _checkpoint;
            _fableData.CurrentCheckpointName = checkpointName;
            _fableData.CurrentScene = _currentScene;
            _fableData.CurrentPlayNode = option;
            _fableData.SaveFable();           
        }

        public void ChangeAttribute(string attribute, int value, bool assign)
        {
            for (int i = 0; i < _fableData.Attributes.Length; i++)
            {
                if (attribute == _fableData.Attributes[i].Name)
                {
                    if(assign)
                        _fableData.Attributes[i].Value = value;
                    else
                        _fableData.Attributes[i].Value += value;
                }
            }
        }
        
        FableAttribute GetAttribute(string attribute)
        {
            for (int i = 0; i < _fableData.Attributes.Length; i++)
            {
                if (attribute == _fableData.Attributes[i].Name)
                {
                    return _fableData.Attributes[i];
                }
            }

            return new FableAttribute();
        }

        public void LoadBeat(int sceneAssetIndex)
        {
            if (SceneManager.sceneCount > 0)
            {
                _fableData.SetSceneAsset(sceneAssetIndex);
            }
        }

        public void Play(StoryNode storyNode, int index)
        {
            if (_loadingGame) _loadingGame = false;
            _storyTextContainer.text = "";
            storyNode.Play(
                index,
                _storyTextContainer,
                _textEffect != null ?
                    _textEffectGO.GetComponent<TextEffect>() : null);
        }

        public void AddOption(string name, string connectedOptionName, int optionIndex, float width, float height)
        {
            if (_loadingGame) return;
            var optionBtn = Instantiate(_optionButtonPrefab, _optionButtonsContainer.transform);
            optionBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            optionBtn.transform.localPosition = new Vector3(-width, -height, 0f);
            optionBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            optionBtn.transform.localScale = new Vector3(1f, 1f, 1f);
            optionBtn.GetComponent<FableController>().ConnectedCheckpointName = connectedOptionName;
            optionBtn.GetComponent<FableController>().StoryOptionIndex = optionIndex;
        }

        public void HideOptions()
        {
            foreach (Transform go in _optionButtonsContainer.transform)
            {
                Destroy(go.gameObject);
            }
        }

        public void PlayMusic(int clip, float volume = 1)
        {
            FableAudioManager.Instance.PlayMusic(clip, volume);
        }

        public void PlaySfx(int clip, float volume = 1)
        {
            FableAudioManager.Instance.PlaySfx(clip, volume);
        }

        public void StopAudio()
        {
            FableAudioManager.Instance.StopAudio();
        }
        
        public void ChangeTextEffect(GameObject textEffect)
        {
            if (textEffect == null) return;
            Destroy(_textEffectGO);
            _textEffect = textEffect.GetComponent<TextEffect>();
            _textEffectGO = Instantiate(textEffect);
        }

        public void ChangeSprite(Sprite sprite, Color color, Material shader, int imageContainerIndex)
        {
            if (_loadingGame) return;
            var container = FableUIManager.Instance.GetUIElement(imageContainerIndex);
            container.GetComponent<Image>().color = color;
            if(sprite)
                container.GetComponent<Image>().sprite = sprite;
            if(shader)
                container.GetComponent<Image>().material = shader;
        }
        
        public enum UIElementType
        {
            Slider,
            Text,
        }
        public void ShowAttribute(string attributeName, UIElementType uiElementType, int uiElementIndex)
        {
            var att = _fableData.GetAttribute(attributeName);
            var container = FableUIManager.Instance.GetUIElement(uiElementIndex);

            switch (uiElementType)
            {
                case UIElementType.Slider:
                    container.GetComponent<Slider>().value = att.Value;
                    break;
                case UIElementType.Text:
                    container.GetComponent<TextMeshProUGUI>().text = $"{att.Name}: {att.Value}";
                    break;
            }
        }
    }
}
