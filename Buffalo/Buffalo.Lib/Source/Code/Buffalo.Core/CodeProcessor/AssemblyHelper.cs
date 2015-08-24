using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Buffalo.Core.CodeProcessor
{
    public class AssemblyHelper
    {
        Dictionary<string, Assembly> _AssemblyPool = new Dictionary<string, Assembly>();

        public AssemblyHelper()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            _AssemblyPool.Add("current", currentAssembly);
        }

        public bool Action_InsertAssembly(string keyNameOfAssembly,string assemblyFilePath)
        {
            if (keyNameOfAssembly != "" && assemblyFilePath != "")
            {
                if (!_AssemblyPool.ContainsKey(keyNameOfAssembly))
                {
                    try
                    {
                        Assembly assemblyObj = Assembly.LoadFrom(assemblyFilePath);
                        _AssemblyPool.Add(keyNameOfAssembly, assemblyObj);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            else
                return false;
        }

        public List<Type> Action_GetType(string KeyOfAssembly)
        {
            List<Type> Result = new List<Type>();
            if (KeyOfAssembly != "" && _AssemblyPool.ContainsKey(KeyOfAssembly))
            {
                Assembly activeAssembly = _AssemblyPool[KeyOfAssembly];
                Type[] allTypes = activeAssembly.GetTypes();
                foreach (Type activeTypeItem in allTypes)
                    Result.Add(activeTypeItem);
            }
            return Result;
        }

        public List<MethodInfo> Action_GetMethods(string KeyOfAssembly,string NamespaceWithClass = "")
        {
            List<MethodInfo> Result=new List<MethodInfo>();
            if(KeyOfAssembly!="" && _AssemblyPool.ContainsKey(KeyOfAssembly))
            {
                Assembly activeAssembly = _AssemblyPool[KeyOfAssembly];
                Type activeType;
                if (NamespaceWithClass != "")
                    activeType = activeAssembly.GetType(NamespaceWithClass);
                else
                    activeType = activeAssembly.GetType();
                if(activeType!=null)
                {
                    MethodInfo[] methodList = activeType.GetMethods();
                    foreach(MethodInfo methodItem in methodList)                    
                        Result.Add(methodItem);                    
                }
            }
            return Result;
        }

    }
}
