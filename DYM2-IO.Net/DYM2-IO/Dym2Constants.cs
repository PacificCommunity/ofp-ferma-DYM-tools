/***********************************************************************
*  Copyright - Secretariat of the Pacific Community                   *
*  Droit de copie - Secrétariat Général de la Communauté du Pacifique *
*  http://www.spc.int/                                                *
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.Spc.Ofp.IO.Dym
{
    /**
     * <summary>Contains constants for DYM2 handling.</summary>
     * @author Fabrice Bouyé (fabriceb@spc.int)
     */
    public class Dym2Constants
    {
        /**
         * <summary>Magic ID that identifies a DYM2 file.
         * <br/>Always "DYM2".</summary>
         */
        public const String MAGIC = "DYM2";

        /**
         * <summary>Size of a work in DYM2.
         * <br/>Always 4 bytes (32 bits).</summary>
         */
        public const int WORD_SIZE = 4;

        /**
         * <summary>Default stretching (used when reading DYM1 file as DYM2 files).
         * <br/>Always {@code Dym2Stretching.NONE}.</summary>
         */
        public const Dym2Stretching DEFAULT_STRETCHING = Dym2Stretching.NONE;

        /** <summary>Number of days in a DYM year.</summary>
         * @since 1.1
         */
        public const float DAYS_IN_YEAR = 365.25f;

        /** <summary>Number of days in a DYM month.</summary>
         * @since 1.1
         */
        public const float DAYS_IN_MONTH = DAYS_IN_YEAR / 12f;
    }
}
