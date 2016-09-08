/***********************************************************************
*  Copyright - Secretariat of the Pacific Community                   *
*  Droit de copie - Secrétariat Général de la Communauté du Pacifique *
*  http://www.spc.int/                                                *
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Spc.Ofp.IO.Dym;
using System.IO;

namespace Org.Spc.Ofp.Project.DymExtract
{
    /**
    * <summary>Listener implementation for converting to text file in order to import in database.</summary>
    * @author Fabrice Bouyé (fabriceb@spc.int)
    */
    public class SimpleFileConverterTask : IDymReadObserver, IDisposable
    {
        /**
         * <summary>Number of longitudinal records.</summary>
         */
        private int nlong;
        /**
         * <summary>Number of latidudinal records.</summary>
         */
        private int nlat;
        /**
         * <summary>Number of time/depth records.</summary>
         */
        private int nlevel;
        /**
         * <summary>Stores the longitude values.</summary>
         */
        private float[] xlon;
        /**
         * <summary>Stores the latitude values.</summary>
         */
        private float[] ylat;
        /**
         * <summary>Stores the time values.</summary>
         */
        private float[] zlevel;
        /**
         * <summary>Buffer used during date conversion.</summary>
         */
        private int[] dateBuffer;
        /**
         * <summary>Path to the output file.</summary>
         */
        private String path;

        /**
         * <summary>Creates a new instance.</summary>
         * <param name="path">Path to the output file.</param>
         */
        private SimpleFileConverterTask(String path)
        {
            this.path = path;
        }

        /**
         * <summary>The output writer.</summary>
         */
        private StreamWriter output;

        /**
         * <summary>Implement IDisposable.</summary>
         */
        public void Dispose()
        {
            if (output != null)
            {
                output.Close();
                output = null;
            }
        }

        private long totalExports = 0;
        private long exportedCells = 0;

        /**
         * <summary>Implement IDymReadObserver.</summary>
         */
        public void DataRead(Dym2Field field, Object value, int[] coordinates)
        {
            switch (field)
            {
                case Dym2Field.NLONG:
                    nlong = (int)value;
                    Console.Out.WriteLine(String.Format("Nlong: {0}", nlong));
                    break;
                case Dym2Field.NLAT:
                    nlat = (int)value;
                    Console.Out.WriteLine(String.Format("Nlat: {0}", nlat));
                    break;
                case Dym2Field.NLEVEL:
                    nlevel = (int)value;
                    Console.Out.WriteLine(String.Format("Nlevel: {0}", nlevel));
                    totalExports = nlong * nlat * nlevel;
                    break;
                case Dym2Field.MIN_VALUE:
                    Console.Out.WriteLine(String.Format("Min value: {0}", (float)value));
                    break;
                case Dym2Field.MAX_VALUE:
                    Console.Out.WriteLine(String.Format("Max value: {0}", (float)value));
                    break;
                case Dym2Field.START_DATE:
                    Console.Out.WriteLine(String.Format("Start date: {0}", (float)value));
                    break;
                case Dym2Field.END_DATE:
                    Console.Out.WriteLine(String.Format("End date: {0}", (float)value));
                    break;
                case Dym2Field.XLON:
                    // @todo This will not be correct if idfunc is not 0 or 1.
                    if (xlon == null)
                    {
                        xlon = new float[nlong];
                    }
                    xlon[coordinates[1]] = (float)value;
                    break;
                case Dym2Field.YLAT:
                    // @todo This will not be correct if idfunc is not 0 or 1.
                    if (ylat == null)
                    {
                        ylat = new float[nlat];
                    }
                    if (coordinates[1] == 0)
                    {
                        ylat[coordinates[0]] = (float)value;
                    }
                    break;
                case Dym2Field.ZLEVEL:
                    if (zlevel == null)
                    {
                        zlevel = new float[nlevel];
                    }
                    zlevel[coordinates[0]] = (float)value;
                    break;
                case Dym2Field.MSKSP:
                    break;
                case Dym2Field.DATA:
                    float val = (float)value;
                    float longitude = xlon[coordinates[2]];
                    float latitude = ylat[coordinates[1]];
                    float time = zlevel[coordinates[0]];
                    dateBuffer = Dym2Utils.ToIntDate(time, dateBuffer);
                    //String line = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", dateBuffer[0], dateBuffer[1] + 1, latitude, longitude, val, exportedCells);
                    String line = String.Format("{0}\t{1}\t{2}\t{3}\t{4}", dateBuffer[0], dateBuffer[1] + 1, latitude, longitude, val);
                    if (output == null)
                    {
                        output = new StreamWriter(File.Open(path, FileMode.Create));
                    }
                    output.WriteLine(line);
                    exportedCells++;
                    break;
            };
        }
        /////////////////////////////////////////////////////////////////////////////////
        /**
         * <summary>Convert a simple DYM2 into a text file.</summary>
         * <param name="inputPath">Path to the input file.</param>
         * <param name="outputPath">Path to the output file.</param>
         */
        public static void Convert(String inputPath, String outputPath)
        {
            using (SimpleFileConverterTask writer = new SimpleFileConverterTask(outputPath))
            {
                Console.Out.WriteLine(String.Format("Converting: {0}", inputPath));
                Dym2Reader reader = new Dym2Reader(inputPath);
                reader.AddObserver(writer);
                reader.Read();
                //
                Console.Out.WriteLine(String.Format("Exported: {0}/{1}", writer.exportedCells, writer.totalExports));
                Console.Out.WriteLine("-----------------------------------------------");
            }
        }
    }
}
