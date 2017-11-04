using System;
using System.Runtime.InteropServices;
using SldWorks;
using SwConst;
using System.Collections.Generic;

namespace SolidCompare
{
    public class Assembly
    {
        private string directory;
        private IAssemblyDoc swAssembly = default(IAssemblyDoc);
        private List<IAssemblyDoc> swSubAssemblies = new List<IAssemblyDoc>();
        private List<IPartDoc> swParts = new List<IPartDoc>();
        private List<IMate2> swMates = new List<IMate2>();
        private List<IFeature> swBodyFeatures = new List<IFeature>();

        public Assembly(IAssemblyDoc swAssembly)
        {
            this.swAssembly = swAssembly;
            Initialize();
        }

        private void Initialize()
        {
            IFeature swFeature = ((IModelDoc2)this.swAssembly).FirstFeature();

            while ((swFeature != null))
            {
                Console.WriteLine("++++ TypeName: " + swFeature.GetSpecificFeature2() + " - Typename2: " + swFeature.GetTypeName2()
                    + " - Name: " + swFeature.Name);
                swFeature = swFeature.GetNextFeature();
            }
        }

        // Return all parts without suppressed
        public List<Object> GetComponents()
        {
            return new List<Object>((Object[])swAssembly.GetComponents(false));
        }

        // Return the assembly's directory
        public List<Object> GetDirectory()
        {
            // TBD
            return null;
        }

        // Return all files used by the assembly
        public List<Object> GetUsedFiles()
        {
            // TBD
            return null;
        }

        // Returns all assembly-wise features
        public List<Object> GetFeatures()
        {
            // TBD
            return null;
        }
    }
}
