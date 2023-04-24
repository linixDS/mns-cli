using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Core;

namespace Runtime
{
    public class Route
    {
        protected string ModuleName = null;
        protected string ClassName = null;
        protected string MethodName = null;

        protected object[] inputValues = null;

        public Route()
        {
        }
        
        public Route(string moduleName, string className, string methodName)
        {
            this.ModuleName = moduleName;
            this.ClassName = className.ToUpper() + "CLASS";
            if (methodName != null)
                this.MethodName = methodName.ToLower();
        }

        public void SetParams(string[] values)
        {
            inputValues = new object[values.Length];
            for (int idx = 0; idx < values.Length; idx++)
                inputValues[idx] = values[idx];
        }

        
        private Assembly LoadModule()
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "modules" +
                                Path.DirectorySeparatorChar + ModuleName +
                                Path.DirectorySeparatorChar + ModuleName + ".dll";
                return Assembly.LoadFile(path);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error module: " + error.Message);
                return null;
            }
        }

        private void SetRootDirectory()
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "modules" +
                               Path.DirectorySeparatorChar + ModuleName;
                Directory.SetCurrentDirectory(path);
            }
            catch (Exception) 
            { 
            }
        }


        private Type LoadClass(Assembly module)
        {
            try
            {
                Debug.WriteLine(ModuleName + "." + ClassName);
                return module.GetType(ModuleName + "." + ClassName);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error class: " + error.Message);
                return null;
            }
        }

        public Assembly SelectModule(string module)
        {
            ModuleName = module;
            var result = LoadModule();
            if (result == null)
                ModuleName = null;
            return result;
        }

        public int Run()
        {
            if (ModuleName == null)
            {
                Terminal.ErrorWrite("Error: No define module name!");
                return -1;
            }
            Assembly module = null;
            Type classType = null;
            object c = null;

            try
            {
                module = LoadModule();
                if (module == null) return -1;
                SetRootDirectory();
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error (Run:LoadModule): " + error.Message);
                return -1;
            }

            try
            {
                classType = LoadClass(module);
                if (classType == null)
                {
                   Terminal.ErrorWrite("Error: No found class "+ClassName);
                   return -1;
                }

                c = Activator.CreateInstance(classType);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error (Run:LoadClass): "+classType + " - " + error.Message);
                return -1;
            }

            try 
            { 
                if (MethodName != null)
                {
                    var method = classType.GetMethod(MethodName);

                    if (inputValues == null)
                        method.Invoke(c, null);
                    else
                        method.Invoke(c, inputValues);
                }
                return 0;

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error (Run:GetMethod): " + MethodName+" - "+error.Message);
                return -1;
            }
        }
    }
}
