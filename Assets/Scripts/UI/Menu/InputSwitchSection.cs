using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

using UI.Menu.AnimateMethods;

using System;

namespace UI.Menu
{
    public class InputSwitchSection : MonoBehaviour
    {
        [SerializeField] private Menu[] _menus;

        [Space]
        [SerializeField] private Color _selectedColor = Color.white;
        [SerializeField] private Color _unSelectedColor = Color.grey;
        
        [SerializeField] private int _buttonIndex;
        [SerializeField] private int _menuIndex;
        
        [SerializeField] private AnimateMethod _animateMethod;

        private bool _canInput = true;
        private PlayerMap _playerMap;
        
        private void Awake()
        {
            // Repaint all buttons to unselected,
            // and repaint current button (by _index) into selected
            foreach (var menu in _menus)
            {
                ToolsUI.RepaintButtonsColor(menu.Selectables, _unSelectedColor);
            }
            ToolsUI.RepaintButtonColor(_menus[_menuIndex].Selectables[_buttonIndex], _selectedColor);
            
            _playerMap = new PlayerMap();

            _playerMap.UI.UpDownSection.started += SwitchButton;
            _playerMap.UI.ToSide.started += IntoSide;

            _playerMap.UI.Select.started += OnSelect;
            
            _playerMap.UI.Enable();
        }

        private void IntoSide(InputAction.CallbackContext context)
        {
            if(!_canInput) return;
            
            if(_menus[_menuIndex].Selectables[_buttonIndex] is not Slider) return;
            ((Slider)_menus[_menuIndex].Selectables[_buttonIndex]).value += 1 * context.ReadValue<float>();
        }

        private void OnSelect(InputAction.CallbackContext context)
        {
            if(!_canInput) return;

            if (_menus[_menuIndex].Selectables[_buttonIndex] is Button)
            {
                (_menus[_menuIndex].Selectables[_buttonIndex] as Button)?.onClick.Invoke();
            }
        }

        private void SwitchButton(InputAction.CallbackContext context)
        {
            if(!_canInput) return;
            
            // Repaint previous button color to unselected
            ToolsUI.RepaintButtonColor(_menus[_menuIndex].Selectables[_buttonIndex], _unSelectedColor);

            var direction = context.ReadValue<float>();
            _buttonIndex = (int)Mathf.Repeat(_buttonIndex + direction, _menus[_menuIndex].Selectables.Length);
            
            // Repaint current button color to selected
            ToolsUI.RepaintButtonColor(_menus[_menuIndex].Selectables[_buttonIndex], _selectedColor);
        }

        private bool TryFindMenuIndex(string menuName, out int menuIndex)
        {
            for (var i = 0; i < _menus.Length; i++)
            {
                if (_menus[i].MenuName != menuName) continue;
                menuIndex = i;

                return true;
            }

            menuIndex = -1;
            return false;
        }

        public void _ChangeMenu(string menuName)
        {
            if (!TryFindMenuIndex(menuName, out var menuIndex))
            {
                Debug.LogError($"Menu: '{menuName}', doesn't exist!");
                return;
            }
            
            // Repaint to unselected button on last menu 
            ToolsUI.RepaintButtonColor(_menus[_menuIndex].Selectables[_buttonIndex], _unSelectedColor);
            
            var lastIndex = _menuIndex;
            
            _buttonIndex = 0;
            _menuIndex = menuIndex;
            
            _animateMethod.AnimateMenuCategory(_menus[lastIndex].SelectablesParent, 
                _menus[_menuIndex].SelectablesParent, () => { _canInput = false;} ,() => { _canInput = true;});

            // Repaint to selected button on new menu 
            ToggleCurrentMenuActive(true);
            ToolsUI.RepaintButtonColor(_menus[_menuIndex].Selectables[_buttonIndex], _selectedColor);
        }

        private void ToggleCurrentMenuActive(bool active)
        {
            _menus[_menuIndex].SelectablesParent?.gameObject.SetActive(active);
        }

        [Serializable]
        public class Menu
        {
            [field: SerializeField] public string MenuName { get; private set; }
            [field: SerializeField] public Selectable[] Selectables {get; private set;}
            [field: SerializeField] public RectTransform SelectablesParent { get; private set; }
        }
    }
}
