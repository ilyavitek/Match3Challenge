using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class Score {

	#region Public Properties

	public int CountOfPoints {
		get { return m_countOfPoints; }
	}

	public int CountOfDestroyedTiles {
		get { return m_countOfDestroyedTiles; }
	}

	public int CountOfCombinations {
		get { return m_countOfCombinations; }
	}


	public int HighScoreOfPoints {
		get { return m_highScoreOfPoints; }
	}

	#endregion

	#region Delegates and Events

	public delegate void ScoreDelegate (int amount);

	public static event ScoreDelegate ScoreAddedEvent;

	public static event ScoreDelegate NewHighScoreEvent;

	#endregion

	#region Private Vatiables

	int m_countOfDestroyedTiles;
	int m_countOfPoints;
	int m_countOfCombinations;

	int m_highScoreOfPoints;

	#endregion

	#region Public Methods

	public Score () {
		m_countOfDestroyedTiles = 0;
		m_countOfPoints = 0;
		m_countOfCombinations = 0;
		LoadHighScore ();
	}

	public void NewCombination (int countOfDestroyedTiles) {
		int scoreToAdd = CountScoreToAdd (countOfDestroyedTiles);

		if (ScoreAddedEvent != null) {
			ScoreAddedEvent (scoreToAdd);
		}

		m_countOfPoints += scoreToAdd;
		m_countOfCombinations++;
		m_countOfDestroyedTiles += countOfDestroyedTiles;

		CheckHighScore ();
	}

	public void SaveHighScore () {
		if (Directory.Exists (Application.persistentDataPath + StaticManager.HIGHSCORE_DIRECTORY) == false)
			Directory.CreateDirectory (Application.persistentDataPath + StaticManager.HIGHSCORE_DIRECTORY);
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + StaticManager.HIGHSCORE_DIRECTORY + StaticManager.HIGHSCORE_FILENAME);
		HighScoreData data = new HighScoreData ();

		data.highscore = m_highScoreOfPoints;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void LoadHighScore () {
		if (File.Exists (Application.persistentDataPath + StaticManager.HIGHSCORE_DIRECTORY + StaticManager.HIGHSCORE_FILENAME)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + StaticManager.HIGHSCORE_DIRECTORY + StaticManager.HIGHSCORE_FILENAME, FileMode.Open);
			HighScoreData data = (HighScoreData)bf.Deserialize (file);
			file.Close ();

			m_highScoreOfPoints = data.highscore;
		} else {
			m_highScoreOfPoints = 0;
			SaveHighScore ();
		}
	}

	#endregion

	#region Private Methods

	void CheckHighScore () {
		if (m_countOfPoints > m_highScoreOfPoints) {
			m_highScoreOfPoints = m_countOfPoints;
			SaveHighScore ();

			if (NewHighScoreEvent != null) {
				NewHighScoreEvent (m_highScoreOfPoints);
			}
		}
	}

	int CountScoreToAdd (int countOfDestroyedTiles) {
		int scoreToAdd = 0;

		scoreToAdd += countOfDestroyedTiles * 100;

		if (countOfDestroyedTiles >= 4) {
			scoreToAdd += (countOfDestroyedTiles - 3) * 100;
		}

		return scoreToAdd;
	}

	#endregion

}

[Serializable]
class HighScoreData {
	public int highscore;
}
