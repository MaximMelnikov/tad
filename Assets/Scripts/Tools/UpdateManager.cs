using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateManager : MonoBehaviour
{
	public static UpdateManager Instance { get; private set; }

	public List<MonoBehaviourEX> _managedBoxes = new List<MonoBehaviourEX>();

	void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		for (int i = 0; i < _managedBoxes.Count; ++i)
		{
			//_managedBoxes[i].ManagedUpdate();
		}
	}

	public void RegisterUpdate(MonoBehaviourEX box)
	{
		_managedBoxes.Add(box);
	}

	public void UnregisterUpdate(MonoBehaviourEX box)
	{
		_managedBoxes.Remove(box);
	}
}