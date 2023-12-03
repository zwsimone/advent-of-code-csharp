namespace AdventOfCode
{
    public class Menu
    {
        private int _selectedIndex;
        private readonly string[] _options;
        private readonly string _prompt;

        public Menu(string prompt, string[] options)
        {
            _prompt = prompt;
            _options = options;
            _selectedIndex = 0;
        }

        private void DisplayOptions()
        {
            Console.WriteLine(_prompt);
            for (int i = 0; i < _options.Length; i++)
            {
                string currentOption = _options[i];
                string prefix;
                if (i == _selectedIndex)
                {
                    prefix = "*";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{prefix} << {currentOption} >>");
            }

            Console.ResetColor();
        }

        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                // Update SelectedIndex based on arrow keys.
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    _selectedIndex--;
                    if (_selectedIndex == -1)
                    {
                        _selectedIndex = _options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    _selectedIndex++;
                    if (_selectedIndex == _options.Length)
                    {
                        _selectedIndex = 0;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);

            return _selectedIndex;
        }
    }
}

