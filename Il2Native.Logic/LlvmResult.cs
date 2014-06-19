namespace Il2Native.Logic
{
    using System;

    using PEAssemblyReader;

    public class LlvmResult
    {
        public LlvmResult(LlvmResult result) : this(result.Number, result.Type)
        {
        }

        public LlvmResult(int number, IType type)
        {
            if (number <= 0)
            {
                throw new ArgumentException("number");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            this.Number = number;
            this.Type = type;
        }

        protected LlvmResult()
        {
        }

        /// <summary>
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// </summary>
        public IType Type { get; private set; }
    }
}
