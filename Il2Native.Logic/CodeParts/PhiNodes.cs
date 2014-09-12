namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Collections.Generic;

    public class PhiNodes
    {
        public PhiNodes()
        {
            this.Values = new List<OpCodePart>();
            this.Labels = new List<int>();
        }

        public List<OpCodePart> Values { get; private set; }

        public List<int> Labels { get; private set; }
    }
}
