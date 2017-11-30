using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FlatBuffers;

namespace TMorph.Common
{
    public class Utils
    {
        /// <summary>
        /// Функция получает "правильный" массив foo из builder.DataBuffer.Data.
        /// Воссоздание ByteBuffer из ороигинального builder.DataBuffer.Data Root объект создаётся неправильно,
        /// а из foo - правильно.
        /// </summary>
        /// <param name="buf"></param> оригинальный ByteBuffer
        /// <returns>byte[] скопированный из buf.Data</returns>
        public static byte[] FormatBuff(ByteBuffer buf)
        {
            byte[] foo = new byte[buf.Length - buf.Position];
            System.Buffer.BlockCopy(buf.Data, buf.Position, foo, 0, buf.Length - buf.Position);
            return foo;
        }
    }
}
