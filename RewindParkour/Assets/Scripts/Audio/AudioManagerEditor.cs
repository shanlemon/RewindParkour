using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor {

    public override void OnInspectorGUI() {
        if (GUILayout.Button("Load Sounds")) {
            LoadSounds(target as AudioManager);
        }
        
        GUILayout.Space(20);
        base.OnInspectorGUI();
    }


    /*
     * Called from a button on the inspector.
     * Automagically fills out the sounds array in the AudioManager.
     */
    private void LoadSounds(AudioManager audio) {
        Sound[] sounds = GetAllSounds();

        var serializedAudio = new SerializedObject(audio);
        var soundsProperty = serializedAudio.FindProperty("sounds");
        soundsProperty.ClearArray();
        soundsProperty.arraySize = sounds.Length;

        for (int s = 0; s < sounds.Length; s++) {
            var soundProperty = soundsProperty.GetArrayElementAtIndex(s);
            soundProperty.objectReferenceValue = sounds[s];
        }

        serializedAudio.ApplyModifiedProperties();
    }

    /*
     * Generates an array of all existing Sound scriptable objects from the project files.
     * Copied from Stack Overflow because I am a Professional
     */
    public static Sound[] GetAllSounds() {
        return AssetDatabase.FindAssets("t:" + typeof(Sound).Name)
            .Select(guid => AssetDatabase.LoadAssetAtPath<Sound>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();
    }
}