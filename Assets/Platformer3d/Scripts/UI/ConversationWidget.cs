using TMPro;
using UnityEngine;

namespace Platformer3d.UI
{
	public class ConversationWidget : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _phraseText;

		public void SetText(string phrase) =>
			_phraseText.text = phrase;
	}
}