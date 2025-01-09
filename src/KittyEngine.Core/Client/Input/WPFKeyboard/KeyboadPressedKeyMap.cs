using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFKeyboard
{
    public class KeyboadPressedKeyMap
    {
        protected HashSet<Key> _keyboardPressedKeys = new HashSet<Key>();

        public Key[] GetPressedKeys() => _keyboardPressedKeys.ToArray();

        public void RegisterKeyboardPressedKey(Key key)
        {
            if (!_keyboardPressedKeys.Contains(key))
            {
                _keyboardPressedKeys.Add(key);
            }
        }

        public void RemoveKeyboardPressedKey(Key key)
        {
            if (_keyboardPressedKeys.Contains(key))
            {
                _keyboardPressedKeys.Remove(key);
            }
        }

        public void Reset()
        {
            _keyboardPressedKeys.Clear();
        }
    }
}
