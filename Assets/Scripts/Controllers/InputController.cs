using UnityEngine;

public class InputController : MonoBehaviour {

	#region Delegates and Events

	public delegate void InputControllerTileDelegate (TileController tile);

	public static event InputControllerTileDelegate NewTileTouchedEvent;

	public delegate void InputControllerDelegate ();

	public static event InputControllerDelegate TryStartCombinationEvent;
	public static event InputControllerDelegate TryChangeCombinationEvent;
	public static event InputControllerDelegate TryFinishCombinationEvent;

	#endregion

	#region Private Vatiables

	Vector2 m_inputPos;
	RaycastHit2D m_hit;

	bool m_isInput;

	#endregion

	#region Behaviour Overrides

	void Start () {
		m_isInput = false;
	}

	void Update () {
		UpdateInput ();
	}

	#endregion

	#region Private Methods

	void UpdateInput () {
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN

		bool isMouseMoved = Input.GetAxisRaw ("Mouse X") != 0 || Input.GetAxisRaw ("Mouse Y") != 0;
		bool shouldCast = (isMouseMoved && m_isInput) || Input.GetMouseButtonDown (0);

		if (shouldCast) {
			Cast (Input.mousePosition);
		}

		if (Input.GetMouseButtonDown (0)) {
			m_isInput = true;
			TryStartCombinationEvent ();
		}

		if (Input.GetMouseButtonUp (0)) {
			m_isInput = false;
			TryFinishCombinationEvent ();
		}

		if (isMouseMoved && m_isInput) {
			TryChangeCombinationEvent ();
		}

		#else

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			TouchPhase touchPhase = touch.phase;
			bool shouldCast = touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Moved;

			if (shouldCast) {
				Cast (touch.position);
			}

			switch (touchPhase) {
				case TouchPhase.Began:
					m_isInput = true;
					TryStartCombinationEvent ();
					break;
				case TouchPhase.Moved:
					TryChangeCombinationEvent ();
					break;
				case TouchPhase.Ended:
					m_isInput = false;
					TryFinishCombinationEvent ();
					break;
				default:
					break;
			}
		}

		#endif
	}

	void Cast (Vector3 positionOfInput) {
		m_inputPos = Camera.main.ScreenToWorldPoint (positionOfInput);
		m_hit = Physics2D.Raycast (m_inputPos, positionOfInput);

		if (m_hit) {
			TileController tile = m_hit.transform.GetComponent<TileController> ();
			NewTileTouchedEvent (tile);
		}
	}

	#endregion
}
