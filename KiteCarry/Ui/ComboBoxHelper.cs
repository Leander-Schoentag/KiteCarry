using System.Windows.Controls;
using WindowsInput.Native;

namespace KiteCarry.Ui
{
    internal class ComboBoxHelper
    {
        /// <summary>
        /// Populates ComboBoxes with virtual key codes and sets default selections
        /// </summary>
        public static void SetComboBoxKeyBinds(
            ComboBox autoKiteKey,
            ComboBox attackMoveClickKey,
            ComboBox moveClickKey
        )
        {
            var virtualKeyCodes = Enum.GetValues(typeof(VirtualKeyCode))
                .Cast<VirtualKeyCode>()
                .ToList();

            autoKiteKey.ItemsSource = virtualKeyCodes;
            attackMoveClickKey.ItemsSource = virtualKeyCodes;
            moveClickKey.ItemsSource = virtualKeyCodes;

            autoKiteKey.SelectedItem = VirtualKeyCode.SPACE;
            attackMoveClickKey.SelectedItem = VirtualKeyCode.VK_X;
            moveClickKey.SelectedItem = VirtualKeyCode.VK_C;
        }
    }
}
