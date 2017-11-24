using System;
using System.Linq;
using System.Collections.Generic;
using SldWorks;
using SwConst;
using SolidCompare.Comparators;

namespace SolidCompare.Entities
{
    public class Assembly : AbstractEntity
    {
        private IAssemblyDoc swAssembly = default(IAssemblyDoc);
        private List<Assembly> subAssemblies = new List<Assembly>();
        private List<Part> parts = new List<Part>();
        private List<Mate> mates = new List<Mate>();
        private List<BodyFeature> bodyFeatures = new List<BodyFeature>();

        public Assembly(IAssemblyDoc swAssembly) : this(swAssembly, null)
        {

        }

        public Assembly(IAssemblyDoc swAssembly, IFeature relatedFeature) : base(relatedFeature)
        {
            this.swAssembly = swAssembly;
            Initialize();
        }

        private void Initialize()
        {
            IFeature swFeature = ((IModelDoc2)swAssembly).FirstFeature();

            while (swFeature != null)
            {
                Console.WriteLine("+++++ " + swFeature.Name + " - " + swFeature.GetID() + " - " + swFeature.IsSuppressed2(
                    (int)swInConfigurationOpts_e.swThisConfiguration, null)[0]
                    + "\n \t ++ Created: " + swFeature.DateCreated + "\n \t ++ Modified: " + swFeature.DateModified
                + "\n \t ++ CreatedBy: " + swFeature.CreatedBy);
                var swEntity = (IEntity)swFeature;
                var swEntityType = (swSelectType_e)swEntity.GetType();
                var featureTypeName = swFeature.GetTypeName2();
                var featureDefinition = swFeature.GetDefinition();
                var specificFeature = swFeature.GetSpecificFeature2();

                if (swSelectType_e.swSelBODYFEATURES == swEntityType)
                {
                    bodyFeatures.Add(new BodyFeature(swFeature)); // swFeature.GetDefinition()
                    // Console.WriteLine("++++ " + swFeature.GetTypeName2() + " - " + swFeature.Name + " - " + (swFeature.GetDefinition() is IExtrudeFeatureData2));
                }

                if (swSelectType_e.swSelMATEGROUPS == swEntityType)
                {
                    //Get first mate, which is a subfeature
                    IFeature swMateFeature = (Feature)swFeature.GetFirstSubFeature();
                    while (null != swMateFeature)
                    {
                        // Console.WriteLine(swMateFeature.IsSuppressed2(1, null)[0]);
                        mates.Add(new Mate(swMateFeature));
                        swMateFeature = swMateFeature.GetNextSubFeature();
                    }
                }

                if (swSelectType_e.swSelCOMPONENTS == swEntityType)
                {
                    var swModelDoc = ((IComponent2)swFeature.GetSpecificFeature2()).GetModelDoc2();
                    // Console.WriteLine(((IComponent2)swFeature.GetSpecificFeature2()).IsSuppressed());

                    if (swModelDoc is IAssemblyDoc)
                    {
                        SwApp.ActivateDocument(swModelDoc);
                        subAssemblies.Add(new Assembly((IAssemblyDoc)swModelDoc, swFeature));
                    }

                    if (swModelDoc is IPartDoc)
                    {
                        parts.Add(new Part((IPartDoc)swModelDoc, swFeature));
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

        public override CompareResult CompareTo(AbstractEntity target)
        {
            List<AbstractEntity> a = parts.Cast<AbstractEntity>().ToList();
            return ListComparator.Instance.Compare(parts.Cast<AbstractEntity>().ToList(), ((Assembly)target).GetParts().Cast<AbstractEntity>().ToList());
        }

        public List<Assembly> GetSubAssemblies()
        {
            return subAssemblies;
        }

        public List<Part> GetParts()
        {
            return parts;
        }

        public List<Mate> GetMates()
        {
            return mates;
        }

        public List<BodyFeature> GetBodyFeatures()
        {
            return bodyFeatures;
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
