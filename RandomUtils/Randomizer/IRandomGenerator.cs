using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Randomizer
{
    /// <summary>
    /// Interface for the RandomGenerators that generate random objects of the specified type.
    /// </summary>
    public interface IRandomGenerator
    {
        /// <summary>
        /// Generates a random object of the specified type.
        /// </summary>
        /// <returns>The randomly generated object.</returns>
        dynamic Generate();
    }
}
