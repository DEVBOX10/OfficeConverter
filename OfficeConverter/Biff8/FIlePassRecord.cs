﻿using System;
using System.IO;
using OfficeConverter.Biff8.Interfaces;
using OfficeConverter.Exceptions;

namespace OfficeConverter.Biff8
{

    /// <summary>
    /// Used to read the FilePass record
    /// </summary>
    internal class FilePassRecord
    {
        #region Consts
        private const short SidId = 0x2F;
        private const int EncryptionXor = 0;
        private const int EncryptionOther = 1;
        private const int EncryptionOtherRC4 = 1;
        private const int EncryptionOtherCapi2 = 2;
        private const int EncryptionOtherCapi3 = 3;
        #endregion

        #region Properties
        public byte[] DocId { get; private set; }

        public byte[] SaltData { get; private set; }

        public byte[] SaltHash { get; private set; }

        public short Sid
        {
            get { return SidId; }
        }
        #endregion

        #region Constructor
        public FilePassRecord(Stream inputStream)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var input = new LittleEndianInputStream(inputStream);
            if (input == null)
                throw new ArgumentNullException("inputStream");

            var encryptionType = input.ReadUShort();
            switch (encryptionType)
            {
                case EncryptionXor:
                    throw new OCExcelConfiguration("XOR obfuscation is not supported");

                case EncryptionOther:
                    break;
                
                default:
                    throw new OCExcelConfiguration("Unknown encryption type " + encryptionType);
            }

            var encryptionInfo = input.ReadUShort();
            switch (encryptionInfo)
            {
                case EncryptionOtherRC4:
                    // handled below
                    break;

                case EncryptionOtherCapi2:
                case EncryptionOtherCapi3:
                    throw new OCExcelConfiguration("CryptoAPI encryption is not supported");
                
                default:
                    throw new OCExcelConfiguration("Unknown encryption info " + encryptionInfo);
            }

            input.ReadUShort();
            DocId = Read(input, 16);
            SaltData = Read(input, 16);
            SaltHash = Read(input, 16);
        }
        #endregion

        #region Read
        /// <summary>
        /// Returns <paramref name="size"/> bytes from the <paramref name="input"/> stream
        /// </summary>
        /// <param name="input"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static byte[] Read(ILittleEndianInput input, int size)
        {
            var result = new byte[size];
            input.ReadFully(result);
            return result;
        }
        #endregion
    }
}