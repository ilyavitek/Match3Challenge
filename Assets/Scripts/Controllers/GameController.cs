using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	#region Editor Variables

	[SerializeField] GameObject m_tilePrefab;

	[SerializeField] Sprite[] m_spirtes;

	[SerializeField] Color m_selectedColor;

	[SerializeField] LineRenderer m_line;

	[SerializeField] Transform m_board;

	[SerializeField] Transform m_bottomOfBoard;

	#endregion

	#region Delegates and Events

	public delegate void GameControllerAmountDelegate (int amount);

	public static event GameControllerAmountDelegate NewCombinationEvent;


	public delegate void GameControllerDelegate ();

	public static event GameControllerDelegate NewTileSelectedEvent;

	#endregion

	#region Private Vatiables

	TileController m_currentTile;

	List<TileController> m_selectedTiles;

	string m_choosedSprite;

	int m_countOfSelectedTiles;

	float m_sizeOfTile;

	Transform m_prevTile;

	Vector2 m_sizeOfBoard;

	#endregion

	#region Behaviour Overrides

	void OnEnable () {
		Time.timeScale = 1f;

		InputController.NewTileTouchedEvent += SetCurrentTile;

		InputController.TryStartCombinationEvent += TryStartCombination;
		InputController.TryChangeCombinationEvent += TryChangeCombination;
		InputController.TryFinishCombinationEvent += TryFinishCombination;
	}

	void OnDisable () {
		InputController.NewTileTouchedEvent -= SetCurrentTile;

		InputController.TryStartCombinationEvent -= TryStartCombination;
		InputController.TryChangeCombinationEvent -= TryChangeCombination;
		InputController.TryFinishCombinationEvent -= TryFinishCombination;
	}

	void Start () {
		m_countOfSelectedTiles = 0;
		m_selectedTiles = new List<TileController> ();

		InitBoard ();

		Camera.main.orthographicSize = (Screen.width > Screen.height) ? m_sizeOfTile * m_sizeOfBoard.y : m_sizeOfTile * m_sizeOfBoard.x;
	}

	#endregion

	#region Private Methods

	void InitBoard () {
		m_sizeOfBoard = BoardSizeController.Instance.SizeOfBoard;
		m_sizeOfTile = BoardSizeController.Instance.SizeOfTile;

		m_bottomOfBoard.position = Vector3.down * m_sizeOfBoard.y / 2 * m_sizeOfTile;

		for (int i = 0; i < m_sizeOfBoard.x; i++) {
			for (int j = 0; j < m_sizeOfBoard.y; j++) {
				SpawnTile (new Vector3 (i, j), true);
			}
		}
	}

	void TryStartCombination () {
		Time.timeScale = 0f;

		if (m_currentTile != null) {
			if (!m_currentTile.IsSelected) {
				m_choosedSprite = m_currentTile.TileSpriteRenderer.sprite.name;
				SelectTile (m_currentTile);
			}
		}
	}

	void TryChangeCombination () {
		if (m_currentTile != null) {
			if (IsTileCanBeSelected (m_currentTile)) {

				int indexOfTileInSelection = m_selectedTiles.IndexOf (m_currentTile);
				int selectedTilesCount = m_selectedTiles.Count;

				bool isCurentTileIsInSelection = indexOfTileInSelection != -1 && m_currentTile.IsSelected;

				if (isCurentTileIsInSelection) {
					bool isCurrentTileSelectedLast = indexOfTileInSelection == selectedTilesCount - 2;
					bool isSelectedMoreThanOneTile = selectedTilesCount > 1;

					if (isCurrentTileSelectedLast && isSelectedMoreThanOneTile) {
						RemoveLastInSelection ();
					}
				} else {
					SelectTile (m_currentTile);
				}
			}
		}
	}

	void TryFinishCombination () {
		ClearLine ();
		if (m_countOfSelectedTiles >= StaticManager.MIN_COUNT_SELECTED_TILES_TO_REMOVE_FROM_BOARD) {
			NewCombinationEvent (m_selectedTiles.Count);
			StartRemoveFromBoardAnimationAndSpawnNewTiles ();
		} else {
			CancelChoosing ();
		}

		m_selectedTiles.Clear ();

		m_countOfSelectedTiles = 0;
		m_choosedSprite = string.Empty;
		m_prevTile = null;

		m_currentTile = null;

		Time.timeScale = 1f;
	}

	void SelectTile (TileController tile) {
		tile.SetSelected (true);
		m_selectedTiles.Add (tile);
		m_prevTile = tile.TileTransform;
		m_countOfSelectedTiles++;
		AddToLine (tile);

		NewTileSelectedEvent ();
	}

	void RemoveLastInSelection () {
		m_selectedTiles [m_countOfSelectedTiles - 1].SetSelected (false);
		m_prevTile = m_currentTile.TileTransform;
		m_selectedTiles.Remove (m_selectedTiles [m_countOfSelectedTiles - 1]);
		m_countOfSelectedTiles--;
		RemoveLastFromLine ();
	}

	void StartRemoveFromBoardAnimationAndSpawnNewTiles () {
		for (int j = 0; j < m_selectedTiles.Count; j++) {
			SpawnTile (m_selectedTiles [j].TileTransform.position);
			m_selectedTiles [j].RemoveFromBoard ();
		}
	}

	void CancelChoosing () {
		foreach (TileController tile in m_selectedTiles) {
			tile.SetSelected (false);
		}
	}

	void AddToLine (TileController tile) {
		m_line.positionCount++;
		m_line.SetPosition (m_line.positionCount - 1, tile.TileTransform.position + Vector3.forward);
	}

	void RemoveLastFromLine () {
		m_line.positionCount--;
	}

	void ClearLine () {
		m_line.positionCount = 0;
	}

	void SpawnTile (Vector3 targetPos, bool isStartInit = false) {
		GameObject tile;
		if (isStartInit) {
			tile = m_tilePrefab.Spawn (m_board, new Vector3 (-(m_sizeOfBoard.x - 1) / 2 + targetPos.x, -(m_sizeOfBoard.y - 1) / 2 + targetPos.y) * m_sizeOfTile, Quaternion.identity);
		} else {
			tile = m_tilePrefab.Spawn (m_board, Vector3.up * (Camera.main.orthographicSize + (m_sizeOfBoard.x + 1) / 2 * m_sizeOfTile) + targetPos, Quaternion.identity);
		}

		tile.GetComponent<SpriteRenderer> ().sprite = m_spirtes [Random.Range (0, m_spirtes.Length)];
	}

	void SetCurrentTile (TileController tile) {
		m_currentTile = tile;
	}

	bool IsTileCanBeSelected (TileController tile) {
		bool isTileSpriteMatch = tile.TileSpriteRenderer.sprite.name == m_choosedSprite;

		if (isTileSpriteMatch) {
			Vector2 deltaPosFromPrev = new Vector2 ();
			deltaPosFromPrev.x = Mathf.Abs (tile.TileTransform.position.x - m_prevTile.position.x);
			deltaPosFromPrev.y = Mathf.Abs (tile.TileTransform.position.y - m_prevTile.position.y);

			bool isPositionMatch = deltaPosFromPrev.x <= 1.1f * m_sizeOfTile && deltaPosFromPrev.y <= 1.1f * m_sizeOfTile;

			return isPositionMatch;
		} else {
			return isTileSpriteMatch;
		}
	}

	#endregion
}
