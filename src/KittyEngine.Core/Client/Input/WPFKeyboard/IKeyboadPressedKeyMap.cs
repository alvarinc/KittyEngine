using System.Windows.Input;

namespace KittyEngine.Core.Client.Input.WPFKeyboard
{
    public interface IKeyboadPressedKeyMap
    {
        Key[] GetPressedKeys();

        void RegisterKeyboardPressedKey(Key key);

        void RemoveKeyboardPressedKey(Key key);

        void Reset();
    }
}
