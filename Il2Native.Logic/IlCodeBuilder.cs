namespace Il2Native.Logic
{
    using System;

    public class IlCodeBuilder
    {
        public void Add(Code code)
        {

        }

        public void Add<T>(T valueOrToken)
        {

        }

        public void Add(Code code, int token)
        {

        }

        public Label CreateLabel()
        {
            throw new NotImplementedException();
        }

        public Label Branch(Code code, Code codeShort)
        {
            throw new NotImplementedException();
        }

        public void Branch(Code code, Code codeShort, Label label)
        {
        }

        public byte[] GetCode()
        {
            throw new NotImplementedException();
        }

        // helpers
        public void LoadConstant(int @const)
        {

        }

        public void LoadArgument(int number)
        {

        }

        public void SaveArgument(int number)
        {

        }

        public void LoadLocal(int number)
        {

        }

        public void SaveLocal(int number)
        {

        }

        public class Label
        {
            
        }
    }
}
