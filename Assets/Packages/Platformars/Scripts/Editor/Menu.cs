using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Platformars
{
	public class Menu : Editor
	{
		const string documentation = "https://imanda-syahrul-ramadhan.gitbook.io/platformars/";
		const string docMenu = "Tools/Platformars/Documentation";

		#region methods
		[MenuItem(docMenu, false, 30)]
		public static void OpenDocumentation()
		{
			Help.BrowseURL(documentation);
		}
		#endregion
	}
}

