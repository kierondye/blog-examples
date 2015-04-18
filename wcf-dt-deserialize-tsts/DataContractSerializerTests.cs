using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimezoneTests
{
    [TestClass]
    public class DataContractSerializerTests
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
                    "<DateTimeContract xmlns=\"http://schemas.datacontract.org/2004/07/TimezoneTests\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><Value>2000-01-01T00:00:00+{0:00}:00</Value></DateTimeContract>",
                    timezoneOffsetHoursePlusOne);
            // de-serialize the object
            var serializer = new DataContractSerializer(typeof(DateTimeContract));
            var deserializedObject =
                serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(objectToDeserialize))) as DateTimeContract;
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
                    "<DateTimeContract xmlns=\"http://schemas.datacontract.org/2004/07/TimezoneTests\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"><Value>2000-01-01T00:00:00{0:+00;-00;+00}:00</Value></DateTimeContract>",
                    timezoneOffsetHoursePlusOne);
            // de-serialize the object
            var serializer = new DataContractSerializer(typeof(DateTimeContract));
            var deserializedObject =
                serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(objectToDeserialize))) as DateTimeContract;
            // re-serialize the object
            var memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, deserializedObject);
            var serializedObject = Encoding.UTF8.GetString(memoryStream.ToArray());
            // check that they are the same
            Assert.AreEqual(objectToDeserialize, serializedObject);
        }
    }

    [DataContract]
    public class DateTimeContract
    {
        [DataMember]
        public DateTime Value;
    }
}
