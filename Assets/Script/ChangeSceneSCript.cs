using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSceneSCript : MonoBehaviour
{
    public void ChangeScene (int SceneNumber)
    {
        SceneManager.LoadScene(SceneNumber);
    }
}
