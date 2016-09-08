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
    * <summary>Defines observers that may be interrested in receiving DYM2 reading events.</summary>
    * @author Fabrice Bouyé (fabriceb@spc.int)
    */
    public interface IDymReadObserver
    {
        /**
         * <summary>Called when a data has been read.</summary>
         * <param name="field">The field that has been read.</param>
         * <param name="value">The value that has been read.</param>
         * <param name="coordinates">The data coordinates within the matrix in (layer, row, column) ordering for 3D matrices or (row, column) for 2D matrices.
         * <br/>If data is a single field and not a matrix, the size of the coordinate array is 0.</param>
         */
        void DataRead(Dym2Field field, Object value, int[] coordinates);
    }
}
