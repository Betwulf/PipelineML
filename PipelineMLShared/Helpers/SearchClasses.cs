using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace PipelineMLCore
{
    /// <summary>
    /// This class allows the user to generically search for any class that 
    /// implements ISearchableClass plus an interface of their choice. 
    /// Uses reflection, not fast.
    /// </summary>
    public static class SearchClasses
    {
        static bool SearchDirectory = false; // TODO: TURN OFF FOR NOW, enable later when you want to pursue add on libraries

        public static List<Type> SearchForClassesThatImplementInterface(Type interfaceType)
        {
            return SearchForClassesThatImplementInterface(interfaceType, Environment.CurrentDirectory, null);
        }


        public static List<Type> SearchForClassesThatImplementInterface(Type interfaceType, string searchDirectory)
        {
            return SearchForClassesThatImplementInterface(interfaceType, searchDirectory, null);
        }


        public static List<Type> SearchForClassesThatImplementInterface(Type interfaceType, string searchDirectory, Predicate<Type> filter)
        {
            List<Type> returnArray = new List<Type>();

            // Before looking through DLLs, check exe
            var localAsses = AppDomain.CurrentDomain.GetAssemblies();
            var subAsses = localAsses.Where(x => x.IsDynamic == false); // Exception thrown if you try to use a dynamic assembly
            var list = subAsses.SelectMany( assembly => assembly.GetExportedTypes())
                .Where( t => interfaceType.IsAssignableFrom(t))
                .Where( t => typeof(ISearchableClass).IsAssignableFrom(t));
            foreach (var item in list)
            {
                if (filter != null)
                {
                    // then try filter first
                    if (filter.Invoke(item))
                    {
                        returnArray.Add(item);
                    }
                }
                else
                {
                    // No filter, just add found types
                    returnArray.Add(item);
                }
            }


            // Get DLLs
            if (SearchDirectory)
            {
                if (!Directory.Exists(searchDirectory))
                {
                    throw new Exception($"Directory does not exist [{searchDirectory}]");
                }
                string[] dirs = Directory.GetFiles(searchDirectory, "*.dll", SearchOption.AllDirectories);
                foreach (string dir in dirs)
                {
                    try
                    {
                        Assembly lAss = Assembly.LoadFrom(dir);
                        string lAssName = lAss.GetName().Name;
                        Type[] lTypes = lAss.GetTypes();
                        bool lHasSearchableInterface = false;
                        bool lHasRequiredInterface = false;
                        Type lFoundSearchInterfaceType;
                        Type lFoundRequiredInterfaceType;

                        foreach (Type t in lTypes)
                        {
                            lHasRequiredInterface = false;
                            lHasSearchableInterface = false;

                            Type[] lInterfaces = t.GetInterfaces();
                            foreach (Type lInterface in lInterfaces)
                            {
                                //the following line uses a set of methods which identify what
                                //kind of type we are currently querying
                                if (lInterface.FullName == typeof(ISearchableClass).FullName)
                                {
                                    // we found a required interface
                                    lHasRequiredInterface = true;
                                    lFoundRequiredInterfaceType = t;
                                }
                                if (lInterface.FullName == interfaceType.FullName)
                                {
                                    // we found a match
                                    lHasSearchableInterface = true;
                                    lFoundSearchInterfaceType = t;
                                }
                            }

                            // Did we find a mat ch?
                            if (lHasRequiredInterface && lHasSearchableInterface)
                            {
                                // YES!
                                // Check if we should filter
                                if (filter != null)
                                {
                                    // then try filter first
                                    if (filter.Invoke(t))
                                    {
                                        returnArray.Add(t);
                                    }
                                }
                                else
                                {
                                    // No filter, just add found types
                                    returnArray.Add(t);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // eat it
                        e.ToString();
                    }

                }
            }

            return returnArray;
        }


    }
}
