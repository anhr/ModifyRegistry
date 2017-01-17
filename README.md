# ModifyRegistry
A very simple class to read, write, delete and count of Windows registry values with C#

## Using the code

First of all, you have to import its namespace:

`using Utility.ModifyRegistry;`

and to instantiate your class:

`ModifyRegistry myRegistry = new ModifyRegistry(Registry.CurrentUser, "my company name", "my applacation product name");`

parameter 1: BaseRegistryKey Registry.LocalMachine	or Registry.CurrentUser. Default - Registry.LocalMachine

parameter 2: company name. Default - null

parameter 3: applacation product name. Default - Application.ProductName

myRegistry.SubKey:	"SOFTWARE\\[my company name]\\[my applacation product name]";

OK, now you can read, write, and delete from your registry.

###To read:

Read any kind of the registry value.

`myRegistry.Read("MY_KEY", [default value]);`

Read the string kind of the registry value.

`myRegistry.ReadString("MY_KEY", [default value]);`

Read the 32-bit binary number kind of the registry value.

`myRegistry.ReadDWord("MY_KEY", [default value]);`

Note: if MY_KEY doesn't exist, the Read family functions will be create key and return default value.

###To write:

Write any kind of the registry value.

`myRegistry.Write("MY_KEY", MY_VALUE);`

Write the string kind of the registry value.

`myRegistry.WriteString("MY_KEY", "MY_VALUE");`

Write the 32-bit binary number kind of the registry value.

`myRegistry.WriteDWord("MY_KEY", number);`

Note: if the SubKey doesn't exist, it will be automatically created.

###To delete a single key:

`myRegistry.DeleteKey("MY_KEY");`

Note: also if the MY_KEY doesn't exist, this function will return TRUE.

###To delete the subkey tree:

`myRegistry.DeleteSubKeyTree();`

Note: this code will delete the SubKey and all its children.

###To retrieve the count of subkeys at the current key:

`myRegistry.SubKeyCount();`

###To retrieve the count of values in the key:

`myRegistry.ValueCount();`

###Thanks to [Francesco Natali](mailto:fn.varie@libero.it). [Read, write and delete from registry with C#](https://www.codeproject.com/articles/3389/read-write-and-delete-from-registry-with-c)
