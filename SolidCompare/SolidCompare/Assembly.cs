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


        public Assembly()
        {

        }

        public Assembly(IAssemblyDoc swAssembly)
        {
            this.swAssembly = swAssembly;
            Initialize();
        }

        public Assembly(string dir)
        {
            directory = dir;
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

        public string Directory
        {
            get { return directory; }
            set { directory = value; }
        }

        // Return all parts without suppressed
        public List<Object> GetComponents()
        {
            return new List<Object>((Object[])swAssembly.GetComponents(false));
        }
    }
}
