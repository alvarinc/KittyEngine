using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.ConsoleKeyboard
{
    public class ConsoleKeyToWindowsKeyConverter
    {
        private readonly Dictionary<ConsoleKey, Key> _consoleKeyToWindowsKeyMap;
        private readonly Dictionary<Key, ConsoleKey> _windowsKeyToConsoleKeyMap;

        public ConsoleKeyToWindowsKeyConverter()
        {
            _consoleKeyToWindowsKeyMap = new Dictionary<ConsoleKey, Key>();
            _windowsKeyToConsoleKeyMap = new Dictionary<Key, ConsoleKey>();

            PopulateMappings();
        }

        /// <summary>
        /// Converts a ConsoleKey to a Windows Key.
        /// </summary>
        public Key? Convert(ConsoleKey consoleKey)
        {
            if (_consoleKeyToWindowsKeyMap.TryGetValue(consoleKey, out var windowsKey))
            {
                return windowsKey;
            }
            return null;
        }

        /// <summary>
        /// Converts a set of ConsoleKey to a set of Windows Key.
        /// </summary>
        public List<Key> Convert(IEnumerable<ConsoleKey> consoleKeys)
        {
            var keys = new List<Key>();

            foreach (var consoleKey in consoleKeys)
            {
                var key = Convert(consoleKey);
                if (key.HasValue)
                {
                    keys.Add(key.Value);
                }
            }

            return keys;
        }

        /// <summary>
        /// Converts a Windows Key to a ConsoleKey.
        /// </summary>
        public ConsoleKey? Convert(Key windowsKey)
        {
            if (_windowsKeyToConsoleKeyMap.TryGetValue(windowsKey, out var consoleKey))
            {
                return consoleKey;
            }
            return null;
        }

        /// <summary>
        /// Converts a set of Windows Key to a set of ConsoleKey.
        /// </summary>
        public List<ConsoleKey> Convert(IEnumerable<Key> keys)
        {
            var consoleKeys = new List<ConsoleKey>();

            foreach (var key in keys)
            {
                var consoleKey = Convert(key);
                if (consoleKey.HasValue)
                {
                    consoleKeys.Add(consoleKey.Value);
                }
            }

            return consoleKeys;
        }

        private void PopulateMappings()
        {
            AddMapping(ConsoleKey.None, Key.None);

            // Letters (A-Z)
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var consoleKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), c.ToString());
                var windowsKey = (Key)Enum.Parse(typeof(Key), c.ToString());
                AddMapping(consoleKey, windowsKey);
            }

            // Digits (0-9)
            for (int i = 0; i <= 9; i++)
            {
                var consoleKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), $"D{i}");
                var windowsKey = (Key)Enum.Parse(typeof(Key), $"D{i}");
                AddMapping(consoleKey, windowsKey);
            }

            // NumPad Digits (0-9)
            for (int i = 0; i <= 9; i++)
            {
                var consoleKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), $"NumPad{i}");
                var windowsKey = (Key)Enum.Parse(typeof(Key), $"NumPad{i}");
                AddMapping(consoleKey, windowsKey);
            }

            // Function Keys (F1-F12)
            for (int i = 1; i <= 12; i++)
            {
                var consoleKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), $"F{i}");
                var windowsKey = (Key)Enum.Parse(typeof(Key), $"F{i}");
                AddMapping(consoleKey, windowsKey);
            }

            // Special Keys
            AddMapping(ConsoleKey.Backspace, Key.Back);
            AddMapping(ConsoleKey.Tab, Key.Tab);
            AddMapping(ConsoleKey.Clear, Key.Clear);
            AddMapping(ConsoleKey.Enter, Key.Enter);
            AddMapping(ConsoleKey.Pause, Key.Pause);
            AddMapping(ConsoleKey.Escape, Key.Escape);
            AddMapping(ConsoleKey.Spacebar, Key.Space);
            AddMapping(ConsoleKey.PageUp, Key.PageUp);
            AddMapping(ConsoleKey.PageDown, Key.PageDown);
            AddMapping(ConsoleKey.End, Key.End);
            AddMapping(ConsoleKey.Home, Key.Home);

            // Arrow Keys
            AddMapping(ConsoleKey.LeftArrow, Key.Left);
            AddMapping(ConsoleKey.UpArrow, Key.Up);
            AddMapping(ConsoleKey.RightArrow, Key.Right);
            AddMapping(ConsoleKey.DownArrow, Key.Down);

            // Other special Keys
            AddMapping(ConsoleKey.Select, Key.Select);
            AddMapping(ConsoleKey.Print, Key.Print);
            AddMapping(ConsoleKey.Execute, Key.Execute);
            AddMapping(ConsoleKey.PrintScreen, Key.PrintScreen);
            AddMapping(ConsoleKey.Insert, Key.Insert);
            AddMapping(ConsoleKey.Delete, Key.Delete);
            AddMapping(ConsoleKey.Help, Key.Help);

            // Symbols and Other Keys
            AddMapping(ConsoleKey.Sleep, Key.Sleep);
            AddMapping(ConsoleKey.Multiply, Key.Multiply);
            AddMapping(ConsoleKey.Add, Key.Add);
            AddMapping(ConsoleKey.Subtract, Key.Subtract);
            AddMapping(ConsoleKey.Decimal, Key.Decimal);
            AddMapping(ConsoleKey.Divide, Key.Divide);

            AddMapping(ConsoleKey.Oem1, Key.Oem1);
            AddMapping(ConsoleKey.OemPlus, Key.OemPlus);
            AddMapping(ConsoleKey.OemComma, Key.OemComma);
            AddMapping(ConsoleKey.OemMinus, Key.OemMinus);
            AddMapping(ConsoleKey.OemPeriod, Key.OemPeriod);
            AddMapping(ConsoleKey.Oem2, Key.Oem2);
            AddMapping(ConsoleKey.Oem3, Key.Oem3);
            AddMapping(ConsoleKey.Oem4, Key.Oem4);
            AddMapping(ConsoleKey.Oem5, Key.Oem5);
            AddMapping(ConsoleKey.Oem6, Key.Oem6);
            AddMapping(ConsoleKey.Oem7, Key.Oem7);
            AddMapping(ConsoleKey.Oem8, Key.Oem8);
            AddMapping(ConsoleKey.Oem102, Key.Oem102);

            AddMapping(ConsoleKey.CrSel, Key.CrSel);
            AddMapping(ConsoleKey.ExSel, Key.ExSel);
            AddMapping(ConsoleKey.EraseEndOfFile, Key.EraseEof);
            AddMapping(ConsoleKey.Play, Key.Play);
            AddMapping(ConsoleKey.Zoom, Key.Zoom);
            AddMapping(ConsoleKey.NoName, Key.NoName);
            AddMapping(ConsoleKey.Pa1, Key.Pa1);
            AddMapping(ConsoleKey.OemClear, Key.OemClear);
        }

        private void AddMapping(ConsoleKey consoleKey, Key windowsKey)
        {
            // Add both mappings for bi-directional conversion
            _consoleKeyToWindowsKeyMap[consoleKey] = windowsKey;
            _windowsKeyToConsoleKeyMap[windowsKey] = consoleKey;
        }
    }
}
