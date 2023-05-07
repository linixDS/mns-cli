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

        protected LibraryManager libSystem;

        public Route()
        {
        }
        
        public Route(string moduleName, string className, string methodName)
        {
            this.ModuleName = moduleName;
            this.ClassName = className.ToUpper() + "CLASS";
            if (methodName != null)
                this.MethodName = methodName.ToLower();

            libSystem = new LibraryManager();
            libSystem.Loaded();
        }


        public void SetParams(string[] values)
        {
            inputValues = new object[values.Length];
            for (int idx = 0; idx < values.Length; idx++)
                inputValues[idx] = values[idx];
        }

  

        public bool IsModuleExists(string moduleName)
        {
            var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "modules" +
                Path.DirectorySeparatorChar + moduleName +
                Path.DirectorySeparatorChar + moduleName + ".dll";

            if (!File.Exists(path))
                return false;
            else
                return true;

        }

        private bool CheckDependences(string moduleName)
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "modules" + Path.DirectorySeparatorChar + moduleName;
                if (!Directory.Exists(path))
                {
                    Terminal.ErrorWrite("No found "+path);
                    return false;
                }

                var list = libSystem.GetFilesModules(path);
                return libSystem.IsComparisonDependens(list);

            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error module: " + error.Message);
                return false;
            }
        }

        private Assembly LoadModule(string moduleName)
        {
            try
            {

                var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "modules" +
                                Path.DirectorySeparatorChar + moduleName +
                                Path.DirectorySeparatorChar + moduleName + ".dll";


                return Assembly.LoadFile(path);
            }
            catch (Exception error)
            {
                Terminal.ErrorWrite("Error module: " + error.Message);
                return null;
            }
        }

        private void SetRootModuleDirectory(string moduleName)
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "modules" +
                               Path.DirectorySeparatorChar + moduleName;
                Directory.SetCurrentDirectory(path);
            }
            catch (Exception error) 
            {
                Terminal.ErrorWrite("Error: " + error.Message);
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

        public Assembly SelectModule(string moduleName)
        {
            if (IsModuleExists(moduleName))
            {
                var result = LoadModule(moduleName);

                return result;
            }
            else
            {
                Terminal.ErrorWrite("Error: No found module: "+moduleName);
                return null;
            }
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
                module = LoadModule(ModuleName);
                if (module == null) return -1;

                if (!CheckDependences(ModuleName)) return -2;

                SetRootModuleDirectory(ModuleName);
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
