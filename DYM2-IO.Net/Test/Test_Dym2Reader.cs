/***********************************************************************
*  Copyright - Secretariat of the Pacific Community                   *
*  Droit de copie - Secrétariat Général de la Communauté du Pacifique *
*  http://www.spc.int/                                                *
***********************************************************************/
using System;

namespace Org.Spc.Ofp.IO.Dym
{
    /**
    * <summary>Test case for reading DYM2 in C#.</summary>
    * @author Fabrice Bouyé (fabriceb@spc.int)
    */
    public class Test_Dym2Reader
    {
        /**
        * <summary>Listener implementation for printing on screen.
        * <br/>Use similar approach to copy into another fileformat (text, netcdf).</summary>
        * @author Fabrice Bouyé (fabriceb@spc.int)
        */
        class ConsoleWriter : IDymReadObserver
        {
            private int nlong;
            private int nlat;
            private int nlevel;

            public void DataRead(Dym2Field field, Object value, int[] coordinates)
            {
                switch (field)
                {
                    case Dym2Field.IDFUNC:
                        Dym2Stretching idFunc = (Dym2Stretching)((int)value);
                        Console.Out.Write("Strecthing: ");
                        switch(idFunc) {
                            case Dym2Stretching.NONE:
                                Console.Out.WriteLine("None");
                                break;
                            case Dym2Stretching.PARALLEL:
                                Console.Out.WriteLine("Parallel");
                                break;
                            case Dym2Stretching.GRAVITY:
                                Console.Out.WriteLine("Gravity Sink");
                                break;
                            default:
                                Console.Out.WriteLine("Unknown");
                                break;
                        }
                        break;
                    case Dym2Field.NLONG:
                        nlong = (int)value;
                        Console.Out.WriteLine("Longitude records " + nlong);
                        break;
                    case Dym2Field.NLAT:
                        nlat = (int)value;
                        Console.Out.WriteLine("Latitude records " + nlat);
                        break;
                    case Dym2Field.NLEVEL:
                        nlevel = (int)value;
                        Console.Out.WriteLine("Time/depth records " + nlevel);
                        break;
                    case Dym2Field.START_DATE:
                        Console.Out.WriteLine("Start date " + (float)value);
                        break;
                    case Dym2Field.END_DATE:
                        Console.Out.WriteLine("End date " + (float)value);
                        break;
                    case Dym2Field.MAX_VALUE:
                        Console.Out.WriteLine("Start date " + (float)value);
                        break;
                    case Dym2Field.MIN_VALUE:
                        Console.Out.WriteLine("End date " + (float)value);
                        break;
                    case Dym2Field.XLON:
                        //Console.Out.Write((float)value);
                        //Console.Out.Write(" ");
                        //if (coordinates[1] == nlong - 1)
                        //{
                        //    Console.Out.WriteLine();
                        //}
                        break;
                    case Dym2Field.YLAT:
                        //Console.Out.Write((float)value);
                        //Console.Out.Write(" ");
                        //if (coordinates[1] == nlat - 1)
                        //{
                        //    Console.Out.WriteLine();
                        //}
                        break;
                    case Dym2Field.ZLEVEL:
                        //Console.Out.Write((float)value);
                        //Console.Out.Write(" ");
                        //if (coordinates[0] == nlevel - 1)
                        //{
                        //    Console.Out.WriteLine();
                        //}
                        break;
                    case Dym2Field.MSKSP:
                        //Console.Out.Write((int) value);
                        //Console.Out.Write(" ");
                        //if (coordinates[1] == nlong -1)
                        //{
                        //    Console.Out.WriteLine();
                        //}
                        break;
                    case Dym2Field.DATA:
                        //Console.Out.Write((float)value);
                        //Console.Out.Write(" ");
                        //if (coordinates[2] == nlong - 1)
                        //{
                        //    Console.Out.WriteLine();
                        //}
                        break;
                };
            }
        };

        /**
         * <summary>Program entry point.</summary>
         * <param name="args">Arguments from the command line.</param>
         */
        static void Main(string[] args)
        {
            Dym2Reader reader = new Dym2Reader("C:\\Fabriceb\\public\\temp200mmcl-1deg-mr.dym");
            reader.AddObserver(new ConsoleWriter());
            reader.Read();
        }
    }
}
