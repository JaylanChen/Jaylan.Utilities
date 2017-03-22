using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jaylan.Utilities.Json
{
    //NuGet     Install-Package Newtonsoft.Json
    /// <summary>
    /// Newtonsoft.Json������
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// ���������л�ΪJSON��ʽ
        /// </summary>
        /// <param name="o">����</param>
        /// <returns>json�ַ���</returns>
        public static string SerializeObject(object o)
        {
            var json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// ����JSON�ַ������ɶ���ʵ��
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="json">json�ַ���(eg.{"ID":"112","Name":"ʯ�Ӷ�"})</param>
        /// <returns>����ʵ��</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            //JsonSerializer serializer = new JsonSerializer();
            //StringReader sr = new StringReader(json);
            //object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            //T t = o as T;
            var t = JsonConvert.DeserializeObject<T>(json);
            return t;
        }

        /// <summary>
        /// ����JSON�������ɶ���ʵ�弯��
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="json">json�����ַ���(eg.[{"ID":"112","Name":"ʯ�Ӷ�"}])</param>
        /// <returns>����ʵ�弯��</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            //JsonSerializer serializer = new JsonSerializer();
            //StringReader sr = new StringReader(json);
            //object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            //List<T> list = o as List<T>;
            var list = JsonConvert.DeserializeObject<List<T>>(json);
            return list;
        }

        /// <summary>
        /// �����л�JSON����������������.
        /// </summary>
        /// <typeparam name="T">������������</typeparam>
        /// <param name="json">json�ַ���</param>
        /// <param name="anonymousTypeObject">��������</param>
        /// <returns>��������</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            var t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
    }
}