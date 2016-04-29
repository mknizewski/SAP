namespace SAP.BOL.LogicClasses
{
    public static class BlankChars
    {
        public static string Remove(string word)
        {
            string result = word.Trim(new char[] { '\r', '\n' });

            return result;
        }
    }
}
