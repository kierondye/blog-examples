using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

using DateTimeConsumingService.Contracts;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimezoneTests
{
    [TestClass]
    public class WcfDateTests
    {
        [TestMethod]
        public void DeserializeAndSerializeDateTimeAnHourAhead()
        {
            // get current timezone offset + 1
            // we want to deserialize a date from a country an hour ahead
            // NOTE: DateTime will work out the offset of the date specified and not the current date
            var dateTime = new DateTime(2000, 1, 1);
            var timezoneOffsetHoursePlusOne = dateTime.Subtract(dateTime.ToUniversalTime()).Hours + 1;
            var objectToDeserialize =
                string.Format(
                    "<WcfDateContract xmlns=\"http://schemas.datacontract.org/2004/07/TimezoneTests\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><Value>2000-01-01T00:00:00+{0:00}:00</Value></WcfDateContract>",
                    timezoneOffsetHoursePlusOne);
            // de-serialize the object
            var serializer = new DataContractSerializer(typeof(WcfDateContract));
            var deserializedObject =
                serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(objectToDeserialize))) as WcfDateContract;
            // re-serialize the object
            var memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, deserializedObject);
            var serializedObject = Encoding.UTF8.GetString(memoryStream.ToArray());
            // check that they are the same
            Assert.AreEqual(objectToDeserialize, serializedObject);
        }

        [TestMethod]
        public void DeserializeAndSerializeDateTimeWithCurrentOffset()
        {
            // get current timezone offset
            // we want to deserialize a date from a country an hour ahead
            // NOTE: DateTime will work out the offset of the date specified and not the current date
            var dateTime = new DateTime(2000, 1, 1);
            var timezoneOffsetHoursePlusOne = dateTime.Subtract(dateTime.ToUniversalTime()).Hours;
            var objectToDeserialize =
                string.Format(
                    "<WcfDateContract xmlns=\"http://schemas.datacontract.org/2004/07/TimezoneTests\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><Value>2000-01-01T00:00:00{0:+00;-00;+00}:00</Value></WcfDateContract>",
                    timezoneOffsetHoursePlusOne);
            // de-serialize the object
            var serializer = new DataContractSerializer(typeof(WcfDateContract));
            var deserializedObject =
                serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(objectToDeserialize))) as WcfDateContract;
            // re-serialize the object
            var memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, deserializedObject);
            var serializedObject = Encoding.UTF8.GetString(memoryStream.ToArray());
            // check that they are the same
            Assert.AreEqual(objectToDeserialize, serializedObject);
        }
    }

    [DataContract]
    public class WcfDateContract
    {
        [DataMember]
        public WcfDate Value;
    }
}
