using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
	public static GameEvents Instance;

	private void Awake()
	{
		Instance = this;
	}

	public event Action<string, int> OnItemCollected;
	public void ItemCollected(string s, int am)
	{
		OnItemCollected?.Invoke(s, am);
	}

	public event Action OnNewDay;
	public void NewDay()
	{
		OnNewDay?.Invoke();
	}

	public event Action OnSaveInitiated;
	public void SaveInitiated()
	{
		OnSaveInitiated?.Invoke();
	}
}