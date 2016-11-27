﻿using UnityEngine;
using System.Collections;

namespace EA4S.Maze
{
	public class MazeArrow : MonoBehaviour {

		public bool tweenToColor = false;
		public bool pingPong = false;

		Renderer _renderer;
		// Use this for initialization
		void Start () {
			_renderer = GetComponent<Renderer> ();
		}

		// Update is called once per frame
		void Update () {
			if (!tweenToColor && !pingPong)
				return;

			if(tweenToColor)
				_renderer.material.color = Color.Lerp (_renderer.material.color, Color.green, Time.deltaTime * 2);
			else if(pingPong)
				_renderer.material.color = Color.Lerp (Color.red, Color.green, Mathf.PingPong(Time.time,1));
		}
	}
}

