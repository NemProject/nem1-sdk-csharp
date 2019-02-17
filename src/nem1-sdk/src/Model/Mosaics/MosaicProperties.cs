using System;
using System.Collections.Generic;

namespace io.nem1.sdk.Model.Mosaics
{
    /// <summary>
    /// Class MosaicProperties.
    /// </summary>
    public class MosaicProperties
    {
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        internal List<Tuple<string, string>> Properties { get; }

        /// <summary>
        /// Gets the divisibility.
        /// </summary>
        /// <value>The divisibility.</value>
        public int Divisibility { get; }

        /// <summary>
        /// Gets the initial supply.
        /// </summary>
        /// <value>The initial supply.</value>
        public ulong InitialSupply { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MosaicAmount"/> is supply mutable.
        /// </summary>
        /// <value><c>true</c> if mutable; otherwise, <c>false</c>.</value>
        public bool Mutable { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MosaicAmount"/> is transferable.
        /// </summary>
        /// <value><c>true</c> if transferable; otherwise, <c>false</c>.</value>
        public bool Transferable { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicProperties"/> class.
        /// </summary>
        /// <param name="divisibility">The divisibility.</param>
        /// <param name="initialSupply">The initial supply.</param>
        /// <param name="mutable">if set to <c>true</c> [mutable].</param>
        /// <param name="transferable">if set to <c>true</c> [transferable].</param>
        public MosaicProperties(int divisibility, ulong initialSupply, bool mutable, bool transferable)
        {
            Properties = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("divisibility", divisibility.ToString()),
                new Tuple<string, string>("initialSupply", initialSupply.ToString()),
                new Tuple<string, string>("supplyMutable", mutable.ToString()),
                new Tuple<string, string>("transferable", transferable.ToString()),
            };

            InitialSupply = initialSupply;
            Divisibility = divisibility;
            Mutable = mutable;
            Transferable = transferable;
        }

    }
}
