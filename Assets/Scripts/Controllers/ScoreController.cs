using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] Text m_scoreAmountText;
	[SerializeField] Text m_destroyedTilesAmountText;
	[SerializeField] Text m_combinationsAmountText;

	[SerializeField] Text m_amountScoreToAddText;
	[SerializeField] Animator m_scoreToAddAnimator;

	[SerializeField] Text m_highscoreTitleText;
	[SerializeField] Text m_highscoreAmountText;

	#endregion

	#region Private Vatiables

	Score m_score;

	float m_prevScoreValue;
	float m_prevHighScoreValue;

	float m_speedOfChangingScoreValue;
	float m_speedOfChangingHighScoreValue;

	bool m_isIncrementingScore;
	bool m_isIncrementingHighScore;

	#endregion

	#region Behaviour Overrides

	void Start () {
		m_prevScoreValue = 0;
		m_isIncrementingScore = false;
		m_isIncrementingHighScore = false;
		m_score = new Score ();
		m_prevHighScoreValue = m_score.HighScoreOfPoints;
		m_highscoreAmountText.text = m_score.HighScoreOfPoints.ToString ();
	}

	void Update () {
		if (m_isIncrementingScore) {
			m_prevScoreValue = ShowIncrementing (m_prevScoreValue, m_score.CountOfPoints, m_speedOfChangingScoreValue, m_scoreAmountText);
		}

		if (m_isIncrementingHighScore) {
			m_prevHighScoreValue = ShowIncrementing (m_prevHighScoreValue, m_score.HighScoreOfPoints, m_speedOfChangingHighScoreValue, m_highscoreAmountText);
		}
	}

	void OnEnable () {
		GameController.NewCombinationEvent += NewCombination;
		Score.ScoreAddedEvent += ShowAddedScore;
		Score.NewHighScoreEvent += UpdateHighScore;
	}

	void OnDisable () {
		GameController.NewCombinationEvent -= NewCombination;
		Score.ScoreAddedEvent -= ShowAddedScore;
		Score.NewHighScoreEvent -= UpdateHighScore;
	}

	#endregion

	#region Public Methods

	public void NewCombination (int amountOfTiles) {
		m_prevScoreValue = m_score.CountOfPoints;
		m_score.NewCombination (amountOfTiles);

		m_destroyedTilesAmountText.text = m_score.CountOfDestroyedTiles.ToString ();
		m_combinationsAmountText.text = m_score.CountOfCombinations.ToString ();
	}

	#endregion

	#region Private Methods

	void ShowAddedScore (int amount) {
		m_amountScoreToAddText.enabled = true;
		m_scoreToAddAnimator.enabled = true;
		m_amountScoreToAddText.text = string.Format ("+{0}", amount.ToString ());
		m_speedOfChangingScoreValue = amount / StaticManager.TIME_TO_SHOW_ADDED_SCORE;
		StartCoroutine (WaitToHideAddedScore (StaticManager.TIME_TO_SHOW_ADDED_SCORE));
	}

	void UpdateHighScore (int amount) {
		m_highscoreAmountText.fontStyle = FontStyle.Bold;
		m_highscoreTitleText.fontStyle = FontStyle.Bold;

		m_highscoreAmountText.color = Color.red;
		m_highscoreTitleText.color = Color.red;

		m_speedOfChangingHighScoreValue = (amount - m_prevHighScoreValue) / StaticManager.TIME_TO_SHOW_ADDED_SCORE;
		StartCoroutine (WaitToUpdateHighScoreText (StaticManager.TIME_TO_SHOW_ADDED_SCORE));
	}

	float ShowIncrementing (float currentViewValue, float targetViewValue, float speed, Text text) {
		if (currentViewValue < targetViewValue) {
			currentViewValue += speed * Time.deltaTime;
			text.text = ((int)currentViewValue).ToString ();
		}
		return currentViewValue;
	}

	IEnumerator WaitToHideAddedScore (float time) {
		m_isIncrementingScore = true;
		yield return new WaitForSeconds (time);
		m_amountScoreToAddText.enabled = false;
		m_scoreToAddAnimator.enabled = false;
		m_scoreToAddAnimator.Rebind ();
		m_isIncrementingScore = false;
		m_scoreAmountText.text = m_score.CountOfPoints.ToString ();
		m_prevScoreValue = m_score.CountOfPoints;
	}

	IEnumerator WaitToUpdateHighScoreText (float time) {
		m_isIncrementingHighScore = true;
		yield return new WaitForSeconds (time);
		m_isIncrementingHighScore = false;
		m_highscoreAmountText.text = m_score.HighScoreOfPoints.ToString ();
		m_prevHighScoreValue = m_score.HighScoreOfPoints;
	}

	#endregion
}
