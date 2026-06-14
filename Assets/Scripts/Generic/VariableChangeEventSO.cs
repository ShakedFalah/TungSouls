using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VariableEvent")]
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
		value = defaultValue;
	}
}