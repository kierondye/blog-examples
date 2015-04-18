using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DateTimeConsumingService.Contracts
{
    /// <summary>
    /// Class for serving xs:date in Wcf.
    /// http://www.codeproject.com/Articles/182960/WCF-Support-for-xs-date
    /// </summary>
    [XmlSchemaProvider("MySchema")]
    public class WcfDate : IXmlSerializable
    {
        private DateTimeOffset theDateTimeOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfDate"/> class.
        /// </summary>
        public WcfDate()
            : this(new DateTime(1900, 1, 1))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfDate"/> class.
        /// </summary>
        /// <param name="dt">The dateTime.</param>
        public WcfDate(DateTime dt)
        {
            this.theDateTimeOffset = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Detetmines the schema.
        /// </summary>
        /// <param name="xs">The schema set.</param>
        /// <returns>The qualified name.</returns>
        public static XmlQualifiedName MySchema(XmlSchemaSet xs)
        {
            return new XmlQualifiedName("dateTime", "http://www.w3.org/2001/XMLSchema");
        }

        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <returns>Xml schema.</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadXml(XmlReader reader)
        {
            var content = reader.ReadElementContentAsString();
            this.theDateTimeOffset = DateTimeOffset.Parse(content);
            DateTime.SpecifyKind(this.theDateTimeOffset.DateTime, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteValue(this.theDateTimeOffset.ToString("yyyy-MM-ddTHH:mm:sszzz"));
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <returns>Date as DateTime.</returns>
        public DateTime GetDate()
        {
            return DateTime.SpecifyKind(this.theDateTimeOffset.DateTime, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Converts value to the string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public new string ToString()
        {
            return this.theDateTimeOffset.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }
    }
}