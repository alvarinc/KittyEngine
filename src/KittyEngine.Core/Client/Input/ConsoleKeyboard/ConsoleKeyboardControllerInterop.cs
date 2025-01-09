//------------------------------------------------------------------------------
//
// CommandManager initial implementation from :
// https://github.com/rdblue02/SideProjects/blob/main/BrickBreaker/ConsoleGameFrameWork/Input/CommandManager.cs
//
//------------------------------------------------------------------------------
using System.Runtime.InteropServices;

namespace KittyEngine.Core.Client.Input.ConsoleKeyboard
{
    public class ConsoleKeyboardControllerInterop
    {
        [DllImport("user32.dll")]
        static extern int GetKeyState(int key);

        private Dictionary<int, bool> _currentKeyBoardState;
        private Dictionary<int, bool> _previousKeyBoardState;

        public ConsoleKeyboardControllerInterop()
        {
            _currentKeyBoardState = new Dictionary<int, bool>();
            _previousKeyBoardState = new Dictionary<int, bool>();

            Reset();
        }

        public void Reset()
        {
            var allConsoleKeys = (ConsoleKey[])Enum.GetValues(typeof(ConsoleKey));

            foreach (var key in allConsoleKeys)
            {
                _currentKeyBoardState[(int)key] = false;
                _previousKeyBoardState[(int)key] = false;
            }
        }

        public void HandleInputs()
        {
            foreach (var key in _currentKeyBoardState.Keys)
            {
                _previousKeyBoardState[key] = _currentKeyBoardState[key];

                // Not sure I'm using this right. It works though.
                _currentKeyBoardState[key] = GetKeyState(key) > 1;
            }
        }

        /// <summary>
        /// returns true if the requested key is being pessed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDown(ConsoleKey key)
        {
            return _currentKeyBoardState[(int)key] && !_previousKeyBoardState[(int)key];
        }

        /// <summary>
        /// returns true if the requested key was down and is being released.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyUp(ConsoleKey key)
        {
            return _previousKeyBoardState[(int)key] && !_currentKeyBoardState[(int)key];
        }

        /// <summary>
        /// returns a list of all keys currently being pressed.
        /// </summary>
        /// <returns></returns>
        public List<ConsoleKey> GetPressedKeys()
        {
            return _currentKeyBoardState.Where(x => x.Value).Select(x => (ConsoleKey)x.Key).ToList();
        }
    }
}
