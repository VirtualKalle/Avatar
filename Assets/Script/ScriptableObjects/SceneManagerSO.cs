using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneManagerSO", menuName = "SceneManagerSO", order = 1)]
public class SceneManagerSO : ScriptableObject {

    [SerializeField] public string[] Scenes;

    public void LoadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }

}
