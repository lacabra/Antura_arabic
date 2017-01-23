﻿using UnityEngine;

namespace EA4S.Minigames.HideAndSeek
{
    public class HideAndSeekTreeController : MonoBehaviour
    {
		public delegate void TouchAction(int i);
		public static event TouchAction onTreeTouched;
        
	    void OnMouseDown()
		{
			if (onTreeTouched != null)
            {
				onTreeTouched (id);
			}
		}
		
		public int id;
    }

}
