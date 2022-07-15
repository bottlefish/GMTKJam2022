using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//一个单例，用来管理游戏进程
public class GameManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    public GameObject clock;
	public Animator transition;

	public static GameManager Instance;

	private bool _isGameRunning;
	public bool IsGameRunning{
		get { return _isGameRunning; }
		set { _isGameRunning = value; }
	}

	void Start () {
		if (Instance == null){
			Instance = this;
		}
		if (clock != null){
			clock.GetComponent<Timer>().StartClock();
		}
		IsGameRunning = true;
	}

	public void PauseGame(bool isPaused){
		IsGameRunning = !isPaused;
		if (clock != null){
			if (!IsGameRunning) {
				clock.GetComponent<Timer>().StopClock();
			} else {
				clock.GetComponent<Timer>().StartClock();
			}
		}
	}

	public void TriggerGameOver(){
		StartCoroutine(transitionScene(2f));
	}

	private IEnumerator transitionScene(float duration){
		transition.SetTrigger("FadeOut");
		float elapsed = 0;
		while (elapsed < duration){
			elapsed += Time.deltaTime;
			yield return null;
		}
		SceneManager.LoadSceneAsync("GameOver");
	}
}
