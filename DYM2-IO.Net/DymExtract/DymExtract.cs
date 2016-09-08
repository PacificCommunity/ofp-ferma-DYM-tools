/***********************************************************************
*  Copyright - Secretariat of the Pacific Community                   *
*  Droit de copie - Secrétariat Général de la Communauté du Pacifique *
*  http://www.spc.int/                                                *
***********************************************************************/
using System;

using System.Reflection;

namespace Org.Spc.Ofp.Project.DymExtract
{
    /**
     * @author Fabrice Bouyé (fabriceb@spc.int)
     */
    class DymExtract
    {
        const String VERSION = "0.4";

        /**
         * <summary>Program entry point.</summary>
         * <param name="args">Arguments from the command line.</param>
         */
        static void Main(string[] args)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////
            // Callback is required to have the exec resolve the DLL inside itself.
            AppDomain.CurrentDomain.AssemblyResolve += (sender, margs) =>
            {
                //Console.WriteLine("Trying to resolve " + margs.Name);
                String resourceName = "DymExtract." + new AssemblyName(margs.Name).Name + ".dll";
                //Console.WriteLine(resourceName);
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Console.WriteLine(stream);
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            ////////////////////////////////////////////////////////////////////////////////////////////////
            if (args.Length == 0)
            {
                Usage();
            }
            String inputPath = args[0];
            Convert(inputPath);
        }

        /**
         * <summary>Convert a simple DYM2 into a text file.</summary>
         * <param name="inputPath">The input file.</param>
         */ 
        private static void Convert(String inputPath)
        {
            try
            {
                String outputPath = inputPath.Substring(0, inputPath.LastIndexOf(".") - 1) + ".txt";
                SimpleFileConverterTask.Convert(inputPath, outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Command failed with error: \"" + ex.Message + "\"");
                Environment.Exit(1);
            }
        }

        /**
         * <summary>Usage method.
         * <br/>Exits the program with error code 1.</summary>
         */
        private static void Usage()
        {
            Console.Out.WriteLine("DymExtract " + VERSION);
            Console.Out.WriteLine("© CPS-SPC 2013-2014");
            Console.Out.WriteLine("Usage:");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  DymExtract <filename>");
            Console.Out.WriteLine("");
            Console.Out.WriteLine("Where <filename> is the path to a DYM2 file.");
            Environment.Exit(1);
        }
    }
}
