using UnityEngine;
using UnityEngine.EventSystems;

namespace Platformer3d.UI.MenuSystem.Items
{
    public class MenuItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField]
        private bool _isSelected;
        [SerializeField]
        private string _commandId;
        [SerializeField]
        private int _selectionIndex;
        [SerializeField]
        private RectTransform _selectionMarker;

        private MenuComponent _parent;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnSelectedChanged(value);
            }
        }

        public string CommandId => _commandId;
        public int SelectionIndex => _selectionIndex;

        private void Start()
        {
            // Reassign for refreshing selection
            if (_isSelected)
            {
                IsSelected = _isSelected;
            }
        }

        private void OnSelectedChanged(bool value)
        {
            _selectionMarker.gameObject.SetActive(value);
        }

        public void SetParent(MenuComponent parent) =>
            _parent = parent;

        public void OnPointerEnter(PointerEventData eventData) =>
            _parent.OnItemPointerEntered(CommandId);

        public void OnPointerClick(PointerEventData eventData) =>
            _parent.OnItemPointerClicked();
    }
}