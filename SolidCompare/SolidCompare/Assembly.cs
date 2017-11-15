using System;
using System.Runtime.InteropServices;
using SldWorks;
using SwConst;
using System.Collections.Generic;

namespace SolidCompare
{
    public class Assembly
    {
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

            while (swFeature != null)
            {
                var swEntity = (IEntity)swFeature;
                var swEntityType = (swSelectType_e)swEntity.GetType();
                var featureTypeName = swFeature.GetTypeName2();
                var featureDefinition = swFeature.GetDefinition();
                var specificFeature = swFeature.GetSpecificFeature2();
                
                if (swSelectType_e.swSelBODYFEATURES == swEntityType)
                {
                    swBodyFeatures.Add(swFeature); // swFeature.GetDefinition()
                }

                if (swSelectType_e.swSelMATEGROUPS == swEntityType)
                {
                    //Get first mate, which is a subfeature
                    IFeature swMate = (Feature)swFeature.GetFirstSubFeature();
                    while (null != swMate)
                    {
                        swMates.Add(swMate.GetSpecificFeature2());
                        swMate = swMate.GetNextSubFeature();
                    }
                }

                if (swSelectType_e.swSelCOMPONENTS == swEntityType)
                {                   
                    var swModelDoc = ((IComponent2)swFeature.GetSpecificFeature2()).GetModelDoc2();

                    if (swModelDoc is IAssemblyDoc)
                    {
                        swSubAssemblies.Add((IAssemblyDoc)swModelDoc);
                    }

                    if (swModelDoc is IPartDoc)
                    {
                        swParts.Add((IPartDoc)swModelDoc);
                    }
                }

                swFeature = swFeature.GetNextFeature();
            }
        }

        // Return all parts without suppressed
        /*public List<Object> GetComponents()
        {
            return new List<Object>((Object[])swAssembly.GetComponents(false));
        }
        */

        public List<IAssemblyDoc> GetSubAssemblies()
        {
            return swSubAssemblies;
        }

        public List<IPartDoc> GetParts()
        {
            return swParts;
        }

        public List<IMate2> GetMates()
        {
            return swMates;
        }

        public List<IFeature> GetBodyFeatures()
        {
            return swBodyFeatures;
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
