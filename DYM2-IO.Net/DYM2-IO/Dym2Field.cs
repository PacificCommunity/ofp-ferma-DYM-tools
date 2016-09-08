/***********************************************************************
*  Copyright - Secretariat of the Pacific Community                   *
*  Droit de copie - Secrétariat Général de la Communauté du Pacifique *
*  http://www.spc.int/                                                *
***********************************************************************/
using System;

namespace Org.Spc.Ofp.IO.Dym
{
    /**
     * <summary>Indentifies fields within a DYM2 file.</summary>
     * @author Fabrice Bouyé (fabriceb@spc.int)
     */ 
    public enum Dym2Field
    {
        MAGIC, IDFUNC, MIN_VALUE, MAX_VALUE, NLONG, NLAT, NLEVEL, START_DATE, END_DATE, XLON, YLAT, ZLEVEL, MSKSP, DATA
    };
}
