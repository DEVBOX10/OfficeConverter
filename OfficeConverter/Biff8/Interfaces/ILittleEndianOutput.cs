﻿/*
   Copyright 2014-2015 Kees van Spelde

   Licensed under The Code Project Open License (CPOL) 1.02;
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.codeproject.com/info/cpol10.aspx

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

namespace OfficeConverter.Biff8.Interfaces
{
    internal interface ILittleEndianOutput
    {
        void WriteByte(int v);
        void WriteShort(int v);
        void WriteInt(int v);
        void WriteLong(long v);
        void WriteDouble(double v);
        void Write(byte[] b);
        void Write(byte[] b, int offset, int len);
    }
}
