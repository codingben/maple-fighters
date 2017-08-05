using System;
using System.Collections.Generic;

namespace Shared.Common.Communications
{
    /// <summary>
    /// Every data (property) on parameters structure should use this attribute in order to be able to transfer this data over the network.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ParameterAttribute : Attribute
    {
        public byte Code { get; set; }
    }

    /// <summary>
    /// A serializer of the Photon's dictionary parameters.
    /// </summary>
    public static class ParametersSerializer
    {
        public static T DeserializeParameters<T>(this Dictionary<byte, object> parameters)
            where T : struct, IParameters
        {
            return (T)Convert.ChangeType(Dserialize<T>(parameters), typeof(T));
        }

        public static Dictionary<byte, object> SerializeParameters<T>(this T parameters)
            where T : struct, IParameters
        {
            return Serialize(parameters);
        }

        /// <summary>
        /// Serialize a parameters structure into a Dictionary to be able to transfer it through a network.
        /// </summary>
        /// <typeparam name="T">Should include the only Struct which inherits from IParameters interface.</typeparam>
        /// <param name="parameters">The parameters structure that we want to turn into Dictionary.</param>
        private static Dictionary<byte, object> Serialize<T>(T parameters)
            where T : struct, IParameters
        {
            var temp = new Dictionary<byte, object>();
            var type = parameters.GetType();

            foreach (var propertyInfo in type.GetProperties())
            {
                if (!propertyInfo.CanRead)
                {
                    continue;
                }

                var properties = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), false) as ParameterAttribute[];
                if (properties == null)
                {
                    continue;
                }

                var codeParameter = properties[0].Code;
                temp.Add(codeParameter, propertyInfo.GetValue(parameters));
            }

            return temp;
        }

        /// <summary>
        /// Deserialize a Dictionary into a parameters structure to be able to read data which we have got over the network.
        /// </summary>
        /// <typeparam name="T">Should include the only Struct which inherits from IParameters interface.</typeparam>
        /// <param name="parameters">The parameters Dictionary that we want to turn into a structure.</param>
        private static object Dserialize<T>(IReadOnlyDictionary<byte, object> parameters)
            where T : struct, IParameters
        {
            object Object = new T();

            foreach (var propertyInfo in Object.GetType().GetProperties())
            {
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }

                var properties = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), false) as ParameterAttribute[];
                if (properties == null)
                {
                    continue;
                }

                var codeParameter = properties[0].Code;
                var newTypeValue = Convert.ChangeType(parameters[codeParameter], propertyInfo.PropertyType);

                propertyInfo.SetValue(Object, newTypeValue);
            }

            return Object;
        }
    }
}