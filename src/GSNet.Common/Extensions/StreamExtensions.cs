using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Extensions
{
    /// <summary>
    /// 流 <see cref="Stream"/> 的相关扩展方法
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 从当前流中读取字节，返回字节数组
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>字节数组</returns>
        public static byte[] GetAllBytes(this Stream stream)
        {
            using var memoryStream = new MemoryStream();

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
            //拷贝到MemoryStream
            stream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        /// <summary>
        /// 异步地从当前流中读取字节，返回字节数组
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>字节数组</returns>
        public static async Task<byte[]> GetAllBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream();

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            await stream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// 异步地从当前流中读取字节，并将它们写入另一个流。
        /// </summary>
        /// <param name="stream">当前流</param>
        /// <param name="destination">目标流</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns></returns>
        public static Task CopyToAsync(this Stream stream, Stream destination, CancellationToken cancellationToken)
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            return stream.CopyToAsync(
                destination,
                81920, 
                cancellationToken
            );
        }
    }
}
