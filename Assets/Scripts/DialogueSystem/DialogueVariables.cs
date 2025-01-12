using System;
using System.Collections.Generic;
using System.IO;
using Utils.CustomLogs;
using Ink.Runtime;
using UnityEngine;
using Path = System.IO.Path;

namespace DialogueSystem
{
    public class DialogueVariables
    {
        private Dictionary<string, Ink.Runtime.Object> _variables;

        private readonly Story _globalVariablesStory;
        private readonly string _varDirPath;
        private readonly string _varFileName;
        
        public DialogueVariables(TextAsset loadGlobalsInkJSON, string varDirPath, string varFileName)
        {
            _globalVariablesStory = new Story(loadGlobalsInkJSON.text);
            _varDirPath = varDirPath;
            _varFileName = varFileName;
            Load();
        }
        
        public void StartListening(Story story)
        {
            VariablesToStory(story);
            story.variablesState.variableChangedEvent += OnVariableChange;
        }

        public void StopListening(Story story)
        {
            story.variablesState.variableChangedEvent -= OnVariableChange;
        }

        public void Reset()
        {
            _globalVariablesStory.ResetState();
            _variables.Clear();
        }

        public void Save()
        {
            string fullPath = Path.Combine(_varDirPath, _varFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                VariablesToStory(_globalVariablesStory);
                string dataToStore = _globalVariablesStory.state.ToJson();

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Log($"Error occurred when trying to save variables: {fullPath} \n{e}",
                    FeatureType.Dialogue);
            }
        }

        private void Load()
        {
            string fullPath = Path.Combine(_varDirPath, _varFileName);
            if (File.Exists(fullPath))
            {
                try
                {
                    string json;
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            json = reader.ReadToEnd();
                        }
                    }
                    _globalVariablesStory.state.LoadJson(json);
                }
                catch (Exception e)
                {
                    LogManager.Log($"Error occured when trying to load variables from file: {fullPath} \n{e}",
                        FeatureType.Dialogue);
                }
            }
            
            _variables = new Dictionary<string, Ink.Runtime.Object>();
            foreach (var name in _globalVariablesStory.variablesState)
            {
                Ink.Runtime.Object value = _globalVariablesStory.variablesState.GetVariableWithName(name);
                _variables.Add(name, value);
            }
        }
        
        private void OnVariableChange(string name, Ink.Runtime.Object value)
        {
            LogManager.Log($"Variable changed: {name} = {value}", FeatureType.Dialogue);
            if (_variables.ContainsKey(name))
            {
                _variables.Remove(name);
                _variables.Add(name, value);
            }
        }

        private void VariablesToStory(Story story)
        {
            foreach (var variable in _variables)
            {
                story.variablesState.SetGlobal(variable.Key, variable.Value);
            }
        }
    }
}