partial class Terminal
{
    private partial class ReadLine
    {
        private void HandleEnter()
        {
            Console.WriteLine();
            Loop = false;
        }

        private void HandleBackspace()
        {
            if (CursorPos > CursorStartPos)
            {
                CursorPos--;
                Text = Text.Remove(CursorPos - CursorStartPos, 1);
                Console.SetCursorPosition(CursorPos, Console.CursorTop);
                Console.Write(Text[CursorPos..] + " ");
                Console.SetCursorPosition(CursorPos, Console.CursorTop);
            }
        }

        private void HandleCtrlBackspace()
        {
            if (CursorPos > CursorStartPos)
            {
                if (Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1) == CursorPos - CursorStartPos - 1)
                    HandleBackspace();

                int Previous_word_idx = Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1);
                int length = CursorPos - CursorStartPos - Previous_word_idx;

                for (int i = 0; i < length; i++)
                    HandleBackspace();
            }
        }

        private void HandleLeftArrow()
        {
            if (CursorPos > CursorStartPos)
                CursorPos--;
        }

        private void HandleRightArrow()
        {
            CursorPos++;
        }

        private void HandleCtrlLeftArrow()
        {
            if (CursorPos > CursorStartPos)
            {
                if (Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1) == CursorPos - CursorStartPos - 1)
                    CursorPos--;

                int Previous_word_idx = Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1);
                if (Previous_word_idx == -1)
                    Previous_word_idx = CursorStartPos;

                CursorPos = CursorStartPos + Previous_word_idx + 1;
            }
        }
    }
}
