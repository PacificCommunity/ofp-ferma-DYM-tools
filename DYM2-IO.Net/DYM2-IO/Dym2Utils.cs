/***********************************************************************
*  Copyright - Secretariat of the Pacific Community                   *
*  Droit de copie - Secrétariat Général de la Communauté du Pacifique *
*  http://www.spc.int/                                                *
***********************************************************************/
using System;

namespace Org.Spc.Ofp.IO.Dym
{
    /**
     * <summary>Contains utility methods for DYM2 handling.</summary>
     * @author Fabrice Bouyé (fabriceb@spc.int)
     */
    public class Dym2Utils
    {
        /**
         * <summary>Computes the file size of a DYM2 file.</summary>
         * <param name="nlong">Number of longitudinal records.</param>
         * <param name="nlat">Number of latitudinal records.</param>
         * <param name="nlevel">Number of time/depth records.</param>
         * <returns>The size of the file.</returns>
         */
        public static int GetFileSize(int nlong, int nlat, int nlevel)
        {
            int result = Dym2Constants.WORD_SIZE; // MAGIC.
            result += Dym2Constants.WORD_SIZE; // IDFUNC.
            result += Dym2Constants.WORD_SIZE; // MIN_VALUE.
            result += Dym2Constants.WORD_SIZE; // MAX_VALUE.
            result += Dym2Constants.WORD_SIZE; // NLONG.
            result += Dym2Constants.WORD_SIZE; // NLAT.
            result += Dym2Constants.WORD_SIZE; // NEVEL.
            result += Dym2Constants.WORD_SIZE; // START_DATE.
            result += Dym2Constants.WORD_SIZE; // END_DATE.
            result += Dym2Constants.WORD_SIZE * nlat * nlong; // XLON.
            result += Dym2Constants.WORD_SIZE * nlong * nlat; // YLAT.
            result += Dym2Constants.WORD_SIZE * nlevel; // ZLEVEL.
            result += Dym2Constants.WORD_SIZE * nlat * nlong; // MSKSP.
            result += Dym2Constants.WORD_SIZE * nlevel * nlat * nlong; // DATA.
            return result;
        }

        /**
         * <summary>Gets the offset of a particular field in the file.
         * <br/>When querying offset for non matrix fields, parameters can all be 0.</summary>
         * <param name="nlong">Number of longitudinal records.</param>
         * <param name="nlat">Number of latitudinal records.</param>
         * <param name="nlevel">Number of time/depth records.</param>
         * <returns>The offset within the file.</returns>
         */
        public static int GetOffset(Dym2Field field, int nlong, int nlat, int nlevel)
        {
            int result = 0;
            switch (field)
            {
                case Dym2Field.DATA:
                    result += Dym2Constants.WORD_SIZE * nlat * nlong;
                    goto case Dym2Field.MSKSP;
                case Dym2Field.MSKSP:
                    result += Dym2Constants.WORD_SIZE * nlevel;
                    goto case Dym2Field.ZLEVEL;
                case Dym2Field.ZLEVEL:
                    result += Dym2Constants.WORD_SIZE * nlong * nlat;
                    goto case Dym2Field.YLAT;
                case Dym2Field.YLAT:
                    result += Dym2Constants.WORD_SIZE * nlat * nlong;
                    goto case Dym2Field.XLON;
                case Dym2Field.XLON:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.END_DATE;
                case Dym2Field.END_DATE:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.START_DATE;
                case Dym2Field.START_DATE:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.NLEVEL;
                case Dym2Field.NLEVEL:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.NLAT;
                case Dym2Field.NLAT:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.NLONG;
                case Dym2Field.NLONG:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.MAX_VALUE;
                case Dym2Field.MAX_VALUE:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.MIN_VALUE;
                case Dym2Field.MIN_VALUE:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.IDFUNC;
                case Dym2Field.IDFUNC:
                    result += Dym2Constants.WORD_SIZE;
                    goto case Dym2Field.MAGIC;
                case Dym2Field.MAGIC:
                    result += 0;
                    break;
            }
            return result;
        }

        ///////////////////////////////////////////////////////////////////////
        /**
         * <summary>Converts a year / month / day date to a DYM date.</summary>
         * <param name="year"> the year.</param>
         * <param name="month"> The month; January is month 0.</param>
         * <param name="day"> The day in the month.</param>
         * <returns>A {@code float}.</returns>
         */
        public static float ToDymDate(int year, int month, int day)
        {
            if (year < 1000)
            {
                year += 1900;
            }
            return year + ((Dym2Constants.DAYS_IN_MONTH * month) + day) / Dym2Constants.DAYS_IN_YEAR;
        }

        /**
         * <summary>Transforms a dym date into a year / month / day date.</summary>
         * <param name="time">The DYM time.</param>
         * <returns>An array of 3 {@code int} values :
         * <UL>
         * <LI>Year is at index 0.</LI>
         * <LI>Month is at index 1. <BR>
         * January is month 0.</LI>
         * <LI>Day is at index 2.</LI>
         * </UL>
         * </returns>
         */
        public static int[] ToIntDate(float time)
        {
            return ToIntDate(time, null);
        }

        /**
         * <summary>Transforms a dym date into a year / month / day date.</summary>
         * <param name="time">The DYM time.</param>
         * <param name="result">An array to store the result.</param>
         * <BR/>If the given array is {@code null} or has a size &lt; 3, a new array will be allocated and returned.</param>
         * <returns>An array of 3 {@code int} values:
         * <UL>
         * <LI>Year is at index 0.</LI>
         * <LI>Month is at index 1. <BR>
         * January is month 0.</LI>
         * <LI>Day is at index 2.</LI>
         * </UL>
         * </returns>
         */
        public static int[] ToIntDate(float time, int[] result)
        {
            if ((result == null) || (result.Length < 3))
            {
                result = new int[3];
            }
            if (time < 1000)
            {
                time += 1900;
            }
            int year = (int)time;
            int month = (int)(1 + (((time - year) * Dym2Constants.DAYS_IN_YEAR) / Dym2Constants.DAYS_IN_MONTH));
            int julianDay = (int)((time - year) * Dym2Constants.DAYS_IN_YEAR);
            int day = (int)(julianDay - (Dym2Constants.DAYS_IN_MONTH * (month - 1)));
            result[0] = year;
            result[1] = month - 1;
            result[2] = day;
            return result;
        }
    }
}
