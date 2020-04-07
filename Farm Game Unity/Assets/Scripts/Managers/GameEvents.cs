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

	public event Action<string, int> onItemCollected;
	public void ItemCollected(string s, int am)
	{
		if(onItemCollected != null)
		{
			onItemCollected(s, am);
		}
	}
}