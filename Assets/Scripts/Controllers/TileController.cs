using UnityEngine;

public class TileController : MonoBehaviour {

	#region Public Properties

	public SpriteRenderer TileSpriteRenderer {
		get { return m_spriteRenderer; }
		set { m_spriteRenderer = value; }
	}

	public bool IsSelected {
		get { return m_isSelected; }
	}

	public Transform TileTransform {
		get { return m_transform; }
	}

	#endregion

	#region Private Vatiables

	bool m_isSelected;

	Animator m_animator;
	SpriteRenderer m_spriteRenderer;
	Transform m_transform;

	#endregion

	#region Behaviour Overrides

	void Start () {
		m_isSelected = false;
		m_animator = GetComponent<Animator> ();
		m_spriteRenderer = GetComponent<SpriteRenderer> ();
		m_transform = transform;
	}

	#endregion

	#region Public Methods

	public void SetSelected (bool selected) {
		m_spriteRenderer.color = selected ? Color.black : Color.white;
		m_isSelected = selected;
	}

	public void RemoveFromBoard () {
		m_animator.enabled = true;
	}

	public void Recylcle () {
		this.Recycle ();
	}

	public void OnDisable () {
		m_animator.enabled = false;
		m_animator.Rebind ();
		SetSelected (false);
		m_transform.localScale = Vector3.one;
	}

	#endregion
}
