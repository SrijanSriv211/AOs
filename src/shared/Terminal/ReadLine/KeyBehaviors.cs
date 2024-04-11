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

                Console.SetCursorPosition(CursorStartPos, Console.CursorTop);
                Console.Write(Text + " ");
            }
        }

        private void HandleCtrlBackspace()
        {
            if (CursorPos > CursorStartPos)
            {
                if (Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1) == CursorPos - CursorStartPos - 1)
                    HandleBackspace();

                int PreviousWordIdx = Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1);
                int length = CursorPos - CursorStartPos - PreviousWordIdx - 1;

                CursorPos -= length;
                Text = Text.Remove(CursorPos - CursorStartPos, length);

                Console.SetCursorPosition(CursorStartPos, Console.CursorTop);
                Console.Write(Text + new string (' ', length));
            }
        }

        private void HandleDelete()
        {
            if (CursorPos - CursorStartPos < Text.Length)
            {
                Text = Text.Remove(CursorPos - CursorStartPos, 1);

                Console.SetCursorPosition(CursorStartPos, Console.CursorTop);
                Console.Write(Text + " ");
            }
        }

        private void HandleCtrlDelete()
        {
            if (CursorPos - CursorStartPos < Text.Length)
            {
                if (Text.IndexOf(' ', CursorPos - CursorStartPos) == CursorPos - CursorStartPos)
                    HandleDelete();

                int NextWordIdx = Text.IndexOf(' ', CursorPos - CursorStartPos);
                int length = NextWordIdx == -1 ? Text.Length - (CursorPos - CursorStartPos) : NextWordIdx - (CursorPos - CursorStartPos);

                Text = Text.Remove(CursorPos - CursorStartPos, length);

                Console.SetCursorPosition(CursorStartPos, Console.CursorTop);
                Console.Write(Text + new string (' ', length));
            }
        }

        private void HandleHome()
        {
            CursorPos = CursorStartPos;
        }

        private void HandleEnd()
        {
            CursorPos = Text.Length;
        }

        private void HandleLeftArrow()
        {
            if (CursorPos > CursorStartPos)
                CursorPos--;
        }

        private void HandleCtrlLeftArrow()
        {
            if (CursorPos > CursorStartPos)
            {
                if (Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1) == CursorPos - CursorStartPos - 1)
                    CursorPos--;

                int PreviousWordIdx = Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1);
                int length = CursorPos - CursorStartPos - PreviousWordIdx - 1;

                CursorPos -= length;
            }
        }

        private void HandleRightArrow()
        {
            if (CursorPos - CursorStartPos < Text.Length)
                CursorPos++;
        }

        private void HandleCtrlRightArrow()
        {
            if (CursorPos - CursorStartPos < Text.Length)
            {
                if (Text.IndexOf(' ', CursorPos - CursorStartPos) == CursorPos - CursorStartPos)
                    CursorPos++;

                int NextWordIdx = Text.IndexOf(' ', CursorPos - CursorStartPos);
                int length = CursorPos - CursorStartPos + NextWordIdx;

                CursorPos += length;
            }
        }
    }
}
