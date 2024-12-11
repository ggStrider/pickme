using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public static class ToolsUI
    {
        public static void RepaintButtonColor(Selectable button, Color color)
        {
            var colors = button.colors;
            colors.normalColor = color;
            button.colors = colors;
        }
        
        public static void RepaintButtonsColor(Selectable[] buttons, Color color)
        {
            var newColors = buttons[0].colors;
            newColors.normalColor = color;
            
            foreach (var button in buttons)
            {
                button.colors = newColors;
            }
        }
    }
}