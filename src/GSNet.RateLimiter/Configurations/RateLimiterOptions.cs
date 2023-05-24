using System.Collections.Generic;
using GSNet.Common;

namespace GSNet.RateLimiter.Configurations
{
    /// <summary>
    /// 限流器选项
    /// </summary>
    public class RateLimiterOptions
    {
        /// <summary>
        /// </summary>
        public RateLimiterOptions( )
        {
            Extensions = new List<IRateLimiterOptionsExtension>();
        }

        /// <summary>
        /// 限流器选项的扩展的列表
        /// </summary>
        /// <value>The extensions.</value>
        internal IList<IRateLimiterOptionsExtension> Extensions { get; }

        /// <summary>
        /// Registers the extension.
        /// </summary>
        /// <param name="extension">Extension.</param>
        public void RegisterExtension(IRateLimiterOptionsExtension extension)
        {
            Check.Argument.IsNotNull(extension, nameof(extension));

            Extensions.Add(extension);
        }
    }

}
