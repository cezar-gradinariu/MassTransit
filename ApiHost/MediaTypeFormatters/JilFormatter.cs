using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Jil;

namespace ApiHost.MediaTypeFormatters
{
    public class JilFormatter : MediaTypeFormatter
    {
        private readonly Options _jilOptions;

        public JilFormatter()
        {
            _jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601, excludeNulls: true, prettyPrint: true);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            SupportedEncodings.Add(new UTF8Encoding(false, true));
            SupportedEncodings.Add(new UnicodeEncoding(false, true, true));
        }

        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.FromResult(DeserializeFromStream(type, readStream));
        }

        private object DeserializeFromStream(Type type, Stream readStream)
        {
            try
            {
                using (var reader = new StreamReader(readStream))
                {
                    var method = typeof (JSON).GetMethod("Deserialize", new[] {typeof (TextReader), typeof (Options)});
                    var generic = method.MakeGenericMethod(type);
                    return generic.Invoke(this, new object[] {reader, _jilOptions});
                }
            }
            catch
            {
                return null;
            }
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);
            JSON.Serialize(value, streamWriter, _jilOptions);
            streamWriter.Flush();
            return Task.FromResult(writeStream);
        }
    }
}