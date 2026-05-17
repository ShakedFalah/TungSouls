using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VariableEvent", menuName = "Scriptable Objects/VariableEvent")]
public class VariableChangeEventSO<T> : ScriptableObject
{
	[SerializeField] private T value;
	[SerializeField] private T defaultValue;

	public event UnityAction<T> OnValueChanged = delegate { };
	public T Value
	{
		get => value;
		set
		{
			this.value = value;
			OnValueChanged.Invoke(value);
		}
	}

	private void OnEnable()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
	}

	private void OnDisable()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
	}

#if UNITY_EDITOR
	private void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
	{
		if (state == UnityEditor.PlayModeStateChange.EnteredEditMode)
		{
			value = defaultValue;
			OnValueChanged.Invoke(value);
		}
	}
#endif
}