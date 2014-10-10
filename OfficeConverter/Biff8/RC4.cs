﻿using System.Collections.Generic;

namespace OfficeConverter.Biff8
{
    /// <summary>
    /// Excel RC4 encryption or decryption
    /// </summary>
    internal class RC4
    {
        #region Fields
        private readonly byte[] _bytes = new byte[256];
        private int _i;
        private int _j;
        #endregion

        #region Constructor
        public RC4(IList<byte> key)
        {
            var keyLength = key.Count;

            for (var i = 0; i < 256; i++)
                _bytes[i] = (byte) i;

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i%keyLength] + _bytes[i]) & 255;
                var temp = _bytes[i];
                _bytes[i] = _bytes[j];
                _bytes[j] = temp;
            }

            _i = 0;
            _j = 0;
        }
        #endregion

        #region Output
        public byte Output()
        {
            _i = (_i + 1) & 255;
            _j = (_j + _bytes[_i]) & 255;

            var temp = _bytes[_i];
            _bytes[_i] = _bytes[_j];
            _bytes[_j] = temp;

            return _bytes[(_bytes[_i] + _bytes[_j]) & 255];
        }
        #endregion

        #region Encrypt
        public void Encrypt(byte[] inputBytes)
        {
            for (var i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] = (byte) (inputBytes[i] ^ Output());
            }
        }
        #endregion

        #region Encrypt
        public void Encrypt(byte[] inputBytes, int offSet, int length)
        {
            var end = offSet + length;
            for (var i = offSet; i < end; i++)
                inputBytes[i] = (byte) (inputBytes[i] ^ Output());
        }
        #endregion
    }
}