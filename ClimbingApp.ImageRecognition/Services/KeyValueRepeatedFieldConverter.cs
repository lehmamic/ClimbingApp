using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Google.Protobuf.Collections;
using static Google.Cloud.Vision.V1.Product.Types;

namespace ClimbingApp.ImageRecognition.Services
{
    public class KeyValueRepeatedFieldConverter : ITypeConverter<RepeatedField<KeyValue>, IReadOnlyDictionary<string, string>>
    {
        public IReadOnlyDictionary<string, string> Convert(RepeatedField<KeyValue> source, IReadOnlyDictionary<string, string> destination, ResolutionContext context)
        {
            IReadOnlyDictionary<string, string> result = null;
            if (source != null)
            {
                var dictionary = source.ToDictionary(k => k.Key, v => v.Value);
                result = new ReadOnlyDictionary<string, string>(dictionary);
            }

            return result;
        }
    }

}
