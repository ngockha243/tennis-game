using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button easyBtn;
    public Button hardBtn;
    private void Start() {
        easyBtn.onClick.AddListener(delegate() {LoadGame("easy"); });
        hardBtn.onClick.AddListener(delegate() {LoadGame("hard"); });
    }
    public void LoadGame(string input)
    {
        StateName.difficulty = input;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
