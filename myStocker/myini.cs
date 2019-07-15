using System;
using System.Runtime.InteropServices;
using System.Text;

namespace myStocker
{
    public class myini
    {
        /// <summary>
        /// buffer size when reading value from INI file
        /// </summary>
        public static int capacity = 512;

        /// <summary>
        /// Gets the private profile string.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder value, int size, string filePath);

        /// <summary>
        /// Gets the private profile string.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, [In, Out] char[] value, int size, string filePath);

        /// <summary>
        /// Gets the private profile section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="keyValue">The key value.</param>
        /// <param name="size">The size.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileSection(string section, IntPtr keyValue, int size, string filePath);

        /// <summary>
        /// Writes the private profile string.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);

        /// <summary>
        /// Writes a value into a section/key
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static bool WriteValue(string section, string key, string value, string filePath)
        {
            bool result = WritePrivateProfileString(section, key, value, filePath);
            return result;
        }

        /// <summary>
        /// Deletes a section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        public static bool DeleteSection(string section, string filepath)
        {
            bool result = WritePrivateProfileString(section, null, null, filepath);
            return result;
        }

        /// <summary>
        /// Deletes a key in specific section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
        public static bool DeleteKey(string section, string key, string filepath)
        {
            bool result = WritePrivateProfileString(section, key, null, filepath);
            return result;
        }

        /// <summary>
        /// Reads a value in a specific section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string ReadValue(string section, string key, string filePath, string defaultValue = "")
        {
            var value = new StringBuilder(capacity);
            GetPrivateProfileString(section, key, defaultValue, value, value.Capacity, filePath);
            return value.ToString();
        }

        /// <summary>
        /// Reads all sections.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string[] ReadSections(string filePath)
        {
            // first line will not recognize if ini file is saved in UTF-8 with BOM
            while (true)
            {
                char[] chars = new char[capacity];
                int size = GetPrivateProfileString(null, null, "", chars, capacity, filePath);

                if (size == 0)
                {
                    return null;
                }

                if (size < capacity - 2)
                {
                    string result = new String(chars, 0, size);
                    string[] sections = result.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
                    return sections;
                }

                capacity = capacity * 2;
            }
        }

        /// <summary>
        /// Reads all keys.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string[] ReadKeys(string section, string filePath)
        {
            // first line will not recognize if ini file is saved in UTF-8 with BOM
            while (true)
            {
                char[] chars = new char[capacity];
                int size = GetPrivateProfileString(section, null, "", chars, capacity, filePath);

                if (size == 0)
                {
                    return null;
                }

                if (size < capacity - 2)
                {
                    string result = new String(chars, 0, size);
                    string[] keys = result.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
                    return keys;
                }

                capacity = capacity * 2;
            }
        }

        /// <summary>
        /// Reads the key/value pairs.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string[] ReadKeyValuePairs(string section, string filePath)
        {
            while (true)
            {
                IntPtr returnedString = Marshal.AllocCoTaskMem(capacity * sizeof(char));
                int size = GetPrivateProfileSection(section, returnedString, capacity, filePath);

                if (size == 0)
                {
                    Marshal.FreeCoTaskMem(returnedString);
                    return null;
                }

                if (size < capacity - 2)
                {
                    string result = Marshal.PtrToStringAuto(returnedString, size - 1);
                    Marshal.FreeCoTaskMem(returnedString);
                    string[] keyValuePairs = result.Split('\0');
                    return keyValuePairs;
                }

                Marshal.FreeCoTaskMem(returnedString);
                capacity = capacity * 2;
            }
        }
    }
}
