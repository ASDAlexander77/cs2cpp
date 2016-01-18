// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Il2Converter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    /// <summary>
    /// </summary>
    public class Il2Converter
    {
        /// <summary>
        /// </summary>
        public enum ConvertingMode
        {
            /// <summary>
            /// </summary>
            ForwardDeclaration,

            /// <summary>
            /// </summary>
            PreDeclaration,

            /// <summary>
            /// </summary>
            Declaration,

            /// <summary>
            /// </summary>
            PostDeclaration,

            /// <summary>
            /// </summary>
            PreDefinition,

            /// <summary>
            /// </summary>
            Definition,

            /// <summary>
            /// </summary>
            PostDefinition
        }

        public static bool VerboseOutput { get; set; }

        public static void Convert(string source, string outputFolder, string[] args = null)
        {
            new Il2Converter().ConvertInternal(new[] { source }, outputFolder, args);
        }

        public static void Convert(string[] sources, string outputFolder, string[] args = null)
        {
            new Il2Converter().ConvertInternal(sources, outputFolder, args);
        }

        /// <summary>
        /// </summary>
        /// <param name="sources">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        /// <param name="args">
        /// </param>
        protected void ConvertInternal(string[] sources, string outputFolder, string[] args = null)
        {
            var cs2CGenerator = new Cs2CGenerator(sources, args);
            var assemblySymbol = cs2CGenerator.Load();

            var cgenerator = new CCodeGenerator(assemblySymbol);
            var units = cgenerator.Build();

            var codeSerializer = new CCodeSerializer();
            codeSerializer.WriteTo(assemblySymbol.Identity, units, outputFolder);
        }
    }
}