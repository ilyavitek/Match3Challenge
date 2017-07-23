using UnityEngine;

public class BoardSizeController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] Vector2 m_sizeOfBoard;

	#endregion

	#region Public Properties

	public Vector2 SizeOfBoard {
		get { return m_sizeOfBoard; }
	}

	public float SizeOfTile {
		get { return m_sizeOfTile; }
	}

	public static BoardSizeController Instance {
		get { return m_instance; }
	}

	#endregion

	#region Private Vatiables

	static BoardSizeController m_instance;

	float m_sizeOfTile;

	#endregion

	#region Behaviour Overrides

	void Awake () {
		m_sizeOfTile = StaticManager.SIZE_OF_TILE;

		if (m_instance != null) {
			Destroy (this.gameObject);
		} else {
			m_instance = this;
		}

		DontDestroyOnLoad (transform.gameObject);
	}

	#endregion

	#region Public Methods

	public void SaveX (int xSize) {
		m_sizeOfBoard.x = xSize;
	}

	public void SaveY (int ySize) {
		m_sizeOfBoard.y = ySize;
	}

	#endregion

}
