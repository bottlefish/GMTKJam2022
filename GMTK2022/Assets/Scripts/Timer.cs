using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isCountingDown;
	public int maxTime; //If counting up, this becomes a failsafe to prevent int overflow.
	public Text time;

	private bool _timerRunning;
	public bool TimerRunning {
		get { return _timerRunning; }
		set { _timerRunning = value; }
	}

	private float timer;
	private int minutes;
	private int seconds;
	private string niceTime;

	private bool isClockRunning;

	void Start () {
		if (isCountingDown){
			timer = maxTime;
		} else {
			timer = 0;
		}
		FormatTime();
	}

    // formatTime用于处理时间格式
	private void FormatTime(){
		minutes = Mathf.FloorToInt(timer/60f);
		seconds = Mathf.FloorToInt(timer - minutes * 60);
		niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		time.text = niceTime;
	}

    //countUp用于增加时间
	private void CountUp(){
		if (timer < maxTime){
			timer += Time.deltaTime;
		} else {
			timer = maxTime;
		}
		FormatTime();
	}

    //countDown用于减少时间
	private void CountDown(){
		if (timer > 0){
			timer -= Time.deltaTime;
		} else {
			timer = 0;
			GameManager.Instance.TriggerGameOver();
		}
		FormatTime();
	}
    
    //RunClock使用协程单独开线程来计时
	private IEnumerator RunClock(){
		while (TimerRunning){
			if (isCountingDown){
				CountDown();
			}
			else {
				CountUp();
			}
			yield return null;
		}
	}
	public void StartClock(){
		TimerRunning = true;
		StartCoroutine(RunClock());
	}
	public void StopClock(){
		TimerRunning = false;
	}

	public void ResetClock(){
		if (isCountingDown){
			timer = maxTime;
		} else {
			timer = 0;
		}
		FormatTime();
	}
}
