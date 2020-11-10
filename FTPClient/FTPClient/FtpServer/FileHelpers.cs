using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FtpServer
{
    class FileHelpers
    {
        public static FileStream m_theFile;
        public static bool m_fLoaded;

        public static void OpenFile(string sPath, bool fWrite)
		{
			try
			{
                m_theFile = new FileStream(sPath, 
					                (fWrite) ? FileMode.OpenOrCreate : FileMode.Open, 
					                (fWrite) ? FileAccess.Write : FileAccess.Read);
				if (fWrite)
				{
                    m_theFile.Seek(0, System.IO.SeekOrigin.End);
				}
				m_fLoaded = true;
			}
			catch (System.IO.IOException )
			{
                m_theFile = null;
			}
		}

        #region IFile Members

        public static int Read(byte[] abData, int nDataSize)
        {
            if (m_theFile == null)   return 0;
            try
            {
                return m_theFile.Read(abData, 0, nDataSize);
            }
            catch (System.IO.IOException) {  return 0; }
        }

        public static int Write(byte[] abData, int nDataSize)
        {
            if (m_theFile == null)   return 0;
            try
            {
                m_theFile.Write(abData, 0, nDataSize);
            }
            catch (System.IO.IOException) { return 0; }
            return nDataSize;
        }

        public static void Close()
        {
            if (m_theFile != null)
            {
                try
                {
                    m_theFile.Close();
                }
                catch (System.IO.IOException)  { }
                m_theFile = null;
            }
        }
        #endregion
    }
}
