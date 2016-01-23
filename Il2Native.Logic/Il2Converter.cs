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

            var cgenerator = new CCodeUnitsBuilder(assemblySymbol, cs2CGenerator.BoundBodyByMethodSymbol, cs2CGenerator.SourceMethodByMethodSymbol);
            var units = cgenerator.Build();

            var codeSerializer = new CCodeSerializer();
            codeSerializer.WriteTo(assemblySymbol.Identity, cs2CGenerator.Assemblies, cs2CGenerator.IsCoreLib, units, outputFolder);
        }
    }
}