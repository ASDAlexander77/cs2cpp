// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
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
            var concurrent = true;

            var cs2CGenerator = new Cs2CGenerator(sources, args);
            var assemblySymbol = cs2CGenerator.Load();     
            if (assemblySymbol == null)
            {
                return;
            }

            var cgenerator = new CCodeUnitsBuilder(assemblySymbol, cs2CGenerator.BoundBodyByMethodSymbol, cs2CGenerator.SourceMethodByMethodSymbol);
            cgenerator.Concurrent = concurrent;
            var units = cgenerator.Build();

            var codeSerializer = new CCodeFilesGenerator();
            codeSerializer.Concurrent = concurrent;
            codeSerializer.WriteTo(assemblySymbol.Identity, cs2CGenerator.Assemblies, cs2CGenerator.IsCoreLib, cs2CGenerator.IsLibrary, units, outputFolder, cs2CGenerator.Impl);
        }
    }
}