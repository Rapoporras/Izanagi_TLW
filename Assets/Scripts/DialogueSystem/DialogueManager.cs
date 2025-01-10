using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils.CustomLogs;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _typingDuration = 0.04f;

        [Header("Global Variables")]
        [SerializeField] private string _fileName = "dialogueVariables.json";
        [SerializeField] private TextAsset _loadGlobalsJSON; 
        
        [Header("Dialogue UI")]
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private GameObject _continueIcon;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private Image _portraitImage;
        [SerializeField] private TextMeshProUGUI _charaterNameText;

        [Space(10)]
        [SerializeField] private Sprite _defaultPortraitImage;

        [Header("Choices")]
        [SerializeField] private GameObject[] _choices;

        private TextMeshProUGUI[] _choicesText;

        private DialogueVariables _dialogueVariables;
        private InkExternalFunctions _inkExternalFunctions;
        private Story _currentStory;

        private bool _isSelectingChoice;
        private bool _dialogueIsPlaying;
        private bool _canContinueToNextLine;

        private bool _inputPressed;

        private Coroutine _displayLineCoroutine;

        private static DialogueManager _instance;
        public static DialogueManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            _inputPressed = InputManager.UIActions.Interact.WasPressedThisFrame();
        }

        private void Start()
        {
            _dialogueVariables = new DialogueVariables(_loadGlobalsJSON, Application.persistentDataPath, _fileName);
            _inkExternalFunctions = new InkExternalFunctions();
            
            _dialogueIsPlaying = false;
            _dialoguePanel.SetActive(false);
            ResetDialogueUI();
            
            // initialize choices text
            _choicesText = new TextMeshProUGUI[_choices.Length];
            for (var i = 0; i < _choices.Length; i++)
            {
                _choicesText[i] = _choices[i].GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void OnEnable()
        {
            InputManager.UIActions.Interact.performed += ContinueStoryCallback;
        }

        private void OnDisable()
        {
            InputManager.UIActions.Interact.performed -= ContinueStoryCallback;
        }

        public void EnterDialogueMode(DialogueInfo dialogueInfo) // called from TextAsset listener
        {
            if (_dialogueIsPlaying) return;
            
            InputManager.DisablePlayerActions();
            InputManager.EnableUIActions();
            
            _currentStory = new Story(dialogueInfo.InkJSON.text);
            _dialogueVariables.StartListening(_currentStory);
            _inkExternalFunctions.Bind(_currentStory);
            _dialogueIsPlaying = true;

            if (dialogueInfo.CharacterPortrait)
                _portraitImage.sprite = dialogueInfo.CharacterPortrait;
            
            if (!string.IsNullOrEmpty(dialogueInfo.CharacterName))
                _charaterNameText.text = dialogueInfo.CharacterName;
            
            _dialoguePanel.SetActive(true);
            ContinueStory();
        }

        private void ExitDialogueMode()
        {
            _dialogueIsPlaying = false;
            _dialogueVariables.StopListening(_currentStory);
            _inkExternalFunctions.Unbind(_currentStory);
            _dialoguePanel.SetActive(false);
            ResetDialogueUI();
            
            InputManager.EnablePlayerActions();
        }

        private void ContinueStoryCallback(InputAction.CallbackContext context)
        {
            if (!_canContinueToNextLine) return;
            if (_isSelectingChoice) return;
            
            ContinueStory();
        }

        private void ContinueStory()
        {
            if (!_dialogueIsPlaying) return;
            
            if (_currentStory.canContinue)
            {
                if (_displayLineCoroutine != null)
                    StopCoroutine(_displayLineCoroutine);

                string nextLine = _currentStory.Continue();
                if (string.IsNullOrEmpty(nextLine) && !_currentStory.canContinue)
                    ExitDialogueMode();
                
                _displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
            }
            else
            {
                ExitDialogueMode();
            }
        }

        private IEnumerator DisplayLine(string line)
        {
            _dialogueText.text = line;
            _dialogueText.maxVisibleCharacters = 0;

            _continueIcon.SetActive(false);
            HideChoices();
            
            _canContinueToNextLine = false;

            bool isAddingRichText = false;

            foreach (var letter in line.ToCharArray())
            {
                if (_inputPressed && _dialogueText.text.Length >= 3) // InputManager.UIActions.Interact.WasPressedThisFrame()
                {
                    LogManager.Log("Finish dialogue - input", FeatureType.Dialogue);
                    _dialogueText.maxVisibleCharacters = line.Length;
                    break;
                }
                
                // check rich text
                if (letter == '<' || isAddingRichText)
                {
                    isAddingRichText = true;
                    if (letter == '>')
                    {
                        isAddingRichText = false;
                    }
                }
                else
                {
                    _dialogueText.maxVisibleCharacters++;
                    yield return new WaitForSeconds(_typingDuration);
                }
            }

            _canContinueToNextLine = true;
            _continueIcon.SetActive(true);
            DisplayChoices();
        }

        private void HideChoices()
        {
            foreach (var choice in _choices)
            {
                choice.SetActive(false);
            }
            
            StartCoroutine(SelectFirstChoice());
        }

        private void DisplayChoices()
        {
            List<Choice> currentChoices = _currentStory.currentChoices;
            if (currentChoices.Count == 0)
            {
                _isSelectingChoice = false;
                return;
            }

            if (currentChoices.Count > _choices.Length)
            {
                LogManager.LogError($"More choices were given than the UI can support. Number of choices given: {currentChoices.Count}", 
                    FeatureType.Dialogue);
            }

            int index = 0;
            foreach (var choice in currentChoices)
            {
                _choices[index].SetActive(true);
                _choicesText[index].text = choice.text;
                index++;
            }
            for (var i = index; i < _choices.Length; i++)
            {
                _choices[i].SetActive(false);
            }

            StartCoroutine(SelectFirstChoice());
        }

        private IEnumerator SelectFirstChoice()
        {
            _isSelectingChoice = true;
            
            EventSystem.current.SetSelectedGameObject(null);
            yield return null;
            EventSystem.current.SetSelectedGameObject(_choices[0]);
        }

        public void MakeChoice(int choiceIndex) // on click button choice event
        {
            if (_canContinueToNextLine)
            {
                _currentStory.ChooseChoiceIndex(choiceIndex);
            
                ContinueStory();
                _isSelectingChoice = false;
            }
        }

        private void ResetDialogueUI()
        {
            _dialogueText.text = "";
            _portraitImage.sprite = _defaultPortraitImage;
            _charaterNameText.text = "???";
        }

        public void SaveVariables()
        {
            _dialogueVariables.Save();
        }
    }
}