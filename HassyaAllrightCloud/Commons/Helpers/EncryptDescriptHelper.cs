using HassyaAllrightCloud.Commons.Extensions;
using Jwt;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Text.Json;

namespace HassyaAllrightCloud.Commons.Helpers
{
    public class EncryptHelper
    {
        #region variables
        private const int shuffleKey = 1745902836;

        #endregion

        #region public methods

        /// <summary>
        /// encrypt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string EncryptToUrl<T>(T item, int key = shuffleKey)
        {
            var encrypted = Encrypt(item, key);
            return WebUtility.UrlEncode(encrypted);
        }

        public static string Encrypt<T>(T item, int key = shuffleKey)
        {
            var input = JsonConvert.SerializeObject(item);
            var output = input.EncryptString(key);
            return output;
        }

        /// <summary>
        /// decrypt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T DecryptFromUrl<T>(string url, int key = shuffleKey)
        {
            try
            {
                var encrypted = WebUtility.UrlDecode(url);
                return Decrypt<T>(encrypted, key);
            }
            catch (SignatureVerificationException)
            {
                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static T Decrypt<T>(string str, int key = shuffleKey)
        {
            try
            {
                var dataStr = str.DecryptString(key);
                return JsonConvert.DeserializeObject<T>(dataStr);
            }
            catch (SignatureVerificationException)
            {
                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        #endregion
    }
}
