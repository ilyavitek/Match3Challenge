using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] BoardSizePresets[] m_presetSizeButtons;

	[SerializeField] Button m_customButton;

	[SerializeField] InputField m_inputCustomX;
	[SerializeField] InputField m_inputCustomY;

	#endregion

	#region Private Vatiables

	Vector2 m_tempCustomSize;

	#endregion

	#region Behaviour Overrides

	void Start () {
		m_tempCustomSize = new Vector2 ();
		DeactivateCurrentSizeOption ();
	}

	#endregion

	#region Public Methods

	public void SaveX (int xSize) {
		BoardSizeController.Instance.SaveX (xSize);
	}

	public void SaveY (int ySize) {
		BoardSizeController.Instance.SaveY (ySize);
	}

	public void SetCustomX (string xSize) {
		int parsedSize = 0;
		int.TryParse (xSize, out parsedSize);
		if (parsedSize <= StaticManager.MAX_SIZE_OF_CUSTOM_COORDINATE) {
			if (parsedSize > 0) {
				m_tempCustomSize.x = parsedSize;
			}
		} else {
			m_inputCustomX.text = "";
			m_tempCustomSize.x = 0;
		}

		m_customButton.interactable = m_tempCustomSize.x > 0 && m_tempCustomSize.y > 0;
	}

	public void SetCustomY (string ySize) {
		int parsedSize = 0;
		int.TryParse (ySize, out parsedSize);

		if (parsedSize <= StaticManager.MAX_SIZE_OF_CUSTOM_COORDINATE) {
			if (parsedSize > 0) {
				m_tempCustomSize.y = parsedSize;
			}
		} else {
			m_inputCustomY.text = "";
			m_tempCustomSize.y = 0;
		}

		m_customButton.interactable = m_tempCustomSize.x > 0 && m_tempCustomSize.y > 0;
	}

	public void SaveCustomSize () {
		if (m_tempCustomSize.x > 0) {
			SaveX ((int)m_tempCustomSize.x);
		}

		if (m_tempCustomSize.y > 0) {
			SaveY ((int)m_tempCustomSize.y);
		}
	}

	#endregion

	#region Private Methods

	void DeactivateCurrentSizeOption () {
		Vector2 currentSize = BoardSizeController.Instance.SizeOfBoard;

		for (int i = 0; i < m_presetSizeButtons.Length; i++) {
			if (m_presetSizeButtons [i].IsCurrentSize (currentSize)) {
				m_presetSizeButtons [i].button.SetActive (false);
				return;
			}
		}
	}

	#endregion
}

[System.Serializable]
class BoardSizePresets {
	
	public GameObject button;
	public Vector2 size;

	public bool IsCurrentSize (Vector2 currentSize) {
		return currentSize == size;
	}

}
