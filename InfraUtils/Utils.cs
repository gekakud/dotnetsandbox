using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace InfraUtils
{
    public static class Utils
    {
        public static T Min<T>(T first, T second) where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(first, second) < 0)
                return first;
            return second;
        }

        public static T Max<T>(T first, T second) where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(first, second) > 0)
                return first;
            return second;
        }

        public static string ListAsString(this IEnumerable list, string delim = ", ")
        {
            return ListAsStringLimited(list, 0, delim);
        }

        public static string ListAsStringLimited(IEnumerable list, int itemsLimit, string delim = ", ")
        {
            if (list == null)
            {
                return string.Empty;
            }

            int count = 0;
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (var elem in list)
            {
                count++;
                if (itemsLimit > 0 && count > itemsLimit)
                {
                    continue;
                }
                if (!first)
                {
                    sb.Append(delim);
                }
                else
                {
                    first = false;
                }
                sb.Append(elem?.ToString() ?? "null");
            }

            if (itemsLimit != 0 && count > itemsLimit)
            {
                return $"{count} items";
            }

            return sb.ToString();
        }

        public static bool BinaryEquals<T>(T obj1, T obj2) where T : class
        {
            return SerializeToByteArray(obj1).EqualLists(SerializeToByteArray(obj2));
        }

        public static int BinaryGetHashCode<T>(T obj) where T : class
        {
            if (obj == null)
            {
                return 0;
            }

            //for strings: string.GetHashCode() is 100 times faster than SerializeToByteArray(obj)
            return (obj as string)?.GetHashCode() ?? EnumerableGetHashCode(SerializeToByteArray(obj));
        }

        public static int BinaryGetHashCode(string str)
        {
            return str?.GetHashCode() ?? 0;
        }

        public static byte[] SerializeToByteArray<T>(T obj) where T : class
        {
            if (obj == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T DeserializeFromByteArray<T>(byte[] arr) where T : class
        {
            if (arr == null)
                return default(T);

            using (var stream = new MemoryStream(arr))
            {
                var formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }

        public static string SerializeToBase64String<T>(T obj, Func<T, byte[]> serializer) where T : class
        {
            var arr = serializer(obj);
            if (arr == null)
                return null;

            if (arr.Length == 0)
                return string.Empty;

            return Convert.ToBase64String(arr);
        }

        public static string SerializeToBase64String<T>(T obj) where T : class
        {
            return SerializeToBase64String(obj, SerializeToByteArray);
        }

        public static T DeserializeFromBase64String<T>(string s) where T : class
        {
            if (string.IsNullOrEmpty(s))
                return default(T);

            byte[] arr = Convert.FromBase64String(s);
            return DeserializeFromByteArray<T>(arr);
        }

        public static void WaitUntil(Func<bool> condition, string description, int timeoutInMinutes, int intervalInSeconds)
        {
            var timeout = TimeSpan.FromMinutes(timeoutInMinutes);
            var interval = TimeSpan.FromSeconds(intervalInSeconds);

            WaitUntil(condition, description, timeout, interval);
        }

        public static void WaitUntil(Func<bool> condition, string description, TimeSpan timeout, TimeSpan interval)
        {
            DateTime start = DateTime.Now;
            while (DateTime.Now - start < timeout)
            {
                if (condition())
                {
                    return;
                }

                Thread.CurrentThread.Join(interval);
            }
            throw new TimeoutException($"{description}: did not happen after {timeout:hh\\:mm\\:ss}");
        }

        public static async Task WaitUntilAsync(Func<Task<bool>> condition, string description, TimeSpan timeout, TimeSpan interval)
        {
            DateTime start = DateTime.Now;
            while (DateTime.Now - start < timeout)
            {
                if (await condition())
                {
                    return;
                }

                await Task.Delay(interval);
            }
            throw new TimeoutException($"{description}: did not happen after {timeout:hh\\:mm\\:ss}");
        }

        public static void WaitUntil<TClass>(Func<ValueWithCondition<TClass>> condition,
            Func<ValueWithCondition<TClass>, bool> doBeforeThrow,
            string description,
            TimeSpan timeout,
            TimeSpan interval) where TClass : class
        {
            DateTime start = DateTime.Now;
            ValueWithCondition<TClass> retValue = null;
            while (DateTime.Now - start < timeout)
            {
                retValue = condition();
                if (retValue.Condition)
                {
                    return;
                }

                Thread.CurrentThread.Join(interval);
            }
            doBeforeThrow?.Invoke(retValue);
            throw new TimeoutException($"{description}: did not happen after {timeout:hh\\:mm\\:ss}");
        }

        public static bool WaitAsLong(Func<bool> condition, string description, TimeSpan timeout, TimeSpan interval)
        {
            DateTime start = DateTime.Now;
            while (DateTime.Now - start < timeout)
            {
                if (!condition())
                {
                    return false;
                }
                Thread.CurrentThread.Join(interval);
            }
            return true;
        }

        public static int SafeGetHashCode<T>(this T obj)
        {
            return obj == null
                ? 0
                : obj.GetHashCode();
        }

        public static int EnumerableGetHashCode<T>(this IEnumerable<T> enumerable)
        {
            int result = 0;
            if (enumerable != null)
            {
                foreach (T elem in enumerable)
                {
                    result ^= SafeGetHashCode(elem);
                }
            }
            return result;
        }

        public static int DictionaryGetHashCode<T, T1>(this IDictionary<T, T1> dictionary)
        {
            int result = 0;
            if (dictionary != null)
            {
                foreach (KeyValuePair<T, T1> elem in dictionary)
                {
                    string kvpHash = string.Format("{0}{1}", SafeGetHashCode(elem.Key), SafeGetHashCode(elem.Value));
                    result ^= kvpHash.GetHashCode();
                }
            }
            return result;
        }        

        public static int ReadOnlyDictionaryGetHashCode<T, T1>(this IReadOnlyDictionary<T, T1> dictionary)
        {
            int result = 0;
            if (dictionary != null)
            {
                foreach (KeyValuePair<T, T1> elem in dictionary)
                {
                    string kvpHash = $"{SafeGetHashCode(elem.Key)}{SafeGetHashCode(elem.Value)}";
                    result ^= kvpHash.GetHashCode();
                }
            }
            return result;
        }

        public static string GetProcessAssemblyDirectoryName()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.DirectoryName.TrimEnd('\\');
        }

        public static string GetApplicationLocalDirectoryPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        public static string GetProcessAssemblyDirectoryPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static void SwallowExceptionIfShould(Action action, Predicate<Exception> shouldSwallow)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (!shouldSwallow(e))
                {
                    throw;
                }
            }
        }

        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static List<T> ShuffleList<T>(IEnumerable<T> list)
        {
            Random rand = new Random();
            List<T> ret = new List<T>();
            foreach (T v in list)
            {
                ret.Insert(rand.Next(0, ret.Count + 1), v);
            }
            return ret;
        }

        public static IEnumerable<T> IntersectAll<T>(IEnumerable<IEnumerable<T>> listOfLists)
        {
            if (!listOfLists.Any())
                return new List<T>();
            HashSet<T> hashSet = new HashSet<T>(listOfLists.First());
            foreach (var list in listOfLists.Skip(1))
            {
                hashSet.IntersectWith(list);
            }
            return hashSet.ToList();
        }

        public static Dictionary<T1, T2> UnifyDistinctKeyDictionaries<T1, T2>(Dictionary<T1, T2> dictionary1, Dictionary<T1, T2> dictionary2)
        {
            IEnumerable<KeyValuePair<T1, T2>> joinedEnum = dictionary1.Concat(dictionary2);
            return joinedEnum.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static IEnumerable<T> IntersectEnumerables<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            HashSet<T> hashSet = new HashSet<T>(list1);
            return list2.Where(hashSet.Contains);
        }

        public static HashSet<T> IntersectEnumerablesAsHashSet<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            HashSet<T> hashSet = new HashSet<T>(list1.Intersect(list2));
            return hashSet;
        }

        public static IDictionary<T1, T3> ConvertDictionary<T1, T2, T3>(IDictionary<T1, T2> sourceDictionary, Func<T2, T3> conversionFunc)
        {
            return sourceDictionary.ToDictionary(kvp => kvp.Key, kvp => conversionFunc(kvp.Value));
        }

        public static string ObjectToXml<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, obj);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static void WriteXmlFile<T>(string fileName, List<T> objList)
        {
            XmlSerializer writer = new XmlSerializer(typeof(List<T>));
            using (StreamWriter file = new StreamWriter(fileName))
            {
                writer.Serialize(file, objList);
                file.Close();
            }
        }

        public static T XmlToObject<T>(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(xmlString))
            {
                return (T)(serializer.Deserialize(XmlReader.Create(stringReader)));
            }
        }

        public static T XmlValidateToObject<T>(string xmlString, string xsdString)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            using (StringReader stringReader = new StringReader(xsdString))
            {
                schemas.Add(XmlSchema.Read(XmlReader.Create(stringReader), null));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(xmlString))
            {
                XmlReaderSettings settings = new XmlReaderSettings()
                {
                    Schemas = schemas,
                    ValidationType = ValidationType.Schema
                };
                return (T)(serializer.Deserialize(XmlReader.Create(stringReader, settings)));
            }
        }

        public static string BytesToUnitsString(ulong numberOfBytes)
        {
            var ci = CultureInfo.CreateSpecificCulture("en-EN");
            if (numberOfBytes < 2048)
            {
                return string.Format(ci, "{0}B", numberOfBytes);
            }

            ulong numberOfKilobytes = numberOfBytes / 1024;
            if (numberOfKilobytes < 2048)
            {
                return string.Format(ci, "{0}KB", numberOfKilobytes);
            }

            ulong numberOfMegaBytes = numberOfKilobytes / 1024;
            if (numberOfMegaBytes < 512)
            {
                return string.Format(ci, "{0}MB", numberOfMegaBytes);
            }

            double numberOfGigaBytes = (double)numberOfMegaBytes / 1024;
            if (numberOfGigaBytes < 10)
            {
                return string.Format(ci, "{0:0.00}GB", numberOfGigaBytes);
            }
            if (numberOfGigaBytes < 100)
            {
                return string.Format(ci, "{0:0.0}GB", numberOfGigaBytes);
            }
            if (numberOfGigaBytes < 1024)
            {
                return string.Format(ci, "{0}GB", (int)numberOfGigaBytes);
            }

            double numberOfTeraBytes = numberOfGigaBytes / 1024;
            if (Math.Abs(numberOfTeraBytes - (int)numberOfTeraBytes) < 0.001)
            {
                return string.Format(ci, "{0:0}TB", numberOfTeraBytes);
            }
            return string.Format(ci, "{0:0.000}TB", numberOfTeraBytes);
        }

        public static uint GBtoMB(uint GB)
        {
            return GB * 1024;
        }

        public static uint MBtoGB(uint MB)
        {
            return MB / 1024;
        }

        public static int GBtoMB(int GB)
        {
            return GB * 1024;
        }

        public static int MBtoGB(int MB)
        {
            return MB / 1024;
        }

        public static long MBtoGB(long MB)
        {
            return MB / 1024;
        }

        public static long MBtoBytes(long MB)
        {
            return MBtoKB(MB) * 1024;
        }

        public static long MBtoKB(long MB)
        {
            return MB * 1024;
        }


        public static uint IpAddressToUInt(string address)
        {
            if (address == null || address.Equals("") || address.Contains(":"))
            {
                return 0;
            }
            return BitConverter.ToUInt32(IPAddress.Parse(address).GetAddressBytes().Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Merge the new list into originalList
        /// the method doesn't support list inside list
        /// </summary>
        public static void MergeLists<T>(IList<T> originList, IList<T> newList)
        {
            IList<T> copyListFromDb = new List<T>(originList);
            foreach (T elemFromDb in copyListFromDb)
            {
                if (!newList.Contains(elemFromDb))
                {
                    originList.Remove(elemFromDb);
                }
            }

            foreach (T elemFromUser in newList)
            {
                if (!originList.Contains(elemFromUser))
                {
                    originList.Add(elemFromUser);
                }
            }
        }

        public static Dictionary<TKey, int> CountBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var countsByKey = new Dictionary<TKey, int>();
            foreach (var x in source)
            {
                var key = keySelector(x);
                if (!countsByKey.ContainsKey(key))
                    countsByKey[key] = 0;
                countsByKey[key] += 1;
            }
            return countsByKey;
        }

        public static Stream StringToStream(string s)
        {
            byte[] returnBytes = Encoding.UTF8.GetBytes(s);
            return new MemoryStream(returnBytes);
        }

        public static Guid ConvertIntToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        /// <summary>
        /// consistently convert string to guid using SHA1.
        /// RFC 4122  handles string->guid covnersions,
        /// note : RFC 4122 item 4.3 defines a method to convert *names* into guids.but this is NOT what we do here
        /// </summary>
        public static Guid ConverStringToGuidSHA1(string src)
        {
            byte[] stringbytes = Encoding.UTF8.GetBytes(src);
            byte[] hashedBytes = new SHA1CryptoServiceProvider()
                .ComputeHash(stringbytes);
            Array.Resize(ref hashedBytes, 16);
            return new Guid(hashedBytes);
        }

        public static void GetCallerClassAndMethod(StackTrace stackTrace, out string callerClass, out string callerMethod)
        {
            callerClass = "";
            callerMethod = "";
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            StackFrame callingFrame = stackFrames.ElementAtOrDefault(1);
            if (callingFrame == null)
            {
                return;
            }
            MethodBase method = callingFrame.GetMethod();
            callerMethod = method.Name;
            callerClass = method.DeclaringType.Name;
        }

        public static string ObjPropertiesToString(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();

            string outStr = obj.GetType() + ": ";
            foreach (var info in propertyInfos)
            {
                var value = info.GetValue(obj, null) ?? "(null)";
                outStr += info.Name + "=" + value + " ";
            }

            return outStr;
        }

        public static bool SafeDispose(this IDisposable disposable)
        {
            if (disposable == null)
                return false;

            disposable.Dispose();
            return true;
        }

        private static bool IsCharNullOrWhiteSpace(char c)
        {
            return c == 0 || char.IsWhiteSpace(c);
        }

        public static bool IsNullOrWhiteSpaceIncludingZeroChar(string str)
        {
            if (str != null && str.Any(c => !IsCharNullOrWhiteSpace(c)))
            {
                return false;
            }
            return true;
        }

        public static bool IsParsableToEnum<T>(string value) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            T parsingResult;
            bool isParsableToEnum = typeof(T).IsEnum && Enum.TryParse(value, true, out parsingResult);

            return isParsableToEnum;
        }

        public static T ParseEnumWithErrorMessage<T>(string value) where T : struct
        {
            if (!string.IsNullOrEmpty(value))
            {
                T output;
                if (typeof(T).IsEnum && Enum.TryParse(value, true, out output))
                {
                    if (Enum.GetNames(typeof(T)).Contains(output.ToString()))
                    {
                        return output;
                    }
                }
            }
            throw new FormatException($"Failed parsing '{value}' as a enum parameter. ");
        }

        public static T? ParseNullableEnumWithErrorMessage<T>(string value) where T : struct
        {
            if (!string.IsNullOrEmpty(value))
            {
                T output;
                if (typeof(T).IsEnum && Enum.TryParse(value, true, out output))
                {
                    if (Enum.GetNames(typeof(T)).Contains(output.ToString()))
                    {
                        return output;
                    }
                }
                throw new FormatException($"Failed parsing '{value}' as a enum parameter. ");
            }
            return null;
        }

        public static T? ParseNullableEnumWithErrorMessage<T>(string value, StringBuilder errMsg) where T : struct
        {
            if (!string.IsNullOrEmpty(value))
            {
                T output;
                if (typeof(T).IsEnum && Enum.TryParse(value, true, out output))
                {
                    if (Enum.GetNames(typeof(T)).Contains(output.ToString()))
                    {
                        return output;
                    }
                }

                if (errMsg == null)
                {
                    throw new FormatException($"Failed parsing '{value}' as a enum parameter. ");
                }
                errMsg.Append("Failed parsing '").Append(value).Append("' as a enum parameter. ");
            }
            return null;
        }

        public static Guid? ParseNullableGuidWithErrorMessage(string value, string parameterName, StringBuilder errMsg)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Guid output;
                if (value == "''")
                {
                    return Guid.Empty;
                }
                if (Guid.TryParse(value, out output))
                {
                    return output;
                }

                var message = $"Failed parsing '{value}' as a {parameterName} parameter. ";
                if (errMsg == null)
                {
                    throw new FormatException(message);
                }
                errMsg.Append(message);
            }
            return null;
        }

        public static bool IsParsableToGuid(string value)
        {
            Guid parsingResult;
            bool isParsableToGuid = Guid.TryParse(value, out parsingResult);

            return isParsableToGuid;
        }

        public static Guid ParseGuidWithErrorMessage(string value, string parameterName)
        {
            Guid output;
            if (Guid.TryParse(value, out output))
            {
                return output;
            }

            throw new FormatException($"Failed parsing '{value}' as a valid {parameterName}.");
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
        }

        public static Stream GetMemoryStreamFromString(string s)
        {
            var stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}