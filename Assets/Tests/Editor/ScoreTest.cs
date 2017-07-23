using System.IO;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[TestFixture]
[Category ("Score Tests")]
public class ScoreTest {
	[SetUp] 
	public void Init () {
		DeleteHighScoreFile ();
	}

	[Test]
	public void DefaultCountOfPointsShouldBeInited () {
		Score scr = new Score ();

		Assert.AreEqual (0, scr.CountOfPoints);
	}

	[Test]
	public void DefaultCountOfDestroyedTilesShouldBeInited () {
		Score scr = new Score ();

		Assert.AreEqual (0, scr.CountOfDestroyedTiles);
	}

	[Test]
	public void DefaultCountOfCombinationsShouldBeInited () {
		Score scr = new Score ();

		Assert.AreEqual (0, scr.CountOfCombinations);
	}

	[Test]
	public void DefaultHighScoreShouldBeInited () {
		Score scr = new Score ();
		Assert.AreEqual (0, scr.HighScoreOfPoints);
	}

	[Test]
	public void DefaultHighScoreShouldBeUpdated () {
		Score scr = new Score ();
		scr.NewCombination (3);
		Assert.AreEqual (300, scr.HighScoreOfPoints);
	}

	[Test]
	public void ScoreShouldBeAdded_0 () {
		Score scr = new Score ();
		scr.NewCombination (3);
		Assert.AreEqual (300, scr.CountOfPoints);
	}

	[Test]
	public void ScoreShouldBeAdded_1 () {
		Score scr = new Score ();
		scr.NewCombination (4);
		Assert.AreEqual (500, scr.CountOfPoints);
	}

	[Test]
	public void ScoreShouldBeAdded_2 () {
		Score scr = new Score ();
		scr.NewCombination (10);
		Assert.AreEqual (1700, scr.CountOfPoints);
	}

	[Test]
	public void ScoreShouldBeAdded_3 () {
		Score scr = new Score ();
		scr.NewCombination (50);
		Assert.AreEqual (9700, scr.CountOfPoints);
	}

	[TearDown] public void CleanHighScore () {
		DeleteHighScoreFile ();
	}

	void DeleteHighScoreFile () {
		if (File.Exists (Application.persistentDataPath + StaticManager.HIGHSCORE_DIRECTORY + StaticManager.HIGHSCORE_FILENAME)) {
			File.Delete (Application.persistentDataPath + StaticManager.HIGHSCORE_DIRECTORY + StaticManager.HIGHSCORE_FILENAME);
		}
	}
}
