/***********************************************************************
*  Copyright - Secretariat of the Pacific Community                   *
*  Droit de copie - Secrétariat Général de la Communauté du Pacifique *
*  http://www.spc.int/                                                *
***********************************************************************/
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Org.Spc.Ofp.IO.Dym
{
    /**
     * <summary>Reads a DYM2 file.</summary>
     * @author Fabrice Bouyé (fabriceb@spc.int)
     */
    public class Dym2Reader
    {
        /**
         * <summary>Path to the file.</summary>
         */ 
        private String path;

        /**
         * <summary>Creates a new instance.</summary>
         * <param name="path">Path to the file.</param>
         */
        public Dym2Reader(String path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (path.Trim().Length == 0)
            {
                throw new ArgumentOutOfRangeException("path");
            }
            else if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            this.path = path;
        }

        /**
         * <summary>Read the file.</summary>
         */ 
        public void Read()
        {
            using (FileStream input = File.Open(path, FileMode.Open))
            {
                int bytesToRead = (int)input.Length;
                byte[] bytes = new byte[Dym2Constants.WORD_SIZE];
                // MAGIC.
                String magic = ReadString(input, bytes);
                NotifyObserversDataRead(Dym2Field.MAGIC, magic);
                //
                if (!Dym2Constants.MAGIC.Equals(magic))
                {
                    throw new IOException("File \"" + path + "\" is not a DYM2 file.");
                }
                // IDFUNC.
                int idfunc = ReadInt32(input, bytes);
                NotifyObserversDataRead(Dym2Field.IDFUNC, idfunc);
                // MIN_VALUE.
                float minValue = ReadSingle(input, bytes);
                NotifyObserversDataRead(Dym2Field.MIN_VALUE, minValue);
                // MAX_VALUE.
                float maxValue = ReadSingle(input, bytes);
                NotifyObserversDataRead(Dym2Field.MAX_VALUE, maxValue);
                // NLONG.
                int nlong = ReadInt32(input, bytes);
                NotifyObserversDataRead(Dym2Field.NLONG, nlong);
                // NLAT.
                int nlat = ReadInt32(input, bytes);
                NotifyObserversDataRead(Dym2Field.NLAT, nlat);
                // NLEVEL.
                int nlevel = ReadInt32(input, bytes);
                NotifyObserversDataRead(Dym2Field.NLEVEL, nlevel);
                //
                int fileSize = Dym2Utils.GetFileSize(nlong, nlat, nlevel);
                if (fileSize < bytesToRead)
                {
                    throw new IOException("File \"" + path + "\" is truncated, was expecting " + fileSize + " bytes, found " + bytesToRead + " bytes.");
                }
                // START_DATE.
                float startDate = ReadSingle(input, bytes);
                NotifyObserversDataRead(Dym2Field.START_DATE, startDate);
                // END_DATE.
                float endDate = ReadSingle(input, bytes);
                NotifyObserversDataRead(Dym2Field.END_DATE, endDate);
                // XLON.
                for (int lat = 0; lat < nlat; lat++)
                {
                    for (int lon = 0; lon < nlong; lon++)
                    {
                        float value = ReadSingle(input, bytes);
                        NotifyObserversDataRead(Dym2Field.XLON, value, new int[]{lat, lon});
                    }
                }
                // YLAT.
                for (int lat = 0; lat < nlat; lat++)
                {
                    for (int lon = 0; lon < nlong; lon++)
                    {
                        float value = ReadSingle(input, bytes);
                        NotifyObserversDataRead(Dym2Field.YLAT, value, new int[] { lat, lon });
                    }
                }
                // ZLEVEL.
                for (int level = 0; level < nlevel; level++)
                {
                    float value = ReadSingle(input, bytes);
                    NotifyObserversDataRead(Dym2Field.ZLEVEL, value, new int[] { level });
                }
                // MSKSP.
                for (int lat = 0; lat < nlat; lat++)
                {
                    for (int lon = 0; lon < nlong; lon++)
                    {
                        int value = ReadInt32(input, bytes);
                        NotifyObserversDataRead(Dym2Field.MSKSP, value, new int[] { lat, lon });
                    }
                }
                // DATA.
                for (int level = 0; level < nlevel; level++)
                {
                    for (int lat = 0; lat < nlat; lat++)
                    {
                        for (int lon = 0; lon < nlong; lon++)
                        {
                            float value = ReadSingle(input, bytes);
                            NotifyObserversDataRead(Dym2Field.DATA, value, new int[] { level, lat, lon });
                        }
                    }
                }
            }
        }

        /**
         * <summary>Reads a buffer fully.</summary>
         * <param param="input">The source stream.</param>
         * <param param="buffer"> The buffer in which to read.</param>
         */
        private void ReadFully(Stream input, byte[] buffer)
        {
            int sizeToRead = buffer.Length;
            int bytesRead = 0;
            while (bytesRead < sizeToRead)
            {
                int amount = input.Read(buffer, bytesRead, sizeToRead - bytesRead);
                if (amount == 0)
                {
                    break;
                }
                bytesRead += amount;
            }
        }

        /**
         * <summary>Reads a {@code String} from the stream.</summary>
         * <param param="input">The source stream.</param>
         * <param param="buffer"> The buffer in which to read.</param>
         * <returns>An {@code String}.</returns>
         */
        private String ReadString(Stream input, byte[] buffer)
        {
            ReadFully(input, buffer);
            String result = "";
            foreach (byte b in buffer)
            {
                result += (char)b;
            }
            return result;
        }

        /**
         * <summary>Reads an {@code int} from the stream.</summary>
         * <param param="input">The source stream.</param>
         * <param param="buffer"> The buffer in which to read.</param>
         * <returns>An {@code int}.</returns>
         */
        private int ReadInt32(Stream input, byte[] buffer)
        {
            ReadFully(input, buffer);
            //if (BitConverter.IsLittleEndian)
            //{
            //    Array.Reverse(buffer);
            //}
            return BitConverter.ToInt32(buffer, 0);
        }

        /**
         * <summary>Reads a {@code float} from the stream.</summary>
         * <param param="input">The source stream.</param>
         * <param param="buffer">The buffer in which to read.</param>
         * <returns>A {@code float}.</returns>
         */
        private float ReadSingle(Stream input, byte[] buffer)
        {
            ReadFully(input, buffer);
            //if (BitConverter.IsLittleEndian)
            //{
            //    Array.Reverse(buffer);
            //}
            return BitConverter.ToSingle(buffer, 0);
        }

        //////////////////////////////////////////////////////////////////////
        /**
         * <summary>List of listeners.</summary>
         */
        private List<IDymReadObserver> observers = new List<IDymReadObserver>();

        /**
         * <summary>Adds an observer to the list.</summary>
         * <param param="observer">The observer.</param>
         */
        public void AddObserver(IDymReadObserver observer)
        {
            if (observer != null)
            {
                observers.Add(observer);
            }
        }

        /**
         * <summary>Removes an observer from the list.</summary>
         * <param param="observer">The observer.</param>
         */
        public void RemoveObserver(IDymReadObserver observer)
        {
            if (observer != null)
            {
                observers.Remove(observer);
            }
        }

        /**
         * <summary>Notify observers of a single data read.</summary>
         * <param param="field">The field that has been read.</param>
         * <param param="value">The value that has been read.</param>
         */
        private void NotifyObserversDataRead(Dym2Field field, Object value)
        {
            NotifyObserversDataRead(field, value, new int[0]);
        }

        /**
         * <summary>Notify observers of a single data read.</summary>
         * <param param="field">The field that has been read.</param>
         * <param param="value">The value that has been read.</param>
         * <param param="coordinates">The data coordinates within the matrix in (layer, row, column) ordering for 3D matrices or (row, column) for 2D matrices.
         * <br/>If data is a single field and not a matrix, the size of the coordinate array is 0.</param>
         */
        private void NotifyObserversDataRead(Dym2Field field, Object value, int[] coordinates)
        {
            // Notify from last to first.
            foreach (IDymReadObserver observer in observers.AsEnumerable().Reverse())
            {
                observer.DataRead(field, value, coordinates);
            }
        }
    }
}
