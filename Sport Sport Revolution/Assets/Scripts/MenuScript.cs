using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public void loadHelp() { SceneManager.LoadScene("HelpMenu"); }
    public void quitGame() { Application.Quit(); }
    public void startGame() { SceneManager.LoadScene("DodgeBall"); }
    public void mainMenu() { SceneManager.LoadScene("MainMenu"); }
}
