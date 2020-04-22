using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
	public static event Action<string, int> OnItemCollected;
	public static void ItemCollected(string s, int am)
	{
		OnItemCollected?.Invoke(s, am);
	}

	public static event Action OnNewDay;
	public static void NewDay()
	{
		OnNewDay?.Invoke();
	}

	public static event Action OnSaveInitiated;
	public static void SaveInitiated()
	{
		OnSaveInitiated?.Invoke();
	}

	public static event Action<Transform> OnPlayerSelected;
	public static void PlayerSelected(Transform target)
	{
		OnPlayerSelected?.Invoke(target);
	}
}