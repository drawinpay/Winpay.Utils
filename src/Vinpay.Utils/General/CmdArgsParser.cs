namespace Vinpay.Utils.General
{
    /// <summary>
    /// Command line parameter parser.
    /// </summary>
    public class CmdArgsParser
    {
        #region Private Methods

        private static int FindFirstKeyIndex(string[] args, int beginIndex)
        {
            int firstIndex = -1;
            for (int i = beginIndex; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    firstIndex = i;
                    break;
                }
            }

            return firstIndex;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parse input string to a Dictionary with a List of string value.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("The input cannot be null or white space.");
            }

            var inputSegments = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            return Parse(inputSegments);
        }

        /// <summary>
        /// Group the input string array by key
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> Parse(string[] args)
        {
            var groupedArgs = new Dictionary<string, List<string>>();
            if (args == null || args.Length == 0)
            {
                throw new ArgumentException("The input args cannot be null or empty.");
            }

            int firstKeyIndex = FindFirstKeyIndex(args, 0);
            
            if (firstKeyIndex < 0) // No keys.
            {
                groupedArgs.Add("", new List<string>(args));
                return groupedArgs;
            }

            if (firstKeyIndex > 0) // The first [firstKeyIndex] parameters are not keys.
            {
                groupedArgs.Add("", new List<string>(args.Take(firstKeyIndex)));
            }

            var hasNextKey = true;
            int currentKeyIndex = firstKeyIndex;
            while (hasNextKey)
            {
                int nextKeyIndex = FindFirstKeyIndex(args, currentKeyIndex + 1);

                int argCount;
                if (nextKeyIndex > 0) // Exist a next key.
                {
                    argCount = nextKeyIndex - currentKeyIndex - 1;
                }
                else
                {
                    // If there is no next key, then all the remaining parameters belong to the current key.
                    argCount = args.Length - currentKeyIndex - 1;
                }

                string key = args[currentKeyIndex].Substring(1);
                if(string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("\"-\" cannot exist independently");
                }

                if (argCount > 0)
                {
                    groupedArgs.Add(key, args.Skip(currentKeyIndex + 1).Take(argCount).ToList());
                }
                else
                {
                    groupedArgs.Add(key, new List<string>());
                }

                if (nextKeyIndex > 0)
                {
                    currentKeyIndex = nextKeyIndex;
                }
                else
                {
                    hasNextKey = false;
                }
            }

            return groupedArgs;
        }

        #endregion
    }
}
