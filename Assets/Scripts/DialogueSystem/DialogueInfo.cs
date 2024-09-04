using System;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class DialogueInfo
    {
        [SerializeField] private TextAsset _inkJSON;
        [SerializeField] private string _characterName;
        [SerializeField] private Sprite _characterPortrait;

        public TextAsset InkJSON => _inkJSON;
        public string CharacterName => _characterName;
        public Sprite CharacterPortrait => _characterPortrait;
    }
}