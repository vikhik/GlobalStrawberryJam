using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour {

	public float fadeDuration = 3f;
	public float textFadeDuration = 1f;
	public float textDisplayDuration = 1f;
	public float animationDuration = 5f;

	public string nextLevel = "Room01";
	private DescriptionText descriptionText;

	Image fadeImageUI;
	Text endingTextUI;

	// Use this for initialization
	void Start () {
		fadeImageUI = GetComponentInChildren<Image>();
		endingTextUI = GetComponentInChildren<Text>();

		StartCoroutine(FadeIn());
		descriptionText = FindObjectOfType<DescriptionText>();
	}

	private IEnumerator FadeIn() {

		float progress = 0;

		Color startColor = fadeImageUI.color;

		while (progress < fadeDuration) {
			progress += Time.deltaTime;
			fadeImageUI.color = Color.Lerp(startColor, Color.clear, progress / fadeDuration);

			yield return null;
		}

		descriptionText.displaytext = true;
		fadeImageUI.color = Color.clear;
	}

	public void endScene(string endingText, string targetlevel = null) {
		endingTextUI.text = endingText;

		if (targetlevel != null) {
			nextLevel = targetlevel;
		}

		StartCoroutine(FadeOut());
	}

	public void endGame(string endingText) {
		endingTextUI.text = endingText;

		nextLevel = "Credits";

		StartCoroutine(FadeDelay());
	}

	private IEnumerator FadeDelay() {

		float progress = 0;

		while (progress < animationDuration) {
			yield return null;
		}

		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut() {

		descriptionText.displaytext = false;
		float progress = 0;

		Color startColor = fadeImageUI.color;

		while (progress < fadeDuration) {
			progress += Time.deltaTime;
			fadeImageUI.color = Color.Lerp(startColor, Color.black, progress / fadeDuration);

			yield return null;
		}

		fadeImageUI.color = Color.black;


		StartCoroutine(TextIn());
	}

	private IEnumerator TextIn() {

		float progress = 0;

		Color startColor = endingTextUI.color;

		while (progress < textFadeDuration) {
			progress += Time.deltaTime;
			endingTextUI.color = Color.Lerp(startColor, Color.white, progress / textFadeDuration);

			yield return null;
		}

		endingTextUI.color = Color.white;

		StartCoroutine(DisplayText());
	}

	private IEnumerator DisplayText() {

		float progress = 0;

		while (progress < textDisplayDuration) {
			progress += Time.deltaTime;
			yield return null;
		}

		StartCoroutine(TextOut());
	}

	private IEnumerator TextOut() {

		float progress = 0;

		Color startColor = endingTextUI.color;

		while (progress < textFadeDuration) {
			progress += Time.deltaTime;
			endingTextUI.color = Color.Lerp(startColor, Color.clear, progress / textFadeDuration);

			yield return null;
		}

		endingTextUI.color = Color.clear;

		Application.LoadLevel(nextLevel);
	}
}
