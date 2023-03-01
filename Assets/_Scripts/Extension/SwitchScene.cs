using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


/// <summary>
/// Simple SceneSwitcher to tigger the load of a scene.
/// </summary>
public class SwitchScene : MonoBehaviour
{
    public float time = 10;
    public string sceneToLoad;

    /// <summary>
    /// Change Scene to the one selected.
    /// </summary>
    public void ChangeScene()
    {
        StartCoroutine(LoadSceneIn());
    }

    /// <summary>
    /// Coroutine for changing che scene in X seconds.
    /// </summary>
    /// <returns>IEnumerator Wait Time before the scene switch.</returns>
    private IEnumerator LoadSceneIn()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}