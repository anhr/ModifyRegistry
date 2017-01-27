/* *************************************** 
 *			 ModifyRegistry.cs
 * ---------------------------------------
 *         a very simple class 
 *    to read, write, delete and count
 *       registry values with C#
 * ---------------------------------------
 *      if you improve this code 
 *   please email me your improvement!
 * ---------------------------------------
 *         by Andrej Hristoliubov
 *          anhr@mail.ru
 *         Thanks to Francesco Natali
 *        - fn.varie@libero.it -
 *   https://www.codeproject.com/articles/3389/read-write-and-delete-from-registry-with-c
 * ***************************************/

using System;
// it's required for reading/writing into the registry:
using Microsoft.Win32;
// and for the MessageBox function: Help: http://stackoverflow.com/questions/6639468/c-forms-does-not-exist-in-the-namespace-system-windows
using System.Windows.Forms; 

namespace Utility.ModifyRegistry
{
	/// <summary>
	/// An useful class to read/write/delete/count registry keys
	/// </summary>
	public class ModifyRegistry
	{
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="BaseRegistryKey">base key</param>
        /// <param name="companyName">your company name</param>
        /// <param name="productName">your application product name</param>
        public ModifyRegistry(RegistryKey BaseRegistryKey = null, string companyName = null, string productName = null)
        {
            if (BaseRegistryKey == null)
                baseRegistryKey = Registry.LocalMachine;
            else baseRegistryKey = BaseRegistryKey;
            if (companyName != null)
                companyName = "\\" + companyName;
            if (productName == null)
                productName = Application.ProductName;
            subKey = "SOFTWARE" + companyName + "\\" + productName;
        }
        private string subKey;
		private RegistryKey baseRegistryKey;//https://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k(Microsoft.Win32.Registry);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5.2);k(DevLang-csharp)&rd=true

        /* **************************************************************************
		 * **************************************************************************/

        /// <summary>
        /// To read a registry key.
        /// input: KeyName (string), Value (object) default value, valueKind (RegistryValueKind)
        /// output: value (object) 
        /// </summary>
        public object Read(string KeyName, object Value = null, RegistryValueKind valueKind = RegistryValueKind.Unknown)
		{
			// Opening the registry key
			RegistryKey rk = baseRegistryKey ;
			// Open a subKey as read-only
			RegistryKey sk1 = rk.OpenSubKey(subKey);
			// If the RegistrySubKey doesn't exist -> (null)
			if ( sk1 == null )
			{
                if (Value != null)
                {
                    Write(KeyName, Value, valueKind);
                    sk1 = rk.OpenSubKey(subKey);
                    if (sk1 == null)
                        throw new Exception(String.Format("Open {0} subkey failed!", subKey));
                }
                else return Value;
			}
            object value = sk1.GetValue(KeyName);
            if (value == null)
            {
                if (Value != null)
                {
                    Write(KeyName, Value, valueKind);
                    value = sk1.GetValue(KeyName);
                    if (value == null)
                        throw new Exception(String.Format("Open {0} key failed!", KeyName));
                }
                else
                {
                    sk1.Close();
                    return Value;
                }
            }
            if ((valueKind != RegistryValueKind.Unknown) && (valueKind != sk1.GetValueKind(KeyName)))
                throw new Exception( String.Format("Invalid registry data type {0}.", valueKind));
            sk1.Close();
            return value;
		}

        /// <summary>
        /// To read a registry key of the string kind.
        /// input: KeyName (string), Value (string) default value
        /// output: value (string) 
        /// </summary>
        public string ReadString(string KeyName, string Value = null)
        {
            return (string)Read(KeyName, Value, RegistryValueKind.String);
        }

        /// <summary>
        /// To read a registry key of the 32-bit binary number kind.
        /// input: KeyName (string), Value (uint) default value
        /// output: value (uint) 
        /// </summary>
        public uint ReadDWord(string KeyName, uint Value = 0)
        {
            return (uint)(int)Read(KeyName, Value, RegistryValueKind.DWord);
        }

        /* **************************************************************************
		 * **************************************************************************/

        /// <summary>
        /// To write into a registry key.
        /// input: KeyName (string) , Value (object), valueKind (RegistryValueKind)
        /// </summary>
        public void Write(string KeyName, object Value, RegistryValueKind valueKind = RegistryValueKind.Unknown)
		{
			// Setting
			RegistryKey rk = baseRegistryKey ;
			// I have to use CreateSubKey 
			// (create or open it if already exits), 
			// 'cause OpenSubKey open a subKey as read-only
			RegistryKey sk1 = rk.CreateSubKey(subKey);
			// Save the value
			sk1.SetValue(KeyName, Value, valueKind);
            sk1.Close();
		}

        /// <summary>
        /// To write into a registry key.
        /// input: KeyName (string) , Value (string)
        /// </summary>
        public void WriteString(string KeyName, string Value)
        {
            Write(KeyName, Value, RegistryValueKind.String);
        }

        /// <summary>
        /// To write into a registry key.
        /// input: KeyName (string) , Value (uint)
        /// </summary>
        public void WriteDWord(string KeyName, uint Value)
        {
            Write(KeyName, Value, RegistryValueKind.DWord);
        }

        /* **************************************************************************
		 * **************************************************************************/

        /// <summary>
        /// To delete a registry key.
        /// input: KeyName (string)
        /// output: true or false 
        /// </summary>
        public bool DeleteKey(string KeyName)
		{
			// Setting
			RegistryKey rk = baseRegistryKey ;
			RegistryKey sk1 = rk.CreateSubKey(subKey);
			// If the RegistrySubKey doesn't exists -> (true)
			if ( sk1 == null )
				return true;
			else
				sk1.DeleteValue(KeyName);

			return true;
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// To delete a sub key and any child.
		/// input: void
		/// output: true or false 
		/// </summary>
		public bool DeleteSubKeyTree()
		{
    		// Setting
			RegistryKey rk = baseRegistryKey ;
			RegistryKey sk1 = rk.OpenSubKey(subKey);
			// If the RegistryKey exists, I delete it
			if ( sk1 != null )
				rk.DeleteSubKeyTree(subKey);

			return true;
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// Retrive the count of subkeys at the current key.
		/// input: void
		/// output: number of subkeys
		/// </summary>
		public int SubKeyCount()
		{
			// Setting
			RegistryKey rk = baseRegistryKey ;
			RegistryKey sk1 = rk.OpenSubKey(subKey);
			// If the RegistryKey exists...
			if ( sk1 != null )
				return sk1.SubKeyCount;
			else
				return 0; 
		}

		/* **************************************************************************
		 * **************************************************************************/

		/// <summary>
		/// Retrive the count of values in the key.
		/// input: void
		/// output: number of keys
		/// </summary>
		public int ValueCount()
		{
			// Setting
			RegistryKey rk = baseRegistryKey ;
			RegistryKey sk1 = rk.OpenSubKey(subKey);
			// If the RegistryKey exists...
			if ( sk1 != null )
				return sk1.ValueCount;
			else
				return 0; 
		}
	}
}
