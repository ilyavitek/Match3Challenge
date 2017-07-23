using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] Slider m_musicVolumeSlider;
	[SerializeField] Slider m_soundVolumeSlider;

	#endregion

	#region Behaviour Overrides

	void Start () {
		SoundManager.PlayMusic ("Background");

		m_musicVolumeSlider.value = SoundManager.GetMusicVolume ();
		m_soundVolumeSlider.value = SoundManager.GetSoundVolume ();
	}

	void OnEnable () {
		GameController.NewCombinationEvent += NewCombinationSound;
		GameController.NewTileSelectedEvent += NewTileSelected;

		Score.NewHighScoreEvent += HighScore;
	}

	void OnDisable () {
		GameController.NewCombinationEvent -= NewCombinationSound;
		GameController.NewTileSelectedEvent -= NewTileSelected;

		Score.NewHighScoreEvent -= HighScore;
	}

	#endregion

	#region Public Methods

	public void ClickSound () {
		SoundManager.PlaySound ("Click", false, 0.7f);
	}

	public void SetMusicVolume (float volume) {
		SoundManager.SetMusicVolume (volume);
	}

	public void SetSoundVolume (float volume) {
		SoundManager.SetSoundVolume (volume);
	}

	#endregion

	#region Private Methods

	void NewCombinationSound (int amount) {
		SoundManager.PlaySound ("NewCombination", false);
	}

	void NewTileSelected () {
		SoundManager.PlaySound ("NewTileSelected", false);
	}

	void HighScore (int amount) {
		SoundManager.PlaySound ("HighScore", false);
		Score.NewHighScoreEvent -= HighScore;
	}

	#endregion
}
