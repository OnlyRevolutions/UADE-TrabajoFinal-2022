﻿using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
	public class DeadlineAvatar : MonoBehaviour
	{
		public Character NaomiPrefab;
		public Character JulesPrefab;
		public Sprite NaomiAvatar;
		public Sprite JulesAvatar;
		
		protected virtual void Start()
		{
			if (GameManager.Instance.StoredCharacter.name == JulesPrefab.name)
			{
				gameObject.GetComponent<Image>().sprite = JulesAvatar;
			}
			else
			{
				gameObject.GetComponent<Image>().sprite = NaomiAvatar;
			}
		}
	}	
}

